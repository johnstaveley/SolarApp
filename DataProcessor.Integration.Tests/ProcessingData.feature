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

@Ignore
Scenario: Process data from an incoming file
	Given An output file 'xyz.log' exists
	| Timestamp | PAC | Day Energy | Year Energy | Total Energy |
	| [Now] | 321 | 100 | 1000 | 10000 |
	When I process the file
	Then I can store the values from the file
	| Timestamp | PAC | Day Energy | Year Energy | Total Energy |
	| [Recent] | 321 | 100 | 1000 | 10000 |

