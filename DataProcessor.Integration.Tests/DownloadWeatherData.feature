Feature: DownloadWeatherData
	In order to better predict energy production
	As an automated system
	I want to download a new weather forecast for the day

@Ignore
Scenario: Download data from the Met office and store in the database
	Given I have credentials to the met office data point system
	And a weather forecast has been requested
	And my target area is LS10 1EA
	When I download a weather forecast
	Then It is stored in the database
