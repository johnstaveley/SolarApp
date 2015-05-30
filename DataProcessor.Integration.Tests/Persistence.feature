Feature: Persistence
	In order to keep data
	As an automated system
	I want to be able to store data in MongoDb

Scenario: Store a random value in the database and retrieve it
	And I want to use a database 'Test'
	And I want to store some random value
	When I open a connection to the database
	And I persist the value to the database
	Then the random value should be retrievable from the database

@Ignore
Scenario: Store a data point in the database and retrieve it
	Given I have a data point with values:
	| Timestamp | PAC | Day Energy | Year Energy | Total Energy |
	| [Now]     | 321 | 100        | 1000        | 10000        |
	When I open a connection to the database
	And I persist the value to the database
	Then I can retrieve a data point with values:
	| Timestamp | PAC | Day Energy | Year Energy | Total Energy |
	| [Now]     | 321 | 100        | 1000        | 10000        |
