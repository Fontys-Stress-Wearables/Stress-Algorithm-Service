using Microsoft.VisualStudio.TestTools.UnitTesting;
using StressAlgorithmService.Logic;

namespace StressAlgorithmService.Tests
{
    [TestClass]
    public class HRVAlgorithmTests
    {
        HRVAlgorithm algorithm = new HRVAlgorithm();
        [DataTestMethod]
        [DataRow(744, 427, 100489)]
        [DataRow(427, 498, 5041)]
        [DataRow(498, 931, 187489)]
        public void IntervalTest(int firstInterval, int secondInterval, int expected)
        {
            int actual = algorithm.DifferenceBetweenIntervalsToPower2(firstInterval, secondInterval);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AverageTest()
        {
            double expected = 97673;
            int[] intervals = new int[] { 100489, 5041, 187489 };

            double actual = algorithm.FindAverage(intervals);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testCalculateHRVByInterval()
        {
            int expected = 185;
            int[] intervals = new int[] { 1000, 950, 1000, 860, 1230, 1020, 1000 };
            int hrv = algorithm.CalculateHRVBasedOnIntervals(intervals);
            Assert.AreNotEqual(hrv, 0);
            Assert.AreEqual(expected, hrv);
        }
        [TestMethod]
        public void testCalculateHRVByInterval0()
        {
            int expected = 0;
            int[] intervals = new int[] { 1000, 1000, 1000, 1000, 1000 };
            int hrv = algorithm.CalculateHRVBasedOnIntervals(intervals);
            Assert.AreEqual(hrv, expected);
        }
        [TestMethod]
        public void testCalculateHRVByInterval200()
        {
            int expected = 200;
            int[] intervals = new int[] { 1000, 800, 1000, 800, 1000 };
            int hrv = algorithm.CalculateHRVBasedOnIntervals(intervals);
            Assert.AreEqual(hrv, expected);
        }
    }
}
