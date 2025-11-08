using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Contract.UseCases.Question;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Question.Command;

namespace InterviewSimulation.Core.Handler.Question.Command
{
    public class CreateQuestionCommandHandler : ICommandHandler<CreateQuestionCommand, BaseResponseDto<QuestionRespone>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;
        private readonly ISUtilityServiceClient _utilityServiceClient;

        public CreateQuestionCommandHandler(IGenericRepository<Domain.Entities.Question> repository, ISUtilityServiceClient utilityServiceClient)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _utilityServiceClient = utilityServiceClient;
        }

        public async Task<BaseResponseDto<QuestionRespone>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId == Guid.Empty)
            {
                return new BaseResponseDto<QuestionRespone>
                {
                    Status = 400,
                    Message = "CategoryId cannot be empty.",
                    ResponseData = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return new BaseResponseDto<QuestionRespone>
                {
                    Status = 400,
                    Message = "Title cannot be null or empty.",
                    ResponseData = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.QuestionText))
            {
                return new BaseResponseDto<QuestionRespone>
                {
                    Status = 400,
                    Message = "QuestionText cannot be null or empty.",
                    ResponseData = null
                };
            }

            if (request.Difficulty < 1 || request.Difficulty > 5)
            {
                return new BaseResponseDto<QuestionRespone>
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
                    var keyPrefix = $"interview/{Guid.NewGuid().ToString()}";
                    var uploadedUrl = await _utilityServiceClient.UploadFileAsync(keyPrefix, request.FilesToUpload, cancellationToken);

                    if (string.IsNullOrEmpty(uploadedUrl))
                    {
                        return new BaseResponseDto<QuestionRespone> { Status = 400, Message = $"Failed to upload file: {request.FilesToUpload.FileName}. Question creation cancelled.", ResponseData = default };
                    }

                    var entity = new Domain.Entities.Question
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = request.CategoryId,
                        Title = request.Title,
                        Difficulty = request.Difficulty,
                        QuestionText = request.QuestionText,
                        QuestionVideoUrl = uploadedUrl,
                        Prefix = request.Prefix,
                        Filename = request.Filename,
                        PublicUrl = uploadedUrl,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _repository.AddAsync(entity);
                    await transaction.CommitAsync();

                    var respone = new QuestionRespone()
                    {
                        Id = entity.Id,
                        CategoryId = entity.CategoryId,
                        Title = entity.Title,
                        Difficulty = entity.Difficulty,
                        QuestionVideoUrl = entity.QuestionVideoUrl,
                    };

                    return new BaseResponseDto<QuestionRespone>
                    {
                        Status = 201,
                        Message = "Question created successfully.",
                        ResponseData = respone
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
                return new BaseResponseDto<QuestionRespone>
                {
                    Status = 500,
                    Message = $"Failed to create question: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
