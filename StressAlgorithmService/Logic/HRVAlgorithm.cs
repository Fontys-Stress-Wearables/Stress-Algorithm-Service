using StressAlgorithmService.Interfaces;
using StressAlgorithmService.Models;

namespace StressAlgorithmService.Logic
{
    public class HRVAlgorithm : IHRVAlgorithm
    {
        public int CalculateHRVBasedOnIntervals(int[] intervals)
        {
            int lastInterval = 0;
            List<int> intervalDifferences = new();
            foreach (int interval in intervals)
            {
                // Skip the first interval
                if(lastInterval != 0)
                {
                    intervalDifferences.Add(DifferenceBetweenIntervalsToPower2(lastInterval, interval));
                }
                lastInterval = interval;
            }
            double average = FindAverage(intervalDifferences.ToArray());
            return (int)Math.Round(Math.Sqrt(average));
        }

        //Calculates the difference between 2 integer values, and does it to the power 2
        public int DifferenceBetweenIntervalsToPower2(int interval1, int interval2)
        {
            return (int)Math.Pow(interval1 - interval2, 2);
        }

        //calculates the avarage of a list of integers
        public double FindAverage(int[] intervalDifferences)
        {
            int total = 0;
            foreach(int interval in intervalDifferences)
            {
                total += interval;
            }
            return total/intervalDifferences.Length;
        }
    }
}
