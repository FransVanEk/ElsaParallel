using ElsaParallel.Client.Engine;

namespace ElsaParallel.Client.Models
{
    public class ExecutionResult
    {
        public ExecutionResult(Nodes nodes, ActivationToken activationToken)
        {
            Nodes = nodes;
            ActivationToken = activationToken;
        }


        public Nodes Nodes { get; set; }

        public ActivationToken ActivationToken { get; set; }
    }

    public static class ExecutionResultExtensions
    {
        public static ExecutionItems ToExecutionItems(this ExecutionResult executionResult)
        {
            var result = new ExecutionItems();
            executionResult.Nodes.ForEach(node => result.Add(new ExecutionItem(node, executionResult.ActivationToken)));
            return result;
        }
    }
}