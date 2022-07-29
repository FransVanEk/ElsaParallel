namespace ElsaParallel.Client.Models
{
    public static class NodesExtensions
    {
        public static Nodes ToNodes(this IEnumerable<INode> nodes)
        {
            return new Nodes(nodes);
        }
    }
}

