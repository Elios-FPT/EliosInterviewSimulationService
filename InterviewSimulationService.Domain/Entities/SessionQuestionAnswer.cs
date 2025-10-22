using System;
using InterviewSimulationService.Domain.Entities;

namespace InterviewSimulationService.Domain.Entities
{
	public class SessionQuestionAnswer : BaseEntity
	{
		public Guid SessionId { get; set; }
		public Guid QuestionId { get; set; }
		public string QuestionContent { get; set; }
		public int OrderNo { get; set; }

		public InterviewSession Session { get; set; }
		public Question Question { get; set; }
	}
}


