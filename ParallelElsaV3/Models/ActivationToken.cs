namespace ParallelElsaV3.Models
{
    public class ActivationToken 
    {
        public Node? PreviousNode { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();

        public ActivationToken Clone()
        {
            return new ActivationToken() { CustomProperties = this.CustomProperties, PreviousNode = PreviousNode };
        }
    }
}

