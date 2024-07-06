using System.Net;

namespace DepartmentEmployees.Models
{
	public class APIResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public bool IsSuccess { get; set; } = true;
		public List<string> ErrorMessages { get; set; }
		public object Result { get; set; }
	}
}
