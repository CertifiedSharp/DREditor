# Final Gameplay Setups / Starting a Prologue
## Set up Gates
- In the room you wish to start your games dialogue in, go to DREditor/Gates/Auto Setup Gate.
- Make sure you're in your GYM_Dialogue scene.
- DREditor/Utility/Scriptable Object Assigner
- There will be a list field for scriptable objects. Click the + button and fill it in with your database. 
- Click "Assign to All Components in Scenes".
- Ctrl + s to save your scene.

## Set up Progression
- Create a new Folder in Resources called "Progression" 
- Right Click: Create/DREditor/Progression/Database
- Right Click: Create/DREditor/Progression/Chapter named "Prologue"
- Set Save Title, and Title
- Create Objectives with O0_Dialogue or whatever naming convention you're comfortable with. 
- Create a new flag with whatever naming convention. 
- Plug the Prologue chapter into the progression database. 
- Make sure you're in your GYM_Dialogue scene.
- DREditor/Utility/Scriptable Object Assigner
- There will be a list field for scriptable objects. Click the + button and fill it in with your database. 
- Click "Assign to All Components in Scenes".
- Ctrl + s to save your scene.


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

## Setting up the Starting Dialogue for a Game
- In the project window search bar, type: DRE\_Start\_Alone\_Scene
- Click and drag it into your first area. 
- Create a new Folder in your Dialogues folder call it "Prologue" to keep things organized. 
- Create a starting Dialogue with Right Click: Create/DREditor/Dialogues/Dialogue.
- Ensure that your Dialogue's first line must start with a CG, if you do not have one, feel free to use the black screen. To make a CG Event, you can do so by using the drop down under dialogue events under a dialogue line and select "CGDisplay". To end the CG on another line, simply add a CGDisplay event and don't fill it in. 
- Once you complete your starting dialogue. Fill out the Starting Dialogue Field on the DRE\_Start\_Alone\_Scene.
- In the inspector, select the "Add Component" button and type "Spawnable" and add that component to the game object. 
- Right Click DRE\_Start\_Alone\_Scene: DREditor/Create Project Prefab Variant 
- Open up your newly made prefab variant. Under the Spawnable Component, to the left there are 3 dots, once clicked, you should see an option that says "Initialize Destroy Call" click it.
- Ctrl + S to save.
- Now you can use DRE\_Start\_Alone\_Scene in a Room Save.