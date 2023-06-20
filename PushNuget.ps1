# To push new package to proget 
# 1. in Blazor.Diagrams.csproj and Blazor.Diagrams.Core.csproj increase version number
# 2. build Blazor.Diagrams - a package will be created and it should include strong named dlls:
#  Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll, SvgPathProperties.dll
# 3. change the package path in this script to have the package number from the csproj
# 4. run the script (remember to put in the correct api key)

pushd $PSScriptRoot

$feedSource = "http://proget.wtg.zone/nuget/WTG-Internal/"
$apiKey = ""
$packagePath = "C:\git\wtg\Blazor.Diagrams\src\Blazor.Diagrams\bin\Debug\WTG.Z.Blazor.Diagrams.1.0.8.nupkg"

function Write-Log {
    Write-Host "$(get-date -f "yyyy-MM-dd HH:mm:ss.fff")`t$args"
}

try {
    $scriptSw = [System.Diagnostics.Stopwatch]::StartNew()
    & nuget.exe push $packagePath -ApiKey $apiKey -Source $feedSource -Verbosity detailed
    if (-not $?)
    {
        Write-Log "FAILED to deploy: $packagePath)"
    }
    else 
    {
        Write-Log "Deployed $packagePath )"
    }
}
finally {
    popd
    Write-Log "Finished (took: $($scriptSw.Elapsed))"
} 