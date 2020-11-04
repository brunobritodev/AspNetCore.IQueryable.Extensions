param([string] $v)

if (!$v)
{
    $version = '3.1.9-prerelease1.' + $([System.DateTime]::Now.ToString('MM-dd-HHmmss'))
}
else{
	$version = $v
}
Write-Host 'Version: ' $version 
get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\IQueryable.Extensions.sln
dotnet test ..\src\IQueryable.Extensions.sln
dotnet pack ..\src\IQueryable.Extensions.sln -o .\ -p:PackageVersion=$version