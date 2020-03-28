using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Monitor.StepperLogic
{
    public class StepFunctionTasks
    {
        public StepFunctionTasks()
        {
        }

        public State RetrieveRecords(State state, ILambdaContext context)
        {
            state.Message = "RetrieveRecords => ";
            return state;
        }

        public State MonitorRecords(State state, ILambdaContext context)
        {
            state.Message += "MonitorRecords => ";
            return state;
        }

        public State ProcessRecords(State state, ILambdaContext context)
        {
            state.Message += "ProcessRecords";
            return state;
        }
    }
}
