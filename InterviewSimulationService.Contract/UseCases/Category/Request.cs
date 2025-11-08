using System;
using System.ComponentModel.DataAnnotations;

namespace InterviewSimulation.Contract.UseCases.Category
{
    public static class Request
    {
        public record CreateCategoryRequest(
            [Required] string Name
        );

        public record UpdateCategoryRequest(
            [Required] string Name
        );

        public record GetAllCategoriesRequest(
            int PageNumber = 1,
            int PageSize = 20
        );
    }
}
