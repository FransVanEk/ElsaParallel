using ParallelElsaV3.Interfaces;

namespace ParallelElsaV3.Models.Activities
{
    public class ContextJoin : JoinBase
    {
        private ContextCounters contextCounters = new ContextCounters();

        public ContextJoin(string text, string contextKeyPropertyName, bool removeScheduledItemsAfterJoin, eJoinExecutionType executionType) : base(text, executionType)
        {
            ContextKeyPropertyName = contextKeyPropertyName;
            RemoveScheduledItemsAfterJoin = removeScheduledItemsAfterJoin;
        }

        public string ContextKeyPropertyName { get; }
        public bool RemoveScheduledItemsAfterJoin { get; }

        public override ExecutionResult Execute(Connections connections, ActivationToken activationToken)
        {
            ItemCounters contextCounters = GetContextCounter(connections, activationToken);
            var count = IncreaseCounterFor(activationToken.PreviousNode, contextCounters);
            return GetExectutionResult(count, connections, activationToken, contextCounters);
        }

        private ItemCounters GetContextCounter(Connections connections, ActivationToken activationToken)
        {
            var contextValue = activationToken.CustomProperties[ContextKeyPropertyName];
            if (!contextCounters.ContainsKey(contextValue))
            {
                contextCounters.Add(contextValue, new ItemCounters());
                connections.GetInboundConnectedNodes(this).ForEach(n =>
                {
                    contextCounters[contextValue].Add(new ItemCounter { Node = n, Counter = 0 });
                });

            }
            return contextCounters[contextValue];
        }

        private ExecutionResult GetExectutionResult(int count, Connections connections, ActivationToken activationToken, ItemCounters contextCounter)
        {
            switch (JoinExecutionType)
            {
                case eJoinExecutionType.ContinueAlways:
                    return ScheduleNextActivities(connections, activationToken);
                    break;
                case eJoinExecutionType.WaitForAny:
                    if (CheckHighestNumber(count, contextCounter))
                    {
                        return ScheduleNextActivities(connections, activationToken);
                    }
                    break;
                case eJoinExecutionType.WaitForAll:
                    if (CheckIfAllHaveCount(count, contextCounter))
                    {
                        return ScheduleNextActivities(connections, activationToken);
                    }
                    break;

            }
            return ScheduleNoNewActivities(activationToken);
        }



        private bool CheckIfAllHaveCount(int count, ItemCounters contextCounters)
        {
            return contextCounters.All(counter => counter.Counter >= count);
        }

        private bool CheckHighestNumber(int count, ItemCounters contextCounters)
        {
            return contextCounters.Count(n => n.Counter == count) == 1 && contextCounters.Max(n => n.Counter == count);
        }

        private ExecutionResult ScheduleNoNewActivities(ActivationToken activationToken)
        {
            return new ExecutionResult(new Nodes(), activationToken);
        }

        private ExecutionResult ScheduleNextActivities(Connections connections, ActivationToken activationToken)
        {
            return new ExecutionResult(connections.GetOutboundConnectedNodes(this), activationToken) { ClearAllScheduledNodes = RemoveScheduledItemsAfterJoin };
        }

        private int IncreaseCounterFor(Node? previousNode, ItemCounters contextcounter)
        {
            var counter = contextcounter.First(i => i.Node == previousNode);
            counter.Counter++;
            return counter.Counter;
        }

        internal override void AddCounterForConnectingActivities(Connections connections)
        {

        }

        public override ItemCounters GetCounters()
        {
            var result = new ItemCounters();

            //  contextCounters.Keys.ToList().ForEach(k => result.AddRange(contextCounters[result]));
            return result;
        }
    }
}

