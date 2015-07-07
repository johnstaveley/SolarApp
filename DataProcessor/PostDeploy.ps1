function InstallService {
	Import-Module ".\Deployment\PScripts\ServiceManagement.psm1"
	$exe = Get-Item -Path .\*.SolarApp.DataProcessor.exe
	$fullPath = $exe.FullName
	Install-WindowsService -ServiceName $ServiceName -ServiceUser $ServiceUser -ServicePassword $ServicePassword -FullPath $fullPath -PathToDeploymentScripts .\Deployment\Executables
}

function RemoveOldReleases {
	Import-Module ".\Deployment\PScripts\ReleaseManagement.psm1"
	Remove-Releases -ReleaseRoot $ServiceLocation -IsSharedService $false
}

function CleanUp{
	Write-Host ****** Removing config transform files from release ******
	#Remove-Item *.config -exclude *.exe.config, log4net.config | Write-Host
	Remove-Item *.config -exclude *.exe.config | Write-Host
}

try{
	InstallService
	CleanUp
	RemoveOldReleases
} catch {
	"Failed to run script"
	Write-Host $_.Exception.Message  
	[Environment]::ExitCode = 1 
	$LastExitCode = 1 
	Exit 1 
}
