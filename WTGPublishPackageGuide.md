# Publishing WTG.Z.Blazor.Diagrams for WTG usage

This doc explains how the package we use at WTG is updated.

## Creating Releases with Github Actions
When a PR is **merged to master**, the GitHub Action 'Create release' should run. It can also be run manually if needed. This action builds the package, increments the version number, creates a GitHub Release, and uploads the package to the release. The package created should contain Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll and SvgPathProperties.dll.

This action also pushes this release to NuGet, see [WTG.Z.Blazor.Diagrams on nuget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions). We access this internally at WiseTech through ProGet, see [WTG.Z.Blazor.Diagrams on proget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions). See [this article](https://inedo.com/proget/private-nuget-server) for more info about how and why proget is used as a source for NuGet rather than accessing NuGet directly.

See [Winzor.Content - nuget-package.md](https://github.com/WiseTechGlobal/Winzor.Content/blob/main/NativeBlazor(ish)/NCN/blazor-diagrams/nuget-package.md) for more information regarding the building, testing, and package versioning of WTG.Z.Blazor.Diagrams.

## Updating WTG Dev Repo
1. Go to [WTG.Z.Blazor.Diagrams on nuget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions) and find the latest version number.
2. Navigate to [Directory.Packages.Props](https://devops.wisetechglobal.com/wtg/CargoWise/_git/Dev?path=%2FDirectory.Packages.props&version=GBmaster&line=113&lineEnd=113&lineStartColumn=1&lineEndColumn=72&lineStyle=plain&_a=contents) and update the version number in the line `<PackageVersion Include="WTG.Z.Blazor.Diagrams" Version="x.x.x" />` to the latest one.
