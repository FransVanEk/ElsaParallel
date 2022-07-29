namespace ParallelElsaV3.Models
{
    public class Nodes : List<INode>
    {
        public Nodes()
        {

        }

        public bool Exists (string name)
        {
            return this.Any(n => n.Text == name);
        }

        public INode this[string name]
        {
            get { return this.First(n => n.Text == name); }
        }

        public Nodes AddNode(INode node)
        {
            this.Add(node);
            return this;
        }

        public Nodes(IEnumerable<INode> nodes)
        {
            AddRange(nodes);
        }
    }
}

