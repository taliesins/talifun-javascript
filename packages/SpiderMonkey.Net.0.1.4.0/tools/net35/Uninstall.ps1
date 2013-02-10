param($installPath, $toolsPath, $package, $project)

Import-Module (Join-Path $toolsPath VS.psd1)
if ($project.Type -eq 'Web Site') {
    $projectRoot = Get-ProjectRoot $project
    if (!$projectRoot) {
        return;
    }

    $binDirectory = Join-Path $projectRoot "bin"
    $libDirectory = Join-Path $installPath "lib\net35"
    $nativeBinDirectory = Join-Path $installPath "NativeBinaries"

    Remove-FilesFromDirectory $libDirectory $binDirectory
    Remove-FilesFromDirectory $nativeBinDirectory $binDirectory
}
elseif($project.ExtenderNames -contains "WebApplication") {
	$depAsm = Get-ChildProjectItem $Project "_bin_deployableAssemblies";
	if($depAsm) {
		$amd64 = Get-ChildProjectItem $depAsm "amd64";
		if($amd64) {
			Remove-Child $amd64 "msvcp100.dll";
			Remove-Child $amd64 "msvcr100.dll";
			Remove-EmptyFolder $amd64;
		}
		$x86 = Get-ChildProjectItem $depAsm "x86";
		if($x86) {
			Remove-Child $x86 "msvcp100.dll";
			Remove-Child $x86 "msvcr100.dll";
			Remove-EmptyFolder $x86;
		}
	}
	Remove-EmptyFolder $depAsm
}
else {
    Remove-PostBuildEvent $project $installPath
}
Remove-Module VS

$allowedReferences = @("smnetjs")

# Full assembly name is required
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$projectCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection
 
$allProjects = $projectCollection.GetLoadedProjects($project.Object.Project.FullName).GetEnumerator();

if($allProjects.MoveNext())
{
	foreach($Reference in $allProjects.Current.GetItems('Reference') | ? {$allowedReferences -contains $_.UnevaluatedInclude })
	{
		$allProjects.Current.RemoveItem($Reference)
	}
}