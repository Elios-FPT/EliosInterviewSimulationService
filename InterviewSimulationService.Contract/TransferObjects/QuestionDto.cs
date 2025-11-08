using System;

namespace InterviewSimulation.Contract.TransferObjects
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = default!;
        public int Difficulty { get; set; }
        public string QuestionText { get; set; } = default!;
        public string? QuestionVideoUrl { get; set; }
        public string? Prefix { get; set; }
        public string? Filename { get; set; }
        public string? PublicUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
