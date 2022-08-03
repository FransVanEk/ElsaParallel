using ParallelElsaV3.Models;
using ParallelElsaV3.Models.Activities;

namespace ParallelElsaV3.Interfaces
{
    public interface IJoin
    {
        void ResetCounters(Connections conntections);

        ItemCounters GetCounters(); // only needed for the UI
    }
}