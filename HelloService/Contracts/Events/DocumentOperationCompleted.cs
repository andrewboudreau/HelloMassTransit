using System;
using System.Collections.Generic;
using System.Text;

namespace HelloService.Contracts.Events
{
    public interface DocumentOperationCompleted
    {
        int CompleteValue { get; }

        OperationCompletedData OperationCompletedData { get; }
    }

    public interface OperationCompletedData
    {
        string OperationOutputData { get; }
    }
}
