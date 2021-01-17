using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace HelloService.Contracts.Requests
{
    public interface DocumentOperationCommand
    {
        int DocumentIdentifier { get; }

        DetailSetting Settings { get; }
    }

    public interface DetailSetting
    {
        bool AnotherBooleanValue { get; }

        string BackgroundColors { get; }
    }
}
