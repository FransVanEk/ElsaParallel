using ParallelElsaV3.Interfaces;
using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelElsaV3.Engine
{
    public class ExecutionEngine
    {
        public ExecutionEngine(ProcessDefinition processDefinition)
        {
            ProcessDefinition = processDefinition;
            ScheduleStartNodes();
            SetCounters();
        }

        private void SetCounters()
        {
            ProcessDefinition.Nodes.Where(n => n is IJoin).ToList().ForEach(n => ((IJoin)n).ResetCounters(ProcessDefinition.Connections));
        }

        public void ScheduleStartNodes()
        {

            ProcessDefinition
                .Nodes
                .Where(n => n is Start)
                .ToList()
                .ForEach(n => NodesToExecute
                                .Add(new ExecutionItem(n, new ActivationToken())));
        }

        public ProcessDefinition ProcessDefinition { get; }

        public void PerformStep()
        {
            if (NodesToExecute.Count > 0)
            {
                var itemToExecute = NodesToExecute.First();
                var result = itemToExecute.Node.Execute(ProcessDefinition.Connections, itemToExecute.ActivationToken);
                NodesToExecute.RemoveAt(0);
                if (result.ClearAllScheduledNodes) { NodesToExecute.Clear(); }
                NodesToExecute.AddRange(result.ToExecutionItems());
            }
        }

        public void Reset()
        {
            ProcessDefinition.Reset();
            NodesToExecute = new ExecutionItems();
        }

        public ExecutionItems NodesToExecute = new ExecutionItems();

    }
}