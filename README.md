# SolarApp
Project to process solar panel data. A Fronius inverter can be set to ftp data as json to a remote site every 15 minutes. 
The DataProcessor part of the project downloads these files and imports them into a MongoDb instance. 
The front end then processed these files and produces graphs.

Technologies used:
Mvc, C#, MongoDb, D3, TeamCity 9.1.5 for CI and Octopus 3 for CD, Tests in SpecFlow, Powershell 4
