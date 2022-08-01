using ParallelElsaV3.Engine;

namespace ParallelElsaV3.Models
{
    public class ExecutionResult
    {
        public ExecutionResult(Nodes nodes, ActivationToken activationToken)
        {
            Nodes = nodes;
            ActivationToken = activationToken;
            ClearAllScheduledNodes = false; 
        }

        public ExecutionResult(Nodes nodes, ActivationToken activationToken, bool  clearAllScheduledNodes)
        {
            Nodes = nodes;
            ActivationToken = activationToken;
            ClearAllScheduledNodes = clearAllScheduledNodes;
        }


        public Nodes Nodes { get; set; }

        public ActivationToken ActivationToken { get; set; }
        public bool ClearAllScheduledNodes { get; set; } = false;
    }

    public static class ExecutionResultExtensions
    {
        public static ExecutionItems ToExecutionItems(this ExecutionResult executionResult)
        {
            var result = new ExecutionItems();
            if (executionResult.Nodes.Count > 1)
            {
                executionResult.Nodes.ForEach(node => result.Add(new ExecutionItem(node, executionResult.ActivationToken.Clone())));
            }
            else
            {
                executionResult.Nodes.ForEach(node => result.Add(new ExecutionItem(node, executionResult.ActivationToken)));

            }
            return result;
        }
    }
}