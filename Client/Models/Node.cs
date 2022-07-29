namespace ElsaParallel.Client.Models
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
            return new ExecutionResult(connections.GetConnectedNodes(this), activationToken);
        }
    }
}

