# Trials
## Initializing Trial Scene Setup
- Open up your GYM_Trial scene, or whatever you named your trial scene. 
- Mass Select from the Controls\_UI\_Canvas\_ETemplate to the Letter\_Box\_Canvas
- Right Click: Delete
- Menu Item: DREditor/Utility/Scriptable Object Assigner
- Click the "+" button on the Scriptable Objects list.
- You're going to fill the field with your Character Database
- Click "Assign to All Components In Scenes"
- Go to your GYM_MainMenu scene.
- Mass Select your Global Systems (Select everything that is NOT from the Build\_Version\_Canvas to the Fast\_Forward\_To\_Line\_Tool)
- Right Click: Copy
- Move back to your GYM_Trial scene.
- Right Click: Paste to the hierarchy. 
- Reorganize things to where global systems are separated from the trial systems. (Click and drag them above the ====ENVIRONMENT==== game object.)
- Open your Dialogue_System Prefab
- Ensure that the 3D\_UI\_Canvas's canvas component is disabled. (Check box next to the script name)
- Make sure that you add the trial scene to the Build Scene List (File/Build)

## Setting Up Your Trial Camera Animations
- In your root project folder, Right Click: Create/Folder called Animations
- Inside that folder, create another folder called: Trial Camera
- In your Hierarchy, Left click DRCameraAnchor
- Right Click DRCameraAnchor: DREditor/Create Project Prefab Variant
- In the inspector window, press the "Open" button
- Right Click DRCamera_Pivot: DREditor/Duplicate Insert Animation
- Make sure that the target directory field says Assets/Animations/Trial Camera
- Set the Affix Separator to "None".
- Click "Duplicate", this will duplicate all the template trial animations.
- In your project window, Right Click: Create/DREditor/Trials/Camera Anim Database
- In your project folder you'll find an Asset called "TrialCamera" this is the animator controller for the trial camera. 
- Click and drag that asset into the Trial Camera Animator Controller Field. 
- Click "Import Animation Clips"

## Setting Up The Trial Dialogue Minigame Manager, TimeUp and Continue_Confirm Dialogues
- In your Dialogues folder, make a new folder "Trial".
- Inside that folder, Right Click: Create/DREditor/Dialogues/Trial Dialogue
- Name it Continue_Confirm
- Ctrl + D to duplicate it, press F2 to rename the duplicate to TimeUp.
- Fill out both dialogues with at least one line from your protag for when they refuse to give up after a game over and the line they say when they run out of time in a minigame. 
- In the hierarchy select Trial\_Dialogue\_Minigame
- Fill out the continue confirm and time up fields with the dialogues you've just made.
- Right Click Trial\_Dialogue\_Minigame\_Template: DREditor/Create Project Prefab Variant
- Press the Open Button
- In the Hierarchy Search Bar search for Trial\_Camera\_Animation\_Handler There should be 2, select both or one at a time.
- On both, Fill in the TCAD field with the Trial Camera Anim Database you made in the previous tutorials.

## Setting Up the Trial Discussion Minigame Type
- In your Resources folder, make an new Folder named: Trial
- Right Click: Create/DREditor/Trials/Minigame Type
- Name it: Trial Discussion
- Under the "Type Name" field set it to the same name: Trial Discussion
- Set the controls key to: td
- Lock the inspector by clicking the lock icon in the top right of the window.
- Click on your Trial\_Dialogue\_Minigame Project Prefab Template in the hierarchy.
- Click the "Select" button.
- It should have updated your project window, showing you where the Prefab is. 
- Click and drag it into the "Prefab" field in the inspector window. 
- Press the "+" button displayed under the previously mentioned prefab field.
- In the game object field, click and drag your prefab again.
- Click on the "No Function" drop down.
- Hover over "TrialDialogueMinigame"
- Select "StartMinigame" under the Dynamic ScriptableObject section.
- You can now add this to an minigame type database.

