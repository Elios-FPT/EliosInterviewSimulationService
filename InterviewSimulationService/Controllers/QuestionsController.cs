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

        
    }
}
