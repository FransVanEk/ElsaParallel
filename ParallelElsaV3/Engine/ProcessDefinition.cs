using ParallelElsaV3.Interfaces;
using ParallelElsaV3.Models;

namespace ParallelElsaV3.Engine
{
    public class ProcessDefinition
    {

        public ProcessDefinition AddNode(INode node)
        {
            Nodes.Add(node);
            return this;
        }

        public Nodes Nodes { get; set; } = new Nodes();

        public ProcessDefinition AddConnection(INode from, INode to)
        {
            Connections.Add(new Connection(from, to));
            return this;
        }

        public ProcessDefinition AddConnection(string from, string to)
        {
            return AddConnection(Nodes[from], Nodes[to]);
        }

        public Connections Connections { get; set; } = new Connections();

        internal void Reset()
        {
            Nodes.Where(n => n is IJoin).ToList().ForEach(n => ((IJoin)n).ResetCounters(Connections));
        }
    }
}
