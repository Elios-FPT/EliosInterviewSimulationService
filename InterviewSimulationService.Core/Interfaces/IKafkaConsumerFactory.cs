using InterviewSimulation.Core.Interfaces;

namespace InterviewSimulation.Infrastructure.Kafka
{
    public interface IKafkaConsumerFactory<T> where T : class
    {
        IKafkaConsumerRepository<T> CreateConsumer(string sourceServiceName);
    }
}