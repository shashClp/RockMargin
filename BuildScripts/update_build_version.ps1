param
(
    [Parameter(Position=0, Mandatory=1, ValueFromPipeline=$true)]
    [string]$version
)

if (Get-Command Update-AppveyorBuild -errorAction SilentlyContinue)
{
    Write-Host "Updating AppVeyor build version... " -ForegroundColor Cyan -NoNewline
    Update-AppveyorBuild -Version $version | Out-Null
    $version | Write-Host -ForegroundColor Green
}