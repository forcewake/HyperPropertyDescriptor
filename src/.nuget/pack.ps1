$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\Hyper.ComponentModel\bin\Release\Hyper.ComponentModel.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\.nuget\Hyper.ComponentModel.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\.nuget\Hyper.ComponentModel.compiled.nuspec

& $root\.nuget\NuGet.exe pack $root\.nuget\Hyper.ComponentModel.compiled.nuspec