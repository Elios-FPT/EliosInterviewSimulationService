using InterviewSimulation.Contract.Message;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using System;
using System.Collections.Generic;

namespace InterviewSimulation.Contract.UseCases.Category
{
    public static class Query
    {
        public record GetCategoryByIdQuery(
            Guid Id
        ) : IQuery<BaseResponseDto<CategoryDto>>;

        public record GetAllCategoriesQuery(
            int PageNumber = 1,
            int PageSize = 20
        ) : IQuery<BaseResponseDto<IEnumerable<CategoryDto>>>;
    }
}
