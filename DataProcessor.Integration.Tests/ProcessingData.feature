Feature: Processing Data
	In order to process data into the system
	As an automated system
	I want to open files, understand their contents and save to the database

Scenario: Can Process energy data from an incoming file and store it in the database
	Given I have a data point with values:
	| Time  | CurrentReading | DayEnergy | YearEnergy | TotalEnergy | FileName |
	| [Now] | 321            | 100       | 1000       | 10000       | [Random] |
	And I save the data point to a file
	When I process the file
	Then I can retrieve a data point with values:
	| Time		| CurrentReading | DayEnergy  | YearEnergy | TotalEnergy  |
	| [Now]		| 321			 | 100        | 1000       | 10000        |

Scenario: Failed Data - Reject file with invalid status code
	Given I have a data point with values:
	| Time  | FileName | StatusCode |
	| [Now] | [Random] | 1          |
	And I save the data point to a file 
	When I process the file
	Then I cannot retrieve a data point
	Then I can retrieve failed data with text: '"Code":1'

Scenario: Failed Data - Reject file with invalid status reason
	Given I have a data point with values:
	| Time  | FileName | StatusUserMessage |
	| [Now] | [Random] | Rhubarb           |
	And I save the data point to a file
	When I process the file
	Then I cannot retrieve a data point
	Then I can retrieve failed data with text: '"UserMessage":"Rhubarb"'

Scenario: Failed Data - Reject file and move to failed data
	Given I have a file containing garbage: 'fuubar'
	When I process the file
	Then I cannot retrieve a data point
	Then I can retrieve failed data with text: 'fuubar'