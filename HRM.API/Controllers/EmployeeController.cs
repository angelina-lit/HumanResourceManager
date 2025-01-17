﻿using HRM.DataAccess.Repository.IRepository;
using HRM.Models;
using HRM.Models.Employee;
using HRM.Models.Employee.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRM.API.Controllers
{
	[Route("api/Employee")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly APIResponse _response;

		public EmployeeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_response = new();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<APIResponse> GetEmployees()
		{
			try
			{
				IEnumerable<Employee> employeeList = _unitOfWork.Employee.GetAllAsync().Result;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Result = employeeList;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpGet("{id:int}", Name = "GetEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetEmployee(int id)
		{
			try
			{
				if (id == 0)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id);

				if (employee == null)
				{
					_response.ErrorMessages = new List<string>() { "There is no employee with this ID" };
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				_response.StatusCode = HttpStatusCode.OK;
				_response.Result = employee;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> CreateEmployee([FromBody] EmployeeCreateDTO createDTO)
		{
			try
			{
				if (await _unitOfWork.Employee.GetAsync(u => u.FullName.ToLower() == createDTO.FullName.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessage", "An employee with the same name already exists");
					return BadRequest(ModelState);
				}

				if (createDTO == null) return BadRequest();

				Employee model = new()
				{
					FullName = createDTO.FullName,
					Role = createDTO.Role,
				};

				await _unitOfWork.Employee.CreateAsync(model);
				await _unitOfWork.SaveAsync();
				_response.Result = model;
				_response.StatusCode = HttpStatusCode.Created;
				return CreatedAtRoute("GetEmployee", new { id = model.Id }, _response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpDelete("{id:int}", Name = "DeleteEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> DeleteEmployee(int id)
		{
			try
			{
				if (id == 0) return BadRequest();

				var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id);

				if (employee == null) return NotFound();

				await _unitOfWork.Employee.RemoveAsync(employee);
				await _unitOfWork.SaveAsync();
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPut("{id:int}", Name = "UpdateEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO updateDTO)
		{
			try
			{
				if (updateDTO == null || id != updateDTO.Id) return BadRequest();

				if (updateDTO.FullName != null)
				{
					_response.ErrorMessages = new List<string>() { "Changing the FullName field is prohibited, the previous value is retained" };
				}

				var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id, tracked: false);

				Employee model = new()
				{
					Id = updateDTO.Id,
					FullName = employee.FullName,
					Role = updateDTO.Role,
				};

				await _unitOfWork.Employee.UpdateAsync(model);
				await _unitOfWork.SaveAsync();
				_response.Result = model;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPatch("{id:int}", Name = "UpdatePartialEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeUpdateDTO> patchDTO)
		{
			if (patchDTO == null || id == 0) return BadRequest();

			if (patchDTO.Operations.Any(op => op.path.ToLower().Equals("/fullname", StringComparison.OrdinalIgnoreCase)))
			{
				_response.ErrorMessages = new List<string>() { "Changing the FullName field is prohibited, the previous value is retained" };
			}

			var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id, tracked: false);

			EmployeeUpdateDTO employeeDTO = new()
			{
				Id = employee.Id,
				Role = employee.Role,
			};

			if (employee == null) return BadRequest(_response);

			patchDTO.ApplyTo(employeeDTO, ModelState);

			Employee model = new()
			{
				Id = employeeDTO.Id,
				FullName = employee.FullName,
				Role = employeeDTO.Role,
			};

			if (!ModelState.IsValid) return BadRequest(_response);

			await _unitOfWork.Employee.UpdateAsync(model);
			await _unitOfWork.SaveAsync();
			_response.Result = model;
			_response.StatusCode = HttpStatusCode.OK;
			return Ok(_response);
		}
	}
}
