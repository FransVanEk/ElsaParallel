namespace ParallelElsaV3.Models
{
    public class Connections : List<Connection>
    {
        public Nodes GetOutboundConnectedNodes(INode node)
        {
            return this
                .Where(i => i.From == node)
                .Select(i => i.To)
                .ToNodes();
        }

        public Nodes GetInboundConnectedNodes(INode node)
        {
            return this
                .Where(i => i.To == node)
                .Select(i => i.From)
                .ToNodes();
        }
    }

    public class Connection
    {
        public Connection(INode from, INode to)
        {
            From = from;
            To = to;
        }

        public INode From { get; set; }

        public INode To { get; set; }

    }
}
