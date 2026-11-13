# Save Files, Scene List, Exporting Builds

## Persistent Data Path / Save File Location
- Open up windows explorer.
- Paste ```%userprofile%\AppData\LocalLow``` into the top bar and hit enter.
- You're then going to look for the folder for your company name. By default it's labeled as **DefaultCompany**.
- There should now be a folder with your projects name on it. Double click it.
- This should take you to the location of your save files.

### Manually Navigate to the Persistent Data Path / Save File Location
- In the windows search bar, type: %appdata%
- Press the Enter key.
- In the windows explorer to bar, click: **AppData** this should take you to the AppData Folder.
- Double click **LocalLow**.
- You're then going to look for the folder for your company name. By default it's labeled as **DefaultCompany**.
- There should now be a folder with your projects name on it. Double click it.
- This should take you to the location of your save files.

## Changing your Company Name, Game Name, and Build Version
- Top left menu item: Edit/Project Settings
- The first 3 fields you'll see are for the Company Name, Game Name, and Build Version.
- It is wisest to permanently set your Company Name and Game Name now so that you won't have to later.
- The Game Name is also be the name of your games exported .exe file.
- **Build Versions should be updated after every build, if your project has any implementation issues, it's best to know and keep track of what build version you're on.**
- In the main menu, if you haven't noticed already, contains a canvas displaying the current build version of your game for you. You will never need to edit this canvas, as there is a script attached to always match what you set the build version to in the project settings.

## Creating your Scene List
**In order to load into a scene from another, they must be added to a scene list in the build profile, otherwise the game won't load the scene and throw errors.**

- File/Build Profiles
- Press "Open Scene List" 
- To add a scene, simply open up your scene from the project window, and then press the "Add Open Scenes".
- Make sure that GYM_Splash is always index 0
- Make sure that GYM_MainMenu is always index 1
- You can change the indexes by clicking and dragging them in the scene list view.

## Exporting a Build
**Note: It is recommended that builds only be exported until after your main menu has been set up.**

- Top left menu item: File/Build Profiles
- Make sure that the splash screen and main menu are the correct indexes according to ("Creating your Scene List")[https://dreditor.net/docs/tutorials/creatingscenelistbuilds.html#createing-your-scene-list]
- Click the Build button at the bottom.
- Create a folder somewhere on your computer to house the build in. 
- Select that folder in the window and click the Select Folder button.
- The engine will take a LONG time to export your build. Once the build is complete, it should show your newly exported build with it's .exe, and you can now double click on it to play your game whenever it's ready.