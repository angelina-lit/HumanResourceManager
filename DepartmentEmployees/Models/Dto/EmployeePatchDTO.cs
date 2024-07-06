﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Dto
{
	public class EmployeePatchDTO
	{
		[Required]
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string Post { get; set; }
	}
}