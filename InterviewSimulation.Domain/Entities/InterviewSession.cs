using System;
using System.Collections.Generic;
using InterviewSimulation.Domain.Entities;

namespace InterviewSimulation.Domain.Entities
{
	public class InterviewSession : BaseEntity
	{
		public Guid UserId { get; set; }
		public string Status { get; set; }
		public DateTime StartedAt { get; set; }
		public DateTime? CompletedAt { get; set; }
		public bool IsDeleted { get; set; }

		public ICollection<SessionQuestionAnswer> SessionQuestionAnswers { get; set; }
		public ICollection<InterviewAnswer> InterviewAnswers { get; set; }
	}
}


