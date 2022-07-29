namespace ElsaParallel.Client.Models
{
    public class Nodes : List<INode>
    {
        public Nodes()
        {

        }

        public Nodes(IEnumerable<INode> nodes)
        {
            AddRange(nodes);
        }
    }
}

