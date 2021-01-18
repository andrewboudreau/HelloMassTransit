# HelloWorld to MassTransit. 

Instead of `Order` we consider `Document` and the operations which can be performed upon it.
An example that explores just the messaging parts of a system. Provides a sandbox for exploring the workings of [MassTransit](https://masstransit-project.com/) and how it works with Azure ServiceBus.

# Documents
The API is the entry point for processing and coordinating operations upon documents. Services are capable of coordinating through the document API and each other.

# Setting up the secrets.
## Why? Because the example is for AzureServiceBus, a hosted service.
The example builds upon MassTransit but specifically explores how the system works with Azure ServiceBus. Therefor you need an Azure ServiceBus since there is not an emulator.

## Got a Secret? AzureServiceBus ConnectionString.
Get an [Azure ServiceBus account](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview). And use [ServiceBusExplorer](https://github.com/paolosalvatori/ServiceBusExplorer).

## How, Secret Store
I used `dotnet user-secrets` and it was simple and easy. Check out the [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=windows&view=aspnetcore-5.0), but I think my initial reference was [How to add and use user secrets to a .NET Core Console App](https://medium.com/@granthair5/how-to-add-and-use-user-secrets-to-a-net-core-console-app-a0f169a8713f). This [StackOverflow article](https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application) was quite helpful as well.

