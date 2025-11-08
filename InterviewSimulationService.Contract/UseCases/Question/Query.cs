using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using System;
using System.Collections.Generic;

namespace InterviewSimulation.Contract.UseCases.Question
{
    public static class Query
    {
        public record GetQuestionByIdQuery(
            Guid Id
        ) : IQuery<BaseResponseDto<QuestionDto>>;

        public record GetAllQuestionsQuery(
            int PageNumber = 1,
            int PageSize = 20,
            Guid? CategoryId = null,
            int? Difficulty = null,
            bool? IsActive = null
        ) : IQuery<BaseResponseDto<IEnumerable<GetListQuestionsRespone>>>;
    }
}
