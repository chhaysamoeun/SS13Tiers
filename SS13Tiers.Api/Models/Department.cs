using System;
using System.ComponentModel.DataAnnotations;

namespace SS13Tiers.Api.Models
{
	public class Department
	{
		[Key]
		public Guid DepartmentId { get; set; }
		[Required]
		[MaxLength(50)]
		public string DepartmentName { get; set; }
	}
}

