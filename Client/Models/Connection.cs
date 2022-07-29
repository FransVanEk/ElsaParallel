namespace ElsaParallel.Client.Models
{
    public class Connections : List<Connection>
    {
        internal Nodes GetConnectedNodes(INode node)
        {
            return this
                .Where(i => i.From == node)
                .Select(i => i.To)
                .ToNodes();
        }
    }

    public class Connection
    {
        public Connection(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public Node From { get; set; }

        public Node To { get; set; }

        public bool IsActive { get; set; } = false;

    }
}
