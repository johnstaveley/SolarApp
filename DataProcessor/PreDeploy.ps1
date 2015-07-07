try {
	Import-Module ".\Deployment\PScripts\ServiceManagement.psm1"
	Copy-Item SolarApp.DataProcessor.exe.config App.Config
	Write-Host "Calling Stop Service for " $ServiceName
	Stop-WindowsService -ServiceName $ServiceName
}
catch {
	"Failed to run script"
	Write-Host $_.Exception.Message
	[Environment]::ExitCode = 1 
	$LastExitCode = 1 
	Exit 1 
}
