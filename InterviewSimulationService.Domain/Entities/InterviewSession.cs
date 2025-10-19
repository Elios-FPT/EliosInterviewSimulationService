using System;
using System.Collections.Generic;
using InterviewSimulationService.Domain.Entities;

namespace InterviewSimulationService.Domain.Entities
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


