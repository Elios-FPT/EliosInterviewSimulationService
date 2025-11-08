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
    public class CreateQuestionCommandHandler : ICommandHandler<CreateQuestionCommand, BaseResponseDto<QuestionDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;

        public CreateQuestionCommandHandler(IGenericRepository<Domain.Entities.Question> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<QuestionDto>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
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
                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    var entity = new Domain.Entities.Question
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = request.CategoryId,
                        Title = request.Title,
                        Difficulty = request.Difficulty,
                        QuestionText = request.QuestionText,
                        QuestionVideoUrl = request.QuestionVideoUrl,
                        Prefix = request.Prefix,
                        Filename = request.Filename,
                        PublicUrl = request.PublicUrl,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _repository.AddAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<QuestionDto>
                    {
                        Status = 201,
                        Message = "Question created successfully.",
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
                    Message = $"Failed to create question: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
