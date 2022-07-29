namespace ParallelElsaV3.Models
{

    public abstract class Node : INode
    {
        public Node(string text)
        {
            Text = text;
        }
        public string Text { get; set; }


        public virtual ExecutionResult Execute(Connections connections, ActivationToken activationToken)
        {
            activationToken.PreviousNode = this;
            return new ExecutionResult(connections.GetOutboundConnectedNodes(this), activationToken);
        }
    }
}

