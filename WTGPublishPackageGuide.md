# Publishing WTG.Z.Blazor.Diagrams for WTG usage
This doc explains how the package we use at WTG is updated.

When a PR is merged to master, the GitHub Action 'Create release' should run. It can also be run manually if needed. The action picks the next version number, builds the package, pushes it to the `cargowise-nuget-local` repository in JFrog Artifactory, and then creates a GitHub Release with the package attached. The package is created on build in the action and should contain Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll and SvgPathProperties.dll

The action authenticates to Artifactory with GitHub OIDC, so there is no API key to manage. Consumers such as CargoWise restore from `cargowise-nuget-virtual`, which includes `cargowise-nuget-local`.

## Updating the package version in WTG
Once the action has finished, update the version of WTG.Z.Blazor.Diagrams in Directory.Packages.props of the consuming repository (e.g. CargoWise) to match the release.


