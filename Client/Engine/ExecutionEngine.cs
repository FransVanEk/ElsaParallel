using ElsaParallel.Client.Models;
using ElsaParallel.Client.Models.Activities;

namespace ElsaParallel.Client.Engine
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

        private ExecutionItems NodesToExecute = new ExecutionItems();

    }

    public class ExecutionItems : List<ExecutionItem>
    {

    }
    public class ExecutionItem
    {
        public ExecutionItem(INode node, ActivationToken activationToken)
        {
            Node = node;
            ActivationToken = activationToken;
        }

        public INode Node { get; set; }
        public ActivationToken ActivationToken { get; set; }
    }
}

