function Update-Version($path, $version) {
	$content = (Get-Content $path) 
	$content = $content -replace '\$version\$', $version
	$content | Out-File $path
}

function Get-Version() {
	$version = [System.Reflection.Assembly]::LoadFile("$root\src\Hyper.ComponentModel\bin\Release\Hyper.ComponentModel.dll").GetName().Version
	$currentVersion = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)
	return $currentVersion;
}

function Create-Package($path) {
	& $root\src\.nuget\NuGet.exe pack $path
}

$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$hyperDescriptorNuspecPath = "$root\assets\Hyper.ComponentModel.nuspec"
$hyperDescriptorSourceNuspecPath = "$root\assets\Hyper.ComponentModel.Source.nuspec"

$version = Get-Version

Write-Host "Setting .nuspec version tag to $version"

Update-Version $hyperDescriptorSourceNuspecPath $version
Update-Version $hyperDescriptorNuspecPath $version

Create-Package $hyperDescriptorNuspecPath
Create-Package $hyperDescriptorSourceNuspecPath