
function CleanUp{
	Write-Host ****** Removing config transform files from release ******
	Remove-Item *.config -exclude Web.config, privateSettings.config | Write-Host
}

try{
	CleanUp
} catch {
	"Failed to run script"
	Write-Host $_.Exception.Message  
	[Environment]::ExitCode = 1 
	$LastExitCode = 1 
	Exit 1 
}
