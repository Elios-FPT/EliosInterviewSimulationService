using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Question.Query;

namespace InterviewSimulation.Core.Handler.Question.Query
{
    public class GetQuestionByIdQueryHandler : IQueryHandler<GetQuestionByIdQuery, BaseResponseDto<QuestionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;

        public GetQuestionByIdQueryHandler(IGenericRepository<Domain.Entities.Question> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<QuestionDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 400,
                    Message = "ID cannot be empty.",
                    ResponseData = null
                };
            }

            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    return new BaseResponseDto<QuestionDto>
                    {
                        Status = 404,
                        Message = "Question not found.",
                        ResponseData = null
                    };
                }

                return new BaseResponseDto<QuestionDto>
                {
                    Status = 200,
                    Message = "Question retrieved successfully.",
                    ResponseData = entity.ToDto()
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 500,
                    Message = $"Failed to retrieve question: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
