namespace ElsaParallel.Client.Models
{
    public interface INode
    {
        ExecutionResult Execute(Connections connections, ActivationToken activationToken);


    }
}