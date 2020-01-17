param([string] $v)

if (!$v)
{
    $version = '3.1.0-prerelease1.' + $([System.DateTime]::Now.ToString('MM-dd-HHmmss'))
}
else{
	$version = $v
}
Write-Host 'Version: ' $version 
get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\RESTFul.Api.sln
dotnet test ..\src\RESTFul.Api.sln
dotnet pack ..\src\RESTFul.Api.sln -o .\ -p:PackageVersion=$version