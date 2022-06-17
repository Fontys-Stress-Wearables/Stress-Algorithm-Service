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
            DateTime timestamp = GetDateTime(message.message.timestamp);

            int[] data = message.message.records;

            Console.WriteLine(data.Length);

            if (data.Length < measurementInterval)
            {
                int hrv = algorithm.CalculateHRVBasedOnIntervals(data);
                HeartRateVariabilityMeasurement stressData = new HeartRateVariabilityMeasurement(message.message.patientId, message.message.wearableId, timestamp, hrv);
                heartRateVariabilityMeasurements.Add(stressData);
            }
            else
            {
                for (int i = 0; i < data.Length / measurementInterval; i++)
                {
                    if (i == 71)
                    {
                        Console.WriteLine("71 hit");
                    }
                    if (i == 72)
                    {
                        Console.WriteLine("72 hit");
                    }

                    timestamp = CalculateTimeStamp(timestamp, data, i);
                    int offset = GetOffset(data.Length, i);

                    int hrv = algorithm.CalculateHRVBasedOnIntervals(SubArray(data, i * measurementInterval, offset));
                    heartRateVariabilityMeasurements.Add(new HeartRateVariabilityMeasurement(message.message.patientId, message.message.wearableId, timestamp, hrv));
                }
            }

            Console.WriteLine("Calculated " + heartRateVariabilityMeasurements.Count + " hrv values out of " + data.Length + " intervals");

            nats.Publish("stress:created", heartRateVariabilityMeasurements);
        }

        private DateTime CalculateTimeStamp(DateTime dateTime, int[] intervals, int index)
        {
            int offset = GetOffset(intervals.Length, index);

            int[] currentIntervals = SubArray(intervals, index * measurementInterval, offset);
            int totalMillis = 0;

            for(int i = 0; i < currentIntervals.Length; i++)
            {
                totalMillis += currentIntervals[i];
            }

            return dateTime.AddMilliseconds(totalMillis);
        }

        private int GetOffset(int arrayLength, int index)
        {
            if ((index + 1) * measurementInterval > arrayLength)
            {
                int value = arrayLength - index * measurementInterval;
                
                if (value < 60)
                {
                    Console.WriteLine(arrayLength + " - " + value + " * " + measurementInterval);
                }
                
                return value;
            }
            else
            {
                return measurementInterval;
            }
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
        public int[] SubArray(int[] array, int startIndex, int length)
        {
            int[] result = new int[length];
            Array.Copy(array, startIndex, result, 0, length);
            return result;
        }
    }
}
