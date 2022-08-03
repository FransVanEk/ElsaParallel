namespace ParallelElsaV3.Models
{
    public class CustomProperties : Dictionary<string, object>
    {
        internal void Upsert(string contextKeyPropertyName, object value)
        {
            if(ContainsKey(contextKeyPropertyName))
            {
                this[contextKeyPropertyName] = value;  
            }
            else
            {
                this.Add(contextKeyPropertyName, value);
            }
        }
    }
}