## Setting Up a Trial Discussion
- You can now make Trial Discussion assets, Right Click: Create/DREditor/Dialogues/Trial Discussion and just fill in the fields with Trial Dialogues. 
- The common naming convention for trial discussions is: TD\_ChapterNumber\_DiscussionNumber with Trial Dialogues named: TD\_ChapterNumber\_DiscussionNumber\_DialogueNumber

## Setting Up the Trial Minigame Type Database
- In your Resources/Trials folder.
- Right Click: Create/DREditor/Trials/Minigame Type Database
- Add your Trial Discussion minigame type to the databases list.

## Creating a Trial Builder Asset
- In an organized folder in the project window.
- Right Click: Create/DREditor/Trials/Trial Builder Asset
- Click the "Add a new part of the trial" button.
- From the dropdown you can select which minigame type you want. 
- Fill the field with the minigame asset for that minigame's type. (Trial Discussion Assets for the Trial Discussion type, etc.)

## Setting Up Your Trial Manager Prefab
- Left Click on the Trial\_Manager\_Template in the hierarchy.
- Under the Trial Sequencer Script, fill in the trial discussion type with the trial discussion type you made previously. 
- Right Click Trial\_Manager\_Template: DREditor/Create Project Prefab Variant
- Under the Trial Manager Script you can now add Trials per chapter. Make sure they are kept in the proper order. (Prologue/Chapter 1 first and so on)

## Setting Up Your Trial Characters
- Menu Item: DREditor/Characters/Auto Setup Trial Character
- Under the Spawn Settings header, look for the "Ignored Characters" list.
- Add your Tutorial character to this list. 
- The only other option you should ever really mess with is the "Number of Objects" dial to set up how many characters will participate in the trial. (Not Counting Hosts)
- Use the Dropdowns in the circle UI to set up each characters assigned seats in the trial. Where the top most seat will be in front of the brown host chair in the scene template.
- When ready, click the "Create" button at the bottom.
- Ctrl + S to save the scene.
- To do your hosts, essentially do this same process, but fill the "Ignored Characters" Field with the already created Characters + Tutorial.
- Once they spawn, you can simply just move the host to where you wish.

## Post Trial Scene 
After the results of a trial, what usually happens is a dialogue sequence. So we'll set up the Trial scene as a TPFD (2.5D) scene for the game to transition out of Trial Gameplay. 

- Select your Trial Scene in your project window. 
- Ctrl + D to duplicate it. F2 and rename it to something like: GYM_Trial2D
- Open it up.
- Delete everything except for the ====ENVIRONMENT==== game object in the hierarchy.
- Under the ====ENVIRONMENT==== game object's Character object, delete the Trial Characters present.
- Click on the ====ENVIRONMENT==== game object under the transform component near "Scale" click on the chain icon. 
- Set the x scale to 1/3 and hit enter. 
- The entire room should now scale down to a smaller size. (The Trial scene is 3x bigger so we're scaling it down.)
- In your project window, search for your Room State Manager, and Set up the room as you would a TPFD Room.
- You'll want an On Load Actuator with a dialogue subsequent to start up the post trial dialogue scene.
- It is suggested that the dialogue ends on a black CG for smoother transition to the end of the chapter.
- Make sure that you add this scene to the Build Scene List (File/Build)
- At the bottom of your Trial Builder asset, fill out the field with GYM_Trial2D so that when a trial ends, it moves to that scene. (Or you can fill in the gate field) (Do NOT fill in both)

## Finishing a Chapter
- On the final dialogue asset that's played, scroll to the bottom and click "Enter Scene"
- Write GYM_SavePoint or whatever your Save point scene name is. 
- Open up GYM_SavePoint
- Make sure that you add this scene to the Build Scene List (File/Build)
- Open up GYM_Credits
- Make sure that you add this scene to the Build Scene List (File/Build)
- Assuming all of the steps have been completed in combination with the details of your project, the gameplay loop should now be complete, and you can export a build of your game.


