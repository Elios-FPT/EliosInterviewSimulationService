using System;
using System.Collections.Generic;
using InterviewSimulation.Domain.Entities;

namespace InterviewSimulation.Domain.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }

		public ICollection<Question> Questions { get; set; }
	}
}


