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
using static InterviewSimulation.Contract.UseCases.Category.Query;

namespace InterviewSimulation.Core.Handler.Category.Query
{
    public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, BaseResponseDto<IEnumerable<CategoryDto>>>
    {
        private readonly IGenericRepository<InterviewSimulation.Domain.Entities.Category> _repository;

        public GetAllCategoriesQueryHandler(IGenericRepository<InterviewSimulation.Domain.Entities.Category> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BaseResponseDto<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return new BaseResponseDto<IEnumerable<CategoryDto>>
                {
                    Status = 400,
                    Message = "Page number and page size must be positive.",
                    ResponseData = null
                };
            }

            try
            {
                var entities = await _repository.GetListAsync(
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber);

                var dtos = entities.Select(e => e.ToDto()).ToList();

                return new BaseResponseDto<IEnumerable<CategoryDto>>
                {
                    Status = 200,
                    Message = dtos.Any() ? "Categories retrieved successfully." : "No categories found.",
                    ResponseData = dtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<IEnumerable<CategoryDto>>
                {
                    Status = 500,
                    Message = $"Failed to retrieve categories: {ex.Message}",
                    ResponseData = null
                };
            }
        }
    }
}
