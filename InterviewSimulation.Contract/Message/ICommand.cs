using MediatR;
using InterviewSimulation.Contract.Shared;

namespace InterviewSimulation.Contract.Message
{
    public interface ICommand<TResponse> : IRequest<TResponse>;
}
