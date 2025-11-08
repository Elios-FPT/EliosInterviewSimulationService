using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Question.Command;

namespace InterviewSimulation.Core.Handler.Question.Command
{
    public class DeleteQuestionCommandHandler : ICommandHandler<DeleteQuestionCommand, BaseResponseDto<bool>>
    {
        private readonly IGenericRepository<Domain.Entities.Question> _repository;

        public DeleteQuestionCommandHandler(IGenericRepository<Domain.Entities.Question> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponseDto<bool>
                {
                    Status = 400,
                    Message = "ID cannot be empty.",
                    ResponseData = false
                };
            }

            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    return new BaseResponseDto<bool>
                    {
                        Status = 404,
                        Message = "Question not found.",
                        ResponseData = false
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    entity.IsActive = false;
                    await _repository.UpdateAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<bool>
                    {
                        Status = 200,
                        Message = "Question deleted successfully.",
                        ResponseData = true
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
                return new BaseResponseDto<bool>
                {
                    Status = 500,
                    Message = $"Failed to delete question: {ex.Message}",
                    ResponseData = false
                };
            }
        }
    }
}
