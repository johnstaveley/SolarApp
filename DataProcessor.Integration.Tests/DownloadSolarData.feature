Feature: Downloading Data from the drop point
	In order process the data from the solar panel
	As an automated system
	I want to be able to download new solar data files	

Scenario: Download files locally
Given I have the credentials of the ftp site
And there are files waiting there
When I access the site
Then I download the files to a local directory
And Remove the files from the ftp site


