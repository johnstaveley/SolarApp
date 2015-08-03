﻿Feature: Persistence
	In order to keep data
	As an automated system
	I want to be able to store data in MongoDb

Scenario: Store a random value to a setting entry in the database and retrieve it
	Given I want to store some random value
	When I persist the setting to the database
	Then the random value should be retrievable from the database

Scenario: Store a data point in the database and retrieve it
	Given I have a data point with values:
	| Time  | CurrentReading | DayEnergy | YearEnergy | TotalEnergy | FileName |
	| [Now] | 321            | 100       | 1000       | 10000       | [Random] |
	When I persist the data point to the database
	Then I can retrieve a data point with values:
	| Time		| CurrentReading | DayEnergy  | YearEnergy | TotalEnergy  |
	| [Now]		| 321			 | 100        | 1000       | 10000        |

Scenario: Calculates the average reading for a specified hour across two days where data is provided
	Given I have a data points with values:
	| Time                | CurrentReading | Comment  |
	| 2015-06-15 09:23:00 | 100            | Included |
	| 2015-06-15 09:33:00 | 200            | Included |
	| 2015-06-14 09:46:00 | 300            | Included |
	| 2015-06-15 10:02:01 | 2500           | Excluded |
	When I calculate the mean for hour 9
	Then The calculated average value is 200

Scenario: Calculates the average reading as null for a specified hour across two days where no data
	Given I have a data points with values:
	| Time                | CurrentReading | Comment  |
	| 2015-06-15 09:23:00 | 100            | Excluded |
	| 2015-06-15 09:33:00 | 200            | Excluded |
	| 2015-06-14 09:46:00 | 300            | Excluded |
	| 2015-06-15 10:00:01 | 2500           | Excluded |
	When I calculate the mean for hour 8
	Then The calculated average value is null

Scenario: Calculates the latest reading across two days where data is provided
	Given I have a data points with values:
	| Time                | CurrentReading |
	| 2018-06-15 09:23:00 | 100            |
	| 2018-06-15 09:33:00 | 200            |
	| 2018-06-14 09:46:00 | 300            |
	| 2018-06-15 10:00:01 | 2500           |
	When I calculate the latest date
	Then The calculated latest date is '2018-06-15 10:00:01'