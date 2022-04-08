using StressAlgorithmService.Models;

namespace StressAlgorithmService.Interfaces
{
    public interface IHRVAlgorithm
    {
        public List<HeartRateMeasurement> CalculateHRV(List<HeartRateMeasurement> heartRates);
    }
}
