using MediatR;
using InterviewSimulation.Contract.Shared;

namespace InterviewSimulation.Contract.Message
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>;
}
