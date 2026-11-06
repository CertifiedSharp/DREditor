# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.9] - 2026-11-5
### [Added]
- XML Summaries to ImageScanUtility.cs
### [Fixed]
- ImageScanUtility.cs has been reworked since it was having trouble scanning large folders.
- DuplicateScenesInFolder.cs Should now work properly and work by selecting folders on the left hand side of the project window.
- PackageInstallerWizard.cs has been reworked and should now install all packages properly, If it still does not work, then mass select all packages and then select Install.
- FolderUtility.cs's ```bool SelectedObjectIsFolder()``` is now: ```bool TryGetSelectedFolderPath(out string folderPath```
- CGVideoProjector.cs now has inspector fields for the default resolutions for projecting videos so that aspect ratio fitter components will work properly with it.

## [BETA RELEASE] [0.1.8] - 2026-10-31