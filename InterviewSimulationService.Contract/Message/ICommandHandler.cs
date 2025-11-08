using MediatR;
using InterviewSimulation.Contract.Shared;

namespace InterviewSimulation.Contract.Message
{
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>;
}
