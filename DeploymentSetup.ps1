$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$nugetConfigPath = Join-Path $repoRoot 'nuget.config'
$sourceName = 'WTG-Internal'
$sourceUrl = 'https://proget.wtg.zone/nuget/WTG-Internal/v3/index.json'

dotnet nuget update source $sourceName --source $sourceUrl --configfile $nugetConfigPath | Out-Null
if ($LASTEXITCODE -ne 0) {
    dotnet nuget add source $sourceUrl --name $sourceName --configfile $nugetConfigPath | Out-Null
}
