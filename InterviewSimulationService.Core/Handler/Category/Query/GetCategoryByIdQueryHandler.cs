using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Core.Extensions;
using InterviewSimulation.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static InterviewSimulation.Contract.UseCases.Category.Query;

namespace InterviewSimulation.Core.Handler.Category.Query
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, BaseResponseDto<CategoryDto>>
    {
        private readonly IGenericRepository<InterviewSimulation.Domain.Entities.Category> _repository;

        public GetCategoryByIdQueryHandler(IGenericRepository<InterviewSimulation.Domain.Entities.Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
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

                return new BaseResponseDto<CategoryDto>
                {
                    Status = 200,
                    Message = "Category retrieved successfully.",
                    ResponseData = entity.ToDto()
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<CategoryDto>
                {
                    Status = 500,
                    Message = $"Failed to retrieve category: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
