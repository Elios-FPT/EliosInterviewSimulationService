using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Question.Command;

namespace InterviewSimulation.Core.Handler.Question.Command
{
    public class UpdateQuestionCommandHandler : ICommandHandler<UpdateQuestionCommand, BaseResponseDto<QuestionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;

        public UpdateQuestionCommandHandler(IGenericRepository<Domain.Entities.Question> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<QuestionDto>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
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

            if (request.CategoryId == Guid.Empty)
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 400,
                    Message = "CategoryId cannot be empty.",
                    ResponseData = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 400,
                    Message = "Title cannot be null or empty.",
                    ResponseData = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.QuestionText))
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 400,
                    Message = "QuestionText cannot be null or empty.",
                    ResponseData = null
                };
            }

            if (request.Difficulty < 1 || request.Difficulty > 5)
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 400,
                    Message = "Difficulty must be between 1 and 5.",
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

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    entity.CategoryId = request.CategoryId;
                    entity.Title = request.Title;
                    entity.Difficulty = request.Difficulty;
                    entity.QuestionText = request.QuestionText;
                    entity.QuestionVideoUrl = request.QuestionVideoUrl;
                    entity.Prefix = request.Prefix;
                    entity.Filename = request.Filename;
                    entity.PublicUrl = request.PublicUrl;
                    entity.IsActive = request.IsActive;

                    await _repository.UpdateAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<QuestionDto>
                    {
                        Status = 200,
                        Message = "Question updated successfully.",
                        ResponseData = entity.ToDto()
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<QuestionDto>
                {
                    Status = 500,
                    Message = $"Failed to update question: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
