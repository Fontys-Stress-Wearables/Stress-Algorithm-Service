namespace StressAlgorithmService.Models
{
    public class HeartRateVariabilityMeasurement
    {
        public string PatientId { get; set; }
        public string WearableId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int HeartRateVariability { get; set; }

        public HeartRateVariabilityMeasurement(string patientId, string wearableId, DateTime timeStamp, int heartRateVariability)
        {
            PatientId = patientId;
            WearableId = wearableId;
            TimeStamp = timeStamp;
            HeartRateVariability = heartRateVariability;
        }
    }
}
