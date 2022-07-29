using ElsaParallel.Client.Models;

namespace ElsaParallel.Client.Engine
{
    public class ProcessDefinition
    {
        public Nodes Nodes { get; set; } = new Nodes();

        public Connections Connections { get; set; } = new Connections();

    }
}
