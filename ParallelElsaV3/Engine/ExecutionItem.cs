using ParallelElsaV3.Models;

namespace ParallelElsaV3.Engine
{
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