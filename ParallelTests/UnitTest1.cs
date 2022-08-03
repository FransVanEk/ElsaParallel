using NUnit.Framework;
using ParallelElsaV3;
using ParallelElsaV3.Engine;
using ParallelElsaV3.Interfaces;
using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ContextAwareWaitAll()
        {

           var processDefinition = new  ProcessDefinition()
              .AddNode(new Start("Begin"))
              .AddNode(new Fork("Fork on"))
              .AddNode(new ContextActivity("Context Activity 1", "ContextId"))
              .AddNode(new ContextActivity("Context Activity 2", "ContextId"))
              .AddNode(new ContextJoin("Join", "ContextId", true, eJoinExecutionType.WaitForAll))
              .AddNode(new Activity("Final"))
              .AddNode(new End("Stop"))

                .AddConnection("Begin", "Fork on")
                .AddConnection("Fork on", "Fork on")
                .AddConnection("Fork on", "Context Activity 1")
                .AddConnection("Fork on", "Context Activity 2")
                .AddConnection("Context Activity 1", "Join")
                .AddConnection("Context Activity 2", "Join")
                .AddConnection("Join", "Final")
                .AddConnection("Final", "Stop");

            var engine = new ExecutionEngine(processDefinition);
            engine.ScheduleStartNodes();
            while (engine.NodesToExecute.Count > 0)
            {
                engine.PerformStep();
            }

        }
    }
}