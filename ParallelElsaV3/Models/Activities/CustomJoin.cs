using ParallelElsaV3.Interfaces;

namespace ParallelElsaV3.Models.Activities
{
    public class CustomJoin : Node, IJoin
    {
        public CustomJoin(string text) : base(text)
        {
        }

        public List<CounterItem> Counters { get; set; } = new List<CounterItem>();

        public void ResetCounters()
        {
            
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
            if(Counters.All(i => i.Counter > 3)) {
                return ScheduleNextActivities(connections, activationToken);
            }
            return ScheduleNoNewActivities(connections, activationToken);
        }

        private ExecutionResult ScheduleNoNewActivities(Connections connections, ActivationToken activationToken)
        {
            return new ExecutionResult(connections.GetOutboundConnectedNodes(this).Skip(1).ToNodes(), activationToken);
        }

        private ExecutionResult ScheduleNextActivities(Connections connections, ActivationToken activationToken)
        {
            return new ExecutionResult(connections.GetOutboundConnectedNodes(this).Take(1).ToNodes(), activationToken);
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
}

