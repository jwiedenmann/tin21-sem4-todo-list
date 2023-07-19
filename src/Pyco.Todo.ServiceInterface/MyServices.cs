using System;
using ServiceStack;
using Pyco.Todo.ServiceModel;

namespace Pyco.Todo.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
