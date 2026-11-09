# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.11] - 2026-11-9
### [Added]
- New option on ChoiceManager.cs ```postDisplayBufferTime``` indicating the: Time in seconds waited after displaying the choice UI before selecting the first choice option. In order to give more control to specifically when the First Choice UI Option is selected. To additionally help so that players don't accidentally click on a choice just trying to skip through text.
### [Fixed]
- Fixed an issue where importing all packages before FMOD is installed (Normally this scenario technically never even happens) would lock up the editor with an error. Issue was caused by Visual Studios Intellisense mistakenly inserting un-needed using statements on MusicManager.cs during a fix made in 0.1.10.
- Updated the Choice_UI_Template to skip selection so that players don't accidentally click on a choice just trying to skip through text.

## [0.1.10] - 2026-11-7
### [Added]
- Added StudioEventEmitterPatcher.cs to fix a bug in the FMOD StudioEventEmitter script where it only plays the first track ever played. If you installed DREditor before Version 0.1.10, then you can call this patch from: DREditor/Dependencies/FMOD/Patch StudioEventEmitter to apply this patch to your project. 
- Added the StudioEventEmitterPatcher process to the OnCompleted() function of the FMODInitializer.cs so that new DREditor users will not have to do it manually. 
### [Fixed]
- Fixed an issue in the FMOD Installation process in FMODInitializer.cs where if your file path was too long it would overflow the message window. Now, instead of displaying the file path, the system copies the file path to your clipboard for you. An additional dialog box explaining this has also been added in order to clarify the discrepancy.
- Fixed a NDepend ND1104 Complexity rule violation in RoomStateSaveDisplayEditor.cs, the function ```void CreateRoomStateAssets(Objective pValue, string path, RoomStateData data)``` was added to reduce complexity.
- Fixed a bug where: Item_Name_Display_ETemplate's  Item Name Display UI process doesn't go away during TPFD(2.5D mode)'s Leave Ask. The Item_Name_Display_ETemplate has been updated to fix this, you can receive this update by Reimporting Samples from: DREditor/Import/Samples via Search.
- Fixed a bug where: Selecting a Character that gives instant dialogue, then selecting something like a door that DOESNT have instant dialogue. Forces the camera to look at a character. (ActorManager.cs + InstantDialogueEnder.cs)
### [Removed]
- Removed a massive comment detailing the FMOD StudioEventEmitter Bug from the MusicManager.cs's ```PlayValidMusic``` Function. Since the system now has a solution for it. It has been replaced with updated XML remarks.
- Removing Samples paths from Sampless Package Jsons for: CharTweener, Editor.Utility.Converting, Investigations, Itemrolling, PhraseAnimations and RoomStateBuilding. This is an attempt to reduce errors from the Menu Item Command: DREditor/Import/Samples via Search.

## [0.1.9] - 2026-11-5
### [Added]
- XML Summaries to ImageScanUtility.cs
### [Fixed]
- ImageScanUtility.cs has been reworked since it was having trouble scanning large folders.
- DuplicateScenesInFolder.cs Should now work properly and work by selecting folders on the left hand side of the project window.
- PackageInstallerWizard.cs has been reworked and should now install all packages properly, If it still does not work, then mass select all packages and then select Install.
- FolderUtility.cs's ```bool SelectedObjectIsFolder()``` is now: ```bool TryGetSelectedFolderPath(out string folderPath)```
- CGVideoProjector.cs now has inspector fields for the default resolutions for projecting videos so that aspect ratio fitter components will work properly with it.

## [BETA RELEASE] [0.1.8] - 2026-10-31