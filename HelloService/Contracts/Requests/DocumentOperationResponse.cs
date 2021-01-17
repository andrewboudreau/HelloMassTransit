using System;
using System.Collections.Generic;
using System.Text;

namespace HelloService.Contracts.Requests
{
    public interface DocumentOperationResponse
    {
        string FormattedDocument { get; }
    }
}
