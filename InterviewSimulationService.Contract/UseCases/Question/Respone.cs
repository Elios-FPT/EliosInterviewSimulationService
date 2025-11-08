using InterviewSimulation.Contract.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewSimulation.Contract.UseCases.Question
{
    public record QuestionRespone
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = default!;
        public int Difficulty { get; set; }
        public string? QuestionVideoUrl { get; set; }
    }
    public record GetListQuestionsRespone
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public int Difficulty { get; set; }
    }
}
