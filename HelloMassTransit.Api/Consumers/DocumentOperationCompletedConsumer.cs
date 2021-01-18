using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloService.Contracts.Events;
using MassTransit;

namespace HelloMassTransit.Api.Consumers
{
    public class DocumentOperationCompletedConsumer
        : IConsumer<DocumentOperationCompleted>
    {
        public Task Consume(ConsumeContext<DocumentOperationCompleted> context)
        {
            Console.WriteLine($"ProcessId {Environment.ProcessId}: Consumer:{GetType().Name} handles Value:{context.Message.CompleteValue} Text:{context.Message.OperationCompletedData.OperationOutputData}");
            return Task.CompletedTask;
        }
    }

    public class OtherDocumentOperationCompletedConsumer
       : IConsumer<DocumentOperationCompleted>
    {
        public Task Consume(ConsumeContext<DocumentOperationCompleted> context)
        {
            Console.WriteLine($"ProcessId {Environment.ProcessId}: Consumer:{GetType().Name} handles Value:{context.Message.CompleteValue} Text:{context.Message.OperationCompletedData.OperationOutputData}");
            return Task.CompletedTask;
        }
    }
}
