namespace StressAlgorithmService.Models
{
    public class HRVInput
    {
        public string patientId { get; set; }
        public string wearableId { get; set; }
        public string timestamp { get; set; }
        public int[] records { get; set; }

        public HRVInput(string patientId, string wearableId, string timestamp, int[] records)
        {
            this.patientId = patientId;
            this.wearableId = wearableId;
            this.timestamp = timestamp;
            this.records = records;
        }
    }
}
