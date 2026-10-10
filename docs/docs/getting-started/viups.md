# Verify, Import, and Update Packages and Samples
These are the steps to follow _after_ completing the process the dependency manager walks you through. (Both check marks must be green!)

## Verify Dependencies 
- After going through the dependency process, go to: Edit/Project Settings. 
- On the left, make sure "Player" is selected, then under the "Other Settings" dropdown, Look for **Script Compilation**.
- Ensure that **DOTWEEN** and **FMOD_UNITY** are present.
- **If either is missing:** Go to DREditor/Dependencies/Open Dependency Manager and press refresh one more time. 

## FMOD Git Ignore
- Go to: FMOD/Setup Wizard
- Click Source Control
- Copy everything in the text field.
- Go to Unity hub and find your project.
- Click the 3 dots to the right. 
- Click "Show in explorer" 
- You're going to look for a file called .gitignore and open it.
- Make a new line at the very bottom of the file, and paste what you copied earlier.
- Ctrl + s to save (or just File/Save)

## Import TMP
- Go to: Window/TextMeshPro/Import TMP Essential Resources
- Go to: Window/TextMeshPro/Import TMP Examples and Extras

## Import Packages and Samples
- At this point, commit all the changes logged on your github desktop. 
- DREditor/Import/Packages en masse . This will download all the available packages for DREditor to your project. Otherwise, you can manually add the pieces you want. Then you can import the templates from samples.
- DREditor/Import/Samples via Search and click import. This should import all the samples for all installed DREditor Packages.

## Duplicate Template Scenes
- Go to the the Samples folder created earlier, left click on it in your project window.
- Right Click: DREditor/Duplicate Scenes in Folder
- A window will appear, under prefix, it's suggested that you put: GYM
- Click Duplicate. A folder select window will open.
- Select your "Scenes" folder, or wherever you wish to keep your sandbox scenes.
- Click Select Folder. This should duplicate all the template sample scenes so you now have a place to start working on your project.
- Go to your Scenes folder and double click GYM_Dialogue

## Updating DREditor
- At the top go to DREditor/Dependencies/Update Package Wizard 
- Click Update