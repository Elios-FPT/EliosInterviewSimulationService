using MediatR;
using InterviewSimulation.Contract.Shared;

namespace InterviewSimulation.Contract.Message
{
    public interface IQuery<TResponse> : IRequest<TResponse>;
}
