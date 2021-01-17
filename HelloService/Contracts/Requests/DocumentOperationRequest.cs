using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace HelloService.Contracts.Requests
{
    public interface DocumentOperationRequest
    {
        int DocumentIdentifier { get; }

        OperationSettings Settings { get; }
    }

    public interface OperationSettings
    {
        bool ShouldThisWork { get; }

        string CustomOutputName { get; }
    }
}
