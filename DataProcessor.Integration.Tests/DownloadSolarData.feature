Feature: Downloading Data from the drop point
	In order process the data from the solar panel
	As an automated system
	I want to be able to download new solar data files	

Scenario: Get a directory listing of at the data drop site
Given I have the credentials of the ftp site
When I access the site
Then I do a directory listing
And There are files of the right format

Scenario: Download files to a local receiving directory
Given I have the credentials of the ftp site
And I want to navigate to a subdirectory of the ftp site 'test'
And the local temp directory 'receiving' is empty
And there is a file 'Log20150520062000.log' waiting in the 'test' subdirectory
When I access the site
Then I download the file 'Log20150520062000.log' to a local directory
And The file 'Log20150520062000.log' is stored in the 'receiving' directory

