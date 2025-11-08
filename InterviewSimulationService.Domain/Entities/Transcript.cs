using System;
using InterviewSimulation.Domain.Entities;

namespace InterviewSimulation.Domain.Entities
{
	public class Transcript : BaseEntity
	{
		public string TranscriptText { get; set; }
		public string Language { get; set; }
		public DateTime CreatedAt { get; set; }

		public InterviewAnswer InterviewAnswer { get; set; }
	}
}


