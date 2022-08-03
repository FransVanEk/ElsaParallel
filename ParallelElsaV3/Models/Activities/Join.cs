using ParallelElsaV3.Interfaces;
using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelElsaV3.Models.Activities
{
    public class Join : JoinBase
    {
        private ItemCounters counters = new ItemCounters();

        public Join(string text, eJoinExecutionType executionType) : base(text, executionType)
        {
           
        }
        public override ExecutionResult Execute(Connections connections, ActivationToken activationToken)
        {
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

       

        private bool CheckIfAllHaveCount(int count)
        {
            return counters.All(counter => counter.Counter >= count);
        }

        private bool CheckHighestNumber(int count)
        {
            return counters.Count(n => n.Counter == count) ==1 && counters.Max(n=> n.Counter == count);
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
            var counter = counters.First(i => i.Node == previousNode);
            counter.Counter++;
            return counter.Counter;
        }

        internal override void AddCounterForConnectingActivities( Connections connections)
        {
            connections.GetInboundConnectedNodes(this).ForEach(n =>
            {
                counters.Add(new ItemCounter { Node = n, Counter = 0 });
            });
        }

        public override ItemCounters GetCounters()
        {
            return counters;
        }
    }

    public class ItemCounters : List<ItemCounter>
    { }


    public class ItemCounter
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

    public abstract class JoinBase: Node, IJoin
    {
        public eJoinExecutionType JoinExecutionType { get; private set; }

        protected JoinBase(string text, eJoinExecutionType joinExecutionType) : base(text)
        {
            JoinExecutionType = joinExecutionType;
        }

        public abstract override ExecutionResult Execute(Connections connections, ActivationToken activationToken);

        public void ResetCounters(Connections connections)
        {
            AddCounterForConnectingActivities(connections);
        }

        internal abstract void AddCounterForConnectingActivities(Connections connections);

        public abstract ItemCounters GetCounters();
    }
}