# Patient-Service

## Introduction
The Stress Algorithm service is a service made by Group-3 for the SWSP project.
The task of this service is to translate the incoming sensor data into usable stress data, by applying algorithms to it, and then transfering this to another service.

## Build steps

### development
To build and run the project locally just have to run the project through your visual studio.
This will build the API and run all the services necessary for it to function properly.
If there is no NATS service running you can start it by first running  `docker compose -f docker-compose-nats.yaml up -d`.

## Production
Production has not yet been setup, this will be done later.

## Communication
This component has no endpoints, all communication goes through the NATS service. The process for this component is as follows:
- Heartrate measurement data is passed through to this component via NATS
- This data is then passed through the HRVAlgorithm class for processing into stress data
- The resulting HRV measurements are then passed to the Stress Data Service, which saves it to the database
