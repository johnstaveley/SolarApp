Feature: Processing Data
	In order to process data into the system
	As an automated system
	I want to open files, understand their contents and save to the database

@Ignore
Scenario: Process the exception contents of a file
	Given I have access to a new file
	And is a new file there containing some unusual content
	When I process the file
	Then I can store the unusual content 
	And raise a notification

Scenario: Can Process energy data from an incoming file and store it in the database
	Given I have a data point with values:
	| Time  | CurrentReading | DayEnergy | YearEnergy | TotalEnergy | FileName |
	| [Now] | 321            | 100       | 1000       | 10000       | [Random] |
	And I save the data point to a file
	And I want to use a database 'Test'
	And I open a connection to the database
	When I process the file
	Then I can retrieve a data point with values:
	| Time		| CurrentReading | DayEnergy  | YearEnergy | TotalEnergy  |
	| [Now]		| 321			 | 100        | 1000       | 10000        |

