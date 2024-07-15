# Publishing WTG.Z.Blazor.Diagrams for WTG usage

This doc explains how the package we use at WTG is updated.

## Creating Releases with Github Actions

### Releases
When a PR is **merged to master**, the GitHub Action 'Create release' should run. This action creates a GitHub Release and uploads the package to the release. The package created should contain Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll and SvgPathProperties.dll.

This action also pushes this release to NuGet, see [WTG.Z.Blazor.Diagrams on nuget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions). We access this internally at WiseTech through ProGet, see [WTG.Z.Blazor.Diagrams on proget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions). See [this article](https://inedo.com/proget/private-nuget-server) for more info about how and why proget is used as a source for NuGet rather than accessing NuGet directly.

### Version Incrementing
The version number is also handled by the release action. It is determined using the [@reecetech/version-increment](https://github.com/reecetech/version-increment) action. The semantic versioning scheme rules below are verbatim from [reecetech's README.md](https://github.com/reecetech/version-increment?tab=readme-ov-file#conventional-commits-semver-with-smarts-).

> the action will parse the last commit message (usually the merge commit) to determine the increment type for a semver version.
>
> The following increment types by keyword are supported:
> - patch: build, chore, ci, docs, fix, perf, refactor, revert, style, test
> - minor: feat
> - major: any of the above keywords followed by a '!' character, or 'BREAKING CHANGE:' in commit body
>
> If none of the keywords are detected, then the increment specified by the increment input will be used (defaults to patch).

For more info on semantic versioning, refer to the [semantic versioning spec](https://semver.org/spec/v2.0.0.html).

> [!tip]
> If you have a WTG workitem that requires a push to this repo, you will additionally need to update the WTG.Z.Blazor.Diagrams version in the WTG Dev repo with the steps below.

## Updating WTG Dev Repo
1. Go to [WTG.Z.Blazor.Diagrams on nuget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions) and find the latest version number.
2. Navigate to [Directory.Packages.Props](https://devops.wisetechglobal.com/wtg/CargoWise/_git/Dev?path=%2FDirectory.Packages.props&version=GBmaster&line=113&lineEnd=113&lineStartColumn=1&lineEndColumn=72&lineStyle=plain&_a=contents) and update the version number in the line `<PackageVersion Include="WTG.Z.Blazor.Diagrams" Version="x.x.x" />` to the latest one.

Refer to the Winzor.Content document [nuget-package](https://github.com/WiseTechGlobal/Winzor.Content/blob/main/NativeBlazor(ish)/NCN/blazor-diagrams/nuget-package.md) for more information.
