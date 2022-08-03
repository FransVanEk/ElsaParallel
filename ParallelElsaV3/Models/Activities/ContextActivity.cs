namespace ParallelElsaV3.Models.Activities
{
    public class ContextActivity : Node
    {
        public ContextActivity(string text, string contextKeyPropertyName) : base(text)
        {
            ContextKeyPropertyName = contextKeyPropertyName;
        }

        public string ContextKeyPropertyName { get; }

        public override ExecutionResult Execute(Connections connections, ActivationToken activationToken)
        {
            activationToken.CustomProperties.Upsert(ContextKeyPropertyName, new Random().Next(1, 10));
            return base.Execute(connections, activationToken);
        }
    }
}

