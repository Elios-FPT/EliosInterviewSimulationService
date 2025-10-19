using System;
using InterviewSimulationService.Domain.Entities;

namespace InterviewSimulationService.Domain.Entities
{
	public class InterviewAnswer : BaseEntity
	{
		public Guid UserId { get; set; }
		public Guid SessionId { get; set; }
		public Guid SessionQuestionId { get; set; }
		public string ProcessingStatus { get; set; }
		public string? AnswerAudioUrl { get; set; }
		public string? Prefix { get; set; }
		public string? Filename { get; set; }
		public string? PublicUrl { get; set; }
		public DateTime CreatedAt { get; set; }

		public InterviewSession Session { get; set; }
		public SessionQuestionAnswer SessionQuestion { get; set; }
		public Transcript Transcript { get; set; }
	}
}


