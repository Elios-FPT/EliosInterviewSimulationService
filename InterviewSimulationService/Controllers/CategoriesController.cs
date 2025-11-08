using Asp.Versioning;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static InterviewSimulation.Contract.UseCases.Category.Command;
using static InterviewSimulation.Contract.UseCases.Category.Query;
using static InterviewSimulation.Contract.UseCases.Category.Request;

namespace InterviewSimulation.Web.Controllers
{
    [ApiVersion(1)]
    [Produces("application/json")]
    [ControllerName("Categories")]
    [Route("api/interviewsimulation/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponseDto<CategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<BaseResponseDto<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand(
                Name: request.Name);
            return await _sender.Send(command);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponseDto<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<BaseResponseDto<CategoryDto>> GetCategory([FromRoute] Guid id)
        {
            var query = new GetCategoryByIdQuery(Id: id);
            return await _sender.Send(query);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponseDto<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<BaseResponseDto<IEnumerable<CategoryDto>>> GetCategories([FromQuery] GetAllCategoriesRequest request)
        {
            var query = new GetAllCategoriesQuery(
                PageNumber: request.PageNumber,
                PageSize: request.PageSize);
            return await _sender.Send(query);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponseDto<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<BaseResponseDto<CategoryDto>> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand(
                Id: id,
                Name: request.Name);
            return await _sender.Send(command);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<BaseResponseDto<bool>> DeleteCategory([FromRoute] Guid id)
        {
            var command = new DeleteCategoryCommand(Id: id);
            return await _sender.Send(command);
        }
    }
}
