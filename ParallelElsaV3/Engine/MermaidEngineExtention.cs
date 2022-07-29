using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;
using System.Text;

namespace ParallelElsaV3.Engine
{
    public static class MermaidEngineExtention
    {
        public static string ToMermaidGraph(this ExecutionEngine engine, string direction)
        {
            var result = new StringBuilder();
            result.AppendLine($"graph {direction}");
            engine.ProcessDefinition.Connections.ForEach(connection =>
            {
                result.AppendLine($"{GetMermaidTag(engine, connection.From)}-->{GetMermaidTag(engine, connection.To)}");
            });
            return result.ToString();
        }

        private static string GetMermaidTag(ExecutionEngine engine, INode node)
        {
            var indexName = $"id{engine.ProcessDefinition.Nodes.IndexOf(node)}";
            switch (node)
            {
                case Start t1:
                    return $"{indexName}(({node.Text}))";
                case End t2:
                    return $"{indexName}(({node.Text}))";
                case Fork t3:
                    return $"{indexName}{{{node.Text}}}";
                case Join t4:
                    return $"{indexName}{{{node.Text}}}";
                    default:
                    return $"{indexName}[{node.Text}]";
            }
        }
    }
}
