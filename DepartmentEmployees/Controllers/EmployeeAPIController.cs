using DepartmentEmployees.Data;
using DepartmentEmployees.Models;
using DepartmentEmployees.Models.Dto;
using DepartmentEmployees.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentEmployees.Controllers
{
	[Route("api/EmployeeAPI")]
	[ApiController]
	public class EmployeeAPIController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public EmployeeAPIController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<EmployeeDTO>> GetEmployees()
		{
			/*List<Employee> objEmployeeList = _unitOfWork.Employee.GetAll().ToList();
			return Ok(ListOfEmployees.employeeList);*/
			return Ok(ListOfEmployees.employeeList);
		}

		[HttpGet("{id:int}", Name = "GetEmployee")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<EmployeeDTO> GetEmployee(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var employee = ListOfEmployees.employeeList.FirstOrDefault(u => u.Id == id);

			if (employee == null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<EmployeeDTO> CreateEmployee([FromBody] EmployeeDTO employeeDTO)
		{
			/*if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}*/
			if (ListOfEmployees.employeeList.FirstOrDefault(u => u.FullName.ToLower() == employeeDTO.FullName.ToLower()) != null)
			{
				ModelState.AddModelError("CustomError", "Employee already Exists!");
				return BadRequest(ModelState);
			}

			if (employeeDTO == null)
			{
				return BadRequest(employeeDTO);
			}
			if (employeeDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			employeeDTO.Id = ListOfEmployees.employeeList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			ListOfEmployees.employeeList.Add(employeeDTO);

			return CreatedAtRoute("GetEmployee", new { id = employeeDTO.Id }, employeeDTO);
		}

		[HttpDelete("{id:int}", Name = "DeleteEmployee")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult DeleteEmployee(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var employee = ListOfEmployees.employeeList.FirstOrDefault(u => u.Id == id);
			if (employee == null)
			{
				return NotFound();
			}
			ListOfEmployees.employeeList.Remove(employee);
			return NoContent();
		}

		[HttpPut("{id:int}", Name = "UpdateEmployee")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
		{
			if (employeeDTO == null || id != employeeDTO.Id)
			{
				return BadRequest();
			}
			var employee = ListOfEmployees.employeeList.FirstOrDefault(u => u.Id == id);
			employee.FullName = employeeDTO.FullName;
			employee.Post = employeeDTO.Post;

			return NoContent();
		}

		[HttpPatch("{id:int}", Name = "UpdatePartialEmployee")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}
			var employee = ListOfEmployees.employeeList.FirstOrDefault(u => u.Id == id);
			if (employee == null)
			{
				return BadRequest();
			}
			patchDTO.ApplyTo(employee, ModelState);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();
		}
	}
}
