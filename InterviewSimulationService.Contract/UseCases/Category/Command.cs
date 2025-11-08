using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using System;

namespace InterviewSimulation.Contract.UseCases.Category
{
    public static class Command
    {
        public record CreateCategoryCommand(
            string Name
        ) : ICommand<BaseResponseDto<CategoryDto>>;

        public record UpdateCategoryCommand(
            Guid Id,
            string Name
        ) : ICommand<BaseResponseDto<CategoryDto>>;

        public record DeleteCategoryCommand(
            Guid Id
        ) : ICommand<BaseResponseDto<bool>>;
    }
}
