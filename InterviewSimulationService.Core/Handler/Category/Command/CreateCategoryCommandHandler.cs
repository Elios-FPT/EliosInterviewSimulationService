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
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, BaseResponseDto<CategoryDto>>
    {
        private readonly IGenericRepository<InterviewSimulation.Domain.Entities.Category> _repository;

        public CreateCategoryCommandHandler(IGenericRepository<InterviewSimulation.Domain.Entities.Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
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
                using var transaction = await _repository.BeginTransactionAsync();
                try
                {
                    var entity = new InterviewSimulation.Domain.Entities.Category
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                    };

                    await _repository.AddAsync(entity);
                    await transaction.CommitAsync();

                    return new BaseResponseDto<CategoryDto>
                    {
                        Status = 201,
                        Message = "Category created successfully.",
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
                    Message = $"Failed to create category: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
