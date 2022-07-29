using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelElsaV3.Engine
{
    public class ExecutionEngine
    {
        public ExecutionEngine(ProcessDefinition processDefinition)
        {
            ProcessDefinition = processDefinition;
            FindStartNodes();
        }

        private void FindStartNodes()
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
                NodesToExecute.AddRange(itemToExecute.Node.Execute(ProcessDefinition.Connections, itemToExecute.ActivationToken).ToExecutionItems());
                NodesToExecute.RemoveAt(0);
            }
        }

        public ExecutionItems NodesToExecute = new ExecutionItems();

    }
}