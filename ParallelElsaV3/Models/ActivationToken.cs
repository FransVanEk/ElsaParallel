namespace ParallelElsaV3.Models
{
    public class ActivationToken 
    {
        public Node? PreviousNode { get; set; }
        public  CustomProperties CustomProperties { get; set; } = new CustomProperties();

        public ActivationToken Clone()
        {
            return new ActivationToken() { CustomProperties = this.CustomProperties, PreviousNode = PreviousNode };
        }
    }
}

