using ForumService.Contract.TransferObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace InterviewSimulation.Contract.UseCases.Question
{
    public static class Request
    {
        public record CreateQuestionRequest(
            [Required] Guid CategoryId,
            [Required] string Title,
            [Required] int Difficulty,
            [Required] string QuestionText,
            string? Prefix,
            string? Filename
        );

        public record UpdateQuestionRequest(
            [Required] Guid CategoryId,
            [Required] string Title,
            [Required] int Difficulty,
            [Required] string QuestionText,
            string? QuestionVideoUrl,
            string? Prefix,
            string? Filename,
            string? PublicUrl,
            [Required] bool IsActive
        );

        public record GetAllQuestionsRequest(
            int PageNumber = 1,
            int PageSize = 20,
            Guid? CategoryId = null,
            int? Difficulty = null,
            bool? IsActive = null
        );
    }
}
