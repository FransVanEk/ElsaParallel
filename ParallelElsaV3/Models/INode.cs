namespace ParallelElsaV3.Models
{
    public interface INode
    {
        string Text { get; }
        ExecutionResult Execute(Connections connections, ActivationToken activationToken);
    }
}