using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Category.Command;

namespace InterviewSimulation.Core.Handler.Category.Command
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, BaseResponseDto<CategoryDto>>
    {
        private readonly IGenericRepository<InterviewSimulation.Domain.Entities.Category> _repository;

        public UpdateCategoryCommandHandler(IGenericRepository<InterviewSimulation.Domain.Entities.Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                return new BaseResponseDto<CategoryDto>
                {
                    Status = 400,
                    Message = "ID cannot be empty.",
                    ResponseData = null
                };
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new BaseResponseDto<CategoryDto>
                {
                    Status = 400,
                    Message = "Name cannot be null or empty.",
                    ResponseData = null
                };
            }

            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                {
                    return new BaseResponseDto<CategoryDto>
                    {
                        Status = 404,
                        Message = "Category not found.",
                        ResponseData = null
                    };
                }

                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    entity.Name = request.Name;
                    await _repository.UpdateAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<CategoryDto>
                    {
                        Status = 200,
                        Message = "Category updated successfully.",
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
                return new BaseResponseDto<CategoryDto>
                {
                    Status = 500,
                    Message = $"Failed to update category: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
