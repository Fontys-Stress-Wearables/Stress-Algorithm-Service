using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StressAlgorithmService.Interfaces;
using StressAlgorithmService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StressAlgorithmService.Controllers
{
    public class UnprocessedStressDataService
    {
        private readonly INatsService nats;
        private readonly IHRVAlgorithm algorithm;
        private readonly int measurementInterval = 60;

        public UnprocessedStressDataService(INatsService nats, IHRVAlgorithm algorithm)
        {
            this.nats = nats;
            this.algorithm = algorithm;

            this.nats.Subscribe<NatsMessage<HRVInput>>("normalized-data", OnMeasurementCreated);

            /// TODO: Remove the line below, it's for testing only.
            //this.nats.Publish("measurement:created", new HRVInput(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, new int[] { 500, 250 }));
        }

        private void OnMeasurementCreated(NatsMessage<HRVInput> message)
        {
            List<HeartRateVariabilityMeasurement> heartRateVariabilityMeasurements = new List<HeartRateVariabilityMeasurement>();

            int[] data = message.message.records;
            if (data.Length < measurementInterval)
            {
                int hrv= algorithm.CalculateHRV(data);
                HeartRateVariabilityMeasurement stressData = new HeartRateVariabilityMeasurement(message.message.patientId, message.message.wearableId, message.message.timestamp, hrv);
                heartRateVariabilityMeasurements.Add(stressData);
             }
            else
            {
                List<HeartRateVariabilityMeasurement> stressData = new List<HeartRateVariabilityMeasurement>();
                DateTime timestamp = GetDateTime(message.message.timestamp);

                for (int i = 0; i < (data.Length - measurementInterval); i++)
                {
                    timestamp.AddMilliseconds(data[i]);
                    int hrv = algorithm.CalculateHRV(SubArray(data,i, measurementInterval));
                    heartRateVariabilityMeasurements.Add(new HeartRateVariabilityMeasurement(message.message.patientId, message.message.wearableId, message.message.timestamp, hrv));
                }
            }

            nats.Publish("stress:created", heartRateVariabilityMeasurements);
        }

        private DateTime GetDateTime(string dateTimeString)
        {
            if (dateTimeString == null || dateTimeString.Length == 0)
            {
                return DateTime.Now;
            }
            try
            {
                return DateTime.Parse(dateTimeString);
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }
        public int[] SubArray(int[] array, int offset, int length)
        {
            int[] result = new int[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
