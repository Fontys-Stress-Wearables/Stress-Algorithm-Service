namespace StressAlgorithmService.Models
{
    public class HeartRateMeasurement
    {
        public DateTime Date { get; set; }
        public int HeartRate { get; set; }
        public int HeartRateInterval { get; set; }
        public float HRV { get; set; }
        public HeartRateMeasurement(DateTime date, int heartRate, int heartRateInterval)
        {
            Date = date;
            HeartRate = heartRate;
            HeartRateInterval = heartRateInterval;
        }
        public HeartRateMeasurement(DateTime date, int heartRate, int heartRateInterval, float hRV)
        {
            Date = date;
            HeartRate = heartRate;
            HeartRateInterval = heartRateInterval;
            HRV = hRV;
        }
    }
}
