param(
    [string]$PackageId = 'CargoWiseCloudDeployment',
    [string]$PackageVersion = '1.0.0.312',
    [string]$Destination = '.\bin'
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -Path $Destination)) {
    New-Item -Path $Destination -ItemType Directory | Out-Null
}

$packageRoot = Join-Path $Destination ("$PackageId.$PackageVersion")
$packageZip = "$packageRoot.zip"
$packageUrl = "https://proget.wtg.zone/nuget/WTG-Internal/package/${PackageId}/${PackageVersion}"

Write-Host "Downloading $PackageId $PackageVersion"
Invoke-WebRequest -Uri $packageUrl -OutFile $packageZip

if (Test-Path -Path $packageRoot) {
    Remove-Item -Path $packageRoot -Recurse -Force
}

Expand-Archive -Path $packageZip -DestinationPath $packageRoot -Force

$contentPath = Join-Path $packageRoot 'content'
$libPath = Join-Path $packageRoot 'lib'

if (Test-Path -Path $contentPath) {
    Copy-Item -Path (Join-Path $contentPath '*') -Destination $Destination -Recurse -Force
}

if (Test-Path -Path $libPath) {
    Get-ChildItem -Path $libPath -Directory | ForEach-Object {
        Copy-Item -Path (Join-Path $_.FullName '*') -Destination $Destination -Recurse -Force
    }
}

Write-Host "Deployment bootstrap files copied to $Destination"
