# Getting Started
Please be sure to follow each section **_in order._**
## Downloading Unity
- Make a Unity Account: <https://login.unity.com/en/sign-up>|
- Feel free to continue with Email or whatever process it lists.
- Confirm your email or whatever else the process tells you to follow. 
- Once completed Download Unity Hub <https://unity.com/download>
- Download based off your machine/os. **Windows is preferred and what is focused and supported.**
- Go through the installation process for unity hub.
- Start up unity hub once done 
- It will give you the TOS, just scroll all the way down and hit accept. 
- Then sign into unity hub.
- Once sign in on the left click on installs.
- Click Install Editor.
- Close the window and then in the top right, second symbol it may show that a unity version is installing, click the X button to stop the installation.
- Once done, click install editor in the top right.
- Click Archive 
- Click the search search bar and type in 4.9
- Unity Version 6000.4.9f1 should be the first result. Click Install
- Uncheck Microsoft visual studio. Now nothing should be checked.
- At the bottom right click install. The install should now start.

## Setting Up Github
- Create a github account: <https://github.com/signup>
- If the page opens you up and says "Access Restricted" this may happen with users with firefox, download chrome to get around this. (sorry) If you continue to have issues past this please contact the project maintainer for assistance.
- Sign up for github however you like whether it be through google/email whatever. 
- Once signed in Download Github Desktop <https://desktop.github.com/download/>
- When it's done installing, start up the exe and go through the set up process. 
- Sign into github desktop once you've finished installing everything. 
- Once you've signed in there should be a blue button you should click that says "Finish".


## Once Unity Version Finishes Downloading
- Go to projects.
- Top right click new project.|
- Make sure Editor Version is 6000.4.9f1
- There should be a drop down that says "Learning" Click it then select "All"
- Click Universal 3D
- To the Right, you'll create the name of your project **WHEN CREATING THE NAME THE REPO THE SAME THING AS THE PROJECT. USE THE DASHES IF YOURE USING SPACING! EXAMPLE: Project-Edens-Garden**
- Under location, make sure it's where you want to put it on your computer. Wherever that may be.
- Click Create Project.
- It'll give you a TOS just check the agree and click accept and continue.
- Wait for unity to finish loading your project.
- Once loaded, to the right in the inspector window. Click "Remove Readme Assets".
- Click Proceed, then let it finish loading.
- In your project window, right click and delete the InputSystem asset. It should have a lightning bolt on it.

## Move Back to Github Desktop
- Open up Github Desktop
- There should be 4 buttons, The third one should say "Create a New Repository on your local drive". Click it.
- **YOU ARE GOING TO NAME IT THE EXACT SAME THING YOU NAMED YOUR UNITY PROJECT**
- Under local path hit "Choose" 
- Locate your unity project and you are going to select one folder above the project folder. 
- **If you don't know where your project is. Go to unity hub, there should be 3 dots to the right where your project is listed. Click those 3 dots, and then click show in explorer, go one folder up (parent folder). That parent folder is what you will select.** 

<img src="https://files.catbox.moe/ae2z2y.png" width="25%" alt="showinexplorer">
<img src="https://files.catbox.moe/nku4z0.png" width="25%" alt="githubparentfolder">

- Then select the git ignore dropdown, and press the U key, it should highlight "Unity" Click it. 
- Click Create Repository
- After it finishes it may prompt you to initialize git lfs, press the blue Initialize button. 
- At the top press "Publish Repository"
- You can then just press the Blue "Publish Repository" Button

## Install Git
- Go to this website <https://gitforwindows.org> 
- Click Download.
- Open the exe and go through the install process (Hit Next, Next, Next, Next, Next, override the branch name for new repositories then click Next, Next, Next, Next, Next, Next, Next, Next, Install. Uncheck view release notes, then click finish.)
- Close your unity project and unity hub.
- On your tool bar, there should be an up arrow to the right.
- You should see the Unity Hub Icon, right click it and click "Quit Unity Hub"

<img src="https://files.catbox.moe/ddr90r.png" width="15%" alt="hubquitarrow">

## Downloading DREditor
- Open unity hub and open your unity project.
- Once loaded. Go to Window/Package Management/Package Manager
- In the top left corner there should be a + symbol, click that and "Install Package from git url"
- Paste <https://github.com/CertifiedSharp/DREditorInstaller.git> into the field, once it loads, the system will be automatically import the DREditor Registry.
- It'll give you a prompt just click close.
- You can close out the project settings window. 
- Go back to your package manager.
- On the left most side, at the bottom you should see "DREditor" Under "My Registries". Click it.
- Here you can download whatever DRE packages you need, for now, download the DREditor Core.
- On start up of the core, The Dependency system should walk you through everything.