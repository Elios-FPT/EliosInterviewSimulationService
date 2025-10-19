using System;
using System.Collections.Generic;
using InterviewSimulationService.Domain.Entities;

namespace InterviewSimulationService.Domain.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }

		public ICollection<Question> Questions { get; set; }
	}
}


