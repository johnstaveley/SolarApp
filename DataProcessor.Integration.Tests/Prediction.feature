Feature: Prediction
	In order to ensure my appliance is run at a time where it is likely it will be powered mostly by solar energy
	As an automated system
	I want to be told when I can run my appliance

@Ignore
Scenario: An energy prediction algorthm tells me when I can start my appliance
	Given threshold energy required is 80%
	And I am using appliance 'X'
	And I am using prediction algorithm 'Y'
	And I have historic data...
	When I run the prediction routine
	Then the first time the appliance should run is 11 
