using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using System;

namespace InterviewSimulation.Contract.UseCases.Question
{
    public static class Command
    {
        public record CreateQuestionCommand(
            Guid CategoryId,
            string Title,
            int Difficulty,
            string QuestionText,
            string? QuestionVideoUrl,
            string? Prefix,
            string? Filename,
            string? PublicUrl
        ) : ICommand<BaseResponseDto<QuestionDto>>;

        public record UpdateQuestionCommand(
            Guid Id,
            Guid CategoryId,
            string Title,
            int Difficulty,
            string QuestionText,
            string? QuestionVideoUrl,
            string? Prefix,
            string? Filename,
            string? PublicUrl,
            bool IsActive
        ) : ICommand<BaseResponseDto<QuestionDto>>;

        public record DeleteQuestionCommand(
            Guid Id
        ) : ICommand<BaseResponseDto<bool>>;
    }
}
