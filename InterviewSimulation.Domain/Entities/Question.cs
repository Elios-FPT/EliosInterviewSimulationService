using System;
using System.Collections.Generic;
using InterviewSimulation.Domain.Entities;

namespace InterviewSimulation.Domain.Entities
{
	public class Question : BaseEntity
	{
		public Guid CategoryId { get; set; }
		public string Title { get; set; }
		public int Difficulty { get; set; }
		public string QuestionText { get; set; }
		public string? QuestionVideoUrl { get; set; }
		public string? Prefix { get; set; }
		public string? Filename { get; set; }
		public string? PublicUrl { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }

		public Category Category { get; set; }
		public ICollection<SessionQuestionAnswer> SessionQuestionAnswers { get; set; }
	}
}


