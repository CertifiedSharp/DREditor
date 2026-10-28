# Starting a Prologue
## Setting up the Room State Manager
- Search in the project window for the Room\_State\_Manager and put it into the scene, make sure it's position is 0,0,0.
- Right Click Room\_State\_Manager: DREditor/Create Project Prefab Variant.
- Open up your new project prefab variant by selecting "Open" in the top right.
- Select "Actor" in the hierarchy.
- Fill out the Character Database field.
- Ctrl + S to save.

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

## Building a Room Save
- On the Room State Manager, if you haven't already, Press the "Initialize Builder" Button.
- In your project folder, go to where your character prefabs are. 
- You can click and drag them into the scene, place them wherever you like.
-  Click on your character in the hierarchy, in the inspector window, under the Actor Script, there is a drop down called "Conversations".
- Add a new conversation by hitting the "+" button.
- Assign the dialogue you want the character to have by filling out the top most dialogue field on the characters conversation.
- Once you're ready to create your room save, just go to your Room State Manager.
- Select the objective you want the character and it's conversation to be available at. 
- Click "Create Room Save".
- Once you've created your room save, you can click the "Clear All" Button. 
- Ctrl + S to save the scene.

### Overwriting a Room Save (Updating a Room Save)
- Press the load button on the Room State Manager for the save you want to edit. 
- It should load the room save, simply edit whatever you want changed. 
- Press Overwrite on the Room State Manager.
- Confirm the Overwrite.
- Press the "Clear All" button. 
- Ctrl + S to save the scene. 

## Triggering a Progression Flag From Dialogue
- Click on your dialogue asset and scroll to the very bottom. 
- Click the "Trigger Flag" Button
- Here you can select what flags you want to set as triggered once the dialogue ends.

## Rules of Thumb for making Objectives/Progression Per Chapter
- The first objectives being daily life, and however many flags you need.
- If you are required to talk to a character in order to leave a room or progress, then it will have it's own progression flag. 
- The investigation objective at minimum must as many flags as there are truth bullets for that chapter + 1 , the +1 being the flag to notify that all truth bullet flags have been completed. 
- The Trial Objective only really needs 1 flag, that being that the trial is completed, which should be triggered on the final trial dialogue asset played in the trial.
- A Post-Trial Objective that only really needs 1 flag to represent that the post trial (and therefore the chapter) has been completed.

