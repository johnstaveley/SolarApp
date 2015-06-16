﻿Feature: Persistence
	In order to keep data
	As an automated system
	I want to be able to store data in MongoDb

Scenario: Store a random value to a setting entry in the database and retrieve it
	Given I want to use a database 'Test'
	And I want to store some random value
	When I open a connection to the database
	And I persist the setting to the database
	Then the random value should be retrievable from the database

Scenario: Store a data point in the database and retrieve it
	Given I want to use a database 'Test'
	And I have a data point with values:
	| Time		| CurrentReading | DayEnergy | YearEnergy  | TotalEnergy  |
	| [Now]     | 321			  | 100       | 1000        | 10000        |
	When I open a connection to the database
	And I persist the data point to the database
	Then I can retrieve a data point with values:
	| Time		| CurrentReading  | DayEnergy  | YearEnergy | TotalEnergy  |
	| [Now]		| 321			  | 100        | 1000       | 10000        |
