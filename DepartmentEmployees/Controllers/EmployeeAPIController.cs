using DepartmentEmployees.Models;
using DepartmentEmployees.Models.Dto;
using DepartmentEmployees.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace DepartmentEmployees.Controllers
{
	[Route("api/EmployeeAPI")]
	[ApiController]
	public class EmployeeAPIController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly APIResponse _response;

		public EmployeeAPIController(IUnitOfWork unitOfWork)
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
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
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
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<APIResponse>> CreateEmployee([FromBody] EmployeeCreateDTO createDTO)
		{
			try
			{
				if (await _unitOfWork.Employee.GetAsync(u => u.FullName.ToLower() == createDTO.FullName.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessage", "An employee with the same name already exists");
					return BadRequest(ModelState);
				}

				if (createDTO == null)
					return BadRequest();

				Employee model = new()
				{
					FullName = createDTO.FullName,
					Post = createDTO.Post,
				};

				await _unitOfWork.Employee.CreateAsync(model);
				//_unitOfWork.Save();
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
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> DeleteEmployee(int id)
		{
			try
			{
				if (id == 0) 
					return BadRequest();

				var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id);

				if (employee == null) 
					return NotFound();

				await _unitOfWork.Employee.RemoveAsync(employee);
				_response.StatusCode = HttpStatusCode.NoContent;
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
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO updateDTO)
		{
			try
			{
				_response.ErrorMessages = new List<string>();
				if (updateDTO.FullName!=null)
				{
					_response.ErrorMessages.Add("Изменение поля FullName запрещено");
				}

				if (updateDTO == null || id != updateDTO.Id) return BadRequest();
				var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id, tracked: false);

				Employee model = new()
				{
					Id = updateDTO.Id,
					FullName = employee.FullName,
					Post = updateDTO.Post,
				};
				
				await _unitOfWork.Employee.UpdateAsync(model);
				_response.Result = model;
				_response.StatusCode = HttpStatusCode.NoContent;
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
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdatePartialEmployee(int id, JsonPatchDocument<EmployeePatchDTO> patchDTO)
		{
			_response.ErrorMessages = new List<string>();
			if (patchDTO.Operations.Any(op => op.path.ToLower().Equals("/fullname", StringComparison.OrdinalIgnoreCase)))
			{
				_response.ErrorMessages.Add("Изменение поля FullName запрещено");
			}

			if (patchDTO == null || id == 0) return BadRequest();
		
			var employee = await _unitOfWork.Employee.GetAsync(u => u.Id == id, tracked: false);

			EmployeePatchDTO employeeDTO = new()
			{
				Id = employee.Id,
				Post = employee.Post,
			};

			if (employee == null) return BadRequest(_response);

			patchDTO.ApplyTo(employeeDTO, ModelState);

			Employee model = new()
			{
				Id = employeeDTO.Id,
				FullName = employee.FullName,
				Post = employeeDTO.Post,
			};

			if (!ModelState.IsValid) return BadRequest(_response);
			await _unitOfWork.Employee.UpdateAsync(model);
			return Ok();
		}
	}
}
