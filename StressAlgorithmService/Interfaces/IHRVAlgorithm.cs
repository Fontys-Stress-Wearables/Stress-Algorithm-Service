using StressAlgorithmService.Models;

namespace StressAlgorithmService.Interfaces
{
    public interface IHRVAlgorithm
    {
        public int CalculateHRVBasedOnIntervals(int[] intervals);
    }
}
