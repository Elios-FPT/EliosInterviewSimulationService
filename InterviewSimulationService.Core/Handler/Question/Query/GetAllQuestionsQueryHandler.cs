using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Question.Query;

namespace InterviewSimulation.Core.Handler.Question.Query
{
    public class GetAllQuestionsQueryHandler : IQueryHandler<GetAllQuestionsQuery, BaseResponseDto<IEnumerable<QuestionDto>>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;

        public GetAllQuestionsQueryHandler(IGenericRepository<Domain.Entities.Question> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<IEnumerable<QuestionDto>>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return new BaseResponseDto<IEnumerable<QuestionDto>>
                {
                    Status = 400,
                    Message = "Page number and page size must be positive.",
                    ResponseData = null
                };
            }

            try
            {
                var entities = await _repository.GetListAsync(
                    filter: q =>
                        (!request.CategoryId.HasValue || q.CategoryId == request.CategoryId.Value) &&
                        (!request.Difficulty.HasValue || q.Difficulty == request.Difficulty.Value) &&
                        (!request.IsActive.HasValue || q.IsActive == request.IsActive.Value),
                    orderBy: query => query.OrderByDescending(q => q.CreatedAt),
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber);

                var dtos = entities.Select(e => e.ToDto()).ToList();

                return new BaseResponseDto<IEnumerable<QuestionDto>>
                {
                    Status = 200,
                    Message = dtos.Any() ? "Questions retrieved successfully." : "No questions found.",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<QuestionDto>>
                {
                    Status = 500,
                    Message = $"Failed to retrieve questions: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
