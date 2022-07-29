namespace ElsaParallel.Client.Models
{
    public class ActivationToken
    {
        public Node? PreviousNode { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();
    }
}

