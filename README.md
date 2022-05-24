# Stress-Algorithm-Service
## Port: 5032
The service that is responsible for taking in normalized stress data and processing it into processed stress measurements.
Currently, only the Heart Rate Variablity algorithm is implemented.

## Nats
The service subscribes to the following topics:
```
measurement:created
```
And publishes to:
```
stress:created
```
## Docker
If you want to manually build a Docker container of this service and running, use the following commands in a CLI:
```
docker build -t stress_algorithm_service --name StressAlgorithmService .
```
Then
```
docker run -p 5032:80 --network=swsp stress_algorithm_service
```
