# Final Gameplay Setups
## Set up Gates
- In the room you wish to start your games dialogue in, go to DREditor/Gates/Auto Setup Gate.
- Scriptable Object Assigner For the Gate Database

## Set up Progression
- Create a new Folder in Resources called "Progression" 
- Right Click: Create/DREditor/Progression/Database
- Right Click: Create/DREditor/Progression/Chapter named "Prologue"
- Set Save Title, and Title
- Create Objectives with O0_Dialogue or whatever naming convention you're comfortable with. 
- Create a new flag with whatever naming convention. 
- Plug the Prologue chapter into the progression database. 
- Scriptable Object Assigner For the Progression Database


## Creating the Tutorial Character
- Go to where your characters are located in the resources folder. 
- Right Click: Create/DREditor/Characters/Student
- Name the character "Tutorial"
- Set the first name to "Tutorial"
- Add them to the character database. 

## Creating the Leave Ask Dialogue
- Go to your dialogues folder. Make a new folder called "LeaveAsk" 
- Right Click: Create/DREditor/Dialogues/Dialogue
- Name the dialogue asset "LeaveAsk". 
- Under the Game\_Manager\_Template, Select the Leave\_Ask\_Manager\_ETemplate
- Plug in your LeaveAsk Dialogue into the field.
- Right Click Game\_Manager\_Template: DREditor/Create Project Prefab Variant
- Go to the LeaveAsk dialogue asset, have the first line be spoken by the tutorial, and write "Would you like to leave?" or something close. 
- Press the "Add Choice" button.
- Write Yes and No for Choice 1 and 2 Respectively.
- At the very top of the dialogue asset should be a check box that says "Instant Dialogue" make sure it's checked.

## Setting up your Music Jukebox
- Go to your resources folder. 
- Right Click: Create/DREditor/Audio/Music Playlist FMOD
- Under Sound\_Manager\_FMOD\_ETemplate, select Music\_Manager\_FMOD
- Assign your Jukebox to the Jukebox field.
- Right Click Sound\_Manager\_FMOD\_ETemplate: DREditor/Create Project Prefab Variant 

## Setting up the Remaining Project Prefab Variants
- Right Click Progression\_Manager\_ETemplate: DREditor/Create Project Prefab Variant
- Right Click Room\_Loading\_System: DREditor/Create Project Prefab Variant 
- Right Click Dialogue\_System\_Template: DREditor/Create Project Prefab Variant
- Right Click Pause\_Menu\_System\_Template: DREditor/Create Project Prefab Variant
- Right Click Controls\_UI\_Canvas\_ETemplate: DREditor/Create Project Prefab Variant
- Right Click FTE\_Manager\_ETemplate: DREditor/Create Project Prefab Variant

