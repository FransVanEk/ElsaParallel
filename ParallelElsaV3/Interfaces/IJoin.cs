using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelElsaV3.Interfaces
{
    public interface IJoin
    {
        List<CounterItem> Counters { get; set; }

        void ResetCounters(Connections conntections);
    }
}