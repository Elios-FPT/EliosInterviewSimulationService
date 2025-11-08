using Asp.Versioning;
using InterviewSimulation.Contract.Shared;
using InterviewSimulation.Contract.TransferObjects;
using InterviewSimulation.Contract.UseCases.Question;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static InterviewSimulation.Contract.UseCases.Question.Command;
using static InterviewSimulation.Contract.UseCases.Question.Query;
using static InterviewSimulation.Contract.UseCases.Question.Request;

namespace InterviewSimulation.Web.Controllers
{
    [ApiVersion(1)]
    [Produces("application/json")]
    [ControllerName("Questions")]
    [Route("api/interviewsimulation/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ISender _sender;

        public QuestionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponseDto<QuestionRespone>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<BaseResponseDto<QuestionRespone>> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var command = new CreateQuestionCommand(
                CategoryId: request.CategoryId,
                Title: request.Title,
                Difficulty: request.Difficulty,
                QuestionText: request.QuestionText,
                QuestionVideoUrl: request.QuestionVideoUrl,
                Prefix: request.Prefix,
                Filename: request.Filename,
                PublicUrl: request.PublicUrl);
            return await _sender.Send(command);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponseDto<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<BaseResponseDto<QuestionDto>> GetQuestion([FromRoute] Guid id)
        {
            var query = new GetQuestionByIdQuery(Id: id);
            return await _sender.Send(query);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponseDto<IEnumerable<GetListQuestionsRespone>>), StatusCodes.Status200OK)]
        public async Task<BaseResponseDto<IEnumerable<GetListQuestionsRespone>>> GetQuestions([FromQuery] GetAllQuestionsRequest request)
        {
            var query = new GetAllQuestionsQuery(
                PageNumber: request.PageNumber,
                PageSize: request.PageSize,
                CategoryId: request.CategoryId,
                Difficulty: request.Difficulty,
                IsActive: request.IsActive);
            return await _sender.Send(query);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponseDto<QuestionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<BaseResponseDto<QuestionDto>> UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequest request)
        {
            var command = new UpdateQuestionCommand(
                Id: id,
                CategoryId: request.CategoryId,
                Title: request.Title,
                Difficulty: request.Difficulty,
                QuestionText: request.QuestionText,
                QuestionVideoUrl: request.QuestionVideoUrl,
                Prefix: request.Prefix,
                Filename: request.Filename,
                PublicUrl: request.PublicUrl,
                IsActive: request.IsActive);
            return await _sender.Send(command);
        }

    }
}
