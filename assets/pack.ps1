$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\src\Hyper.ComponentModel\bin\Release\Hyper.ComponentModel.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\assets\Hyper.ComponentModel.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\assets\Hyper.ComponentModel.compiled.nuspec

& $root\src\.nuget\NuGet.exe pack $root\assets\Hyper.ComponentModel.compiled.nuspec