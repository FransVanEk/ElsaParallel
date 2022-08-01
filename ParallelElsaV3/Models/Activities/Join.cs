using ParallelElsaV3.Interfaces;
using ParallelElsaV3.Models;

namespace ParallelElsaV3.Models.Activities
{
    public class Join : Node, IJoin
    {
        public List<CounterItem> Counters { get; set; } = new List<CounterItem>();

        public Join(string text, eJoinExecutionType executionType) : base(text)
        {
            JoinExecutionType = executionType;
        }

        public eJoinExecutionType JoinExecutionType { get; private set; }

        public override ExecutionResult Execute(Connections connections, ActivationToken activationToken)
        {
            if (Counters.Count == 0)
            {
                AddCounterForConnectingActivities(connections);
            }

            var count = IncreaseCounterFor(activationToken.PreviousNode);
            return GetExectutionResult(count, connections, activationToken);
        }

        private ExecutionResult GetExectutionResult(int count, Connections connections, ActivationToken activationToken)
        {
            switch (JoinExecutionType)
            {
                case eJoinExecutionType.ContinueAlways:
                    return ScheduleNextActivities(connections, activationToken);
                    break;
                case eJoinExecutionType.WaitForAny:
                    if (CheckHighestNumber(count)){
                        return ScheduleNextActivities(connections, activationToken);
                    }
                    break;
                case eJoinExecutionType.WaitForAll:
                    if (CheckIfAllHaveCount(count)){
                        return ScheduleNextActivities(connections, activationToken);
                    }
                    break;

            }
            return ScheduleNoNewActivities(activationToken);
        }

        public void ResetCounters(Connections connections)
        {
            AddCounterForConnectingActivities(connections);
        }

        private bool CheckIfAllHaveCount(int count)
        {
            return Counters.All(counter => counter.Counter >= count);
        }

        private bool CheckHighestNumber(int count)
        {
            return Counters.Count(n => n.Counter == count) ==1 && Counters.Max(n=> n.Counter == count);
        }

        private ExecutionResult ScheduleNoNewActivities(ActivationToken activationToken)
        {
            return new ExecutionResult(new Nodes(), activationToken);
        }

        private ExecutionResult ScheduleNextActivities(Connections connections, ActivationToken activationToken)
        {
            return new ExecutionResult(connections.GetOutboundConnectedNodes(this), activationToken);
        }

        private int IncreaseCounterFor(Node? previousNode)
        {
            var counter = Counters.First(i => i.Node == previousNode);
            counter.Counter++;
            return counter.Counter;
        }

        private void AddCounterForConnectingActivities(Connections connections)
        {
            connections.GetInboundConnectedNodes(this).ForEach(n =>
            {
                Counters.Add(new CounterItem { Node = n, Counter = 0 });
            });
        }

        
    }

    public class CounterItem
    {
        public INode Node { get; set; }

        public int Counter { get; set; }
    }
}

namespace ParallelElsaV3
{
    public enum eJoinExecutionType
    {
        ContinueAlways,
        WaitForAny,
        WaitForAll,
        CustomJoin
    }
}