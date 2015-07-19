Feature: DownloadWeatherData
	In order to better predict energy production
	As an automated system
	I want to download a new weather forecast for the day

Scenario: Download forecast data from the Met office and store in the database
	Given I have credentials to the met office data point system
	And a weather forecast has been marked as requested
	And I have a target met office forecast area
	When I download a weather forecast
	Then The weather forecast is stored in the database

Scenario: Download observation data from the Met office and store in the database
	Given I have credentials to the met office data point system
	And a weather observation has been marked as requested
	And I have a target met office observation area
	When I download a weather observation
	Then The weather observation is stored in the database
