using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Category.Command;

namespace InterviewSimulation.Core.Handler.Category.Command
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, BaseResponseDto<bool>>
    {
        private readonly IGenericRepository<InterviewSimulation.Domain.Entities.Category> _repository;

        public DeleteCategoryCommandHandler(IGenericRepository<InterviewSimulation.Domain.Entities.Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
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
                        Message = "Category not found.",
                        ResponseData = false
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    await _repository.UpdateAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<bool>
                    {
                        Status = 200,
                        Message = "Category deleted successfully.",
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
                    Message = $"Failed to delete category: {ex.Message}",
                    ResponseData = false
                };
            }
        }
    }
}
