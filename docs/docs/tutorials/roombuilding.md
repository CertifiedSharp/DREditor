# Room Building
## Setting up the Room State Manager
- Search in the project window for the Room\_State\_Manager and put it into the scene, make sure it's position is 0,0,0.
- Right Click Room\_State\_Manager: DREditor/Create Project Prefab Variant.
- Open up your new project prefab variant by selecting "Open" in the top right.
- Select "Actor" in the hierarchy.
- Fill out the Character Database field.
- Ctrl + S to save.

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

## Rules of Thumb for making Objectives/Progression Per Chapter
- The first objectives being daily life, and however many flags you need.
- If you are required to talk to a character in order to leave a room or progress, then it will have it's own progression flag. 
- The investigation objective at minimum must as many flags as there are truth bullets for that chapter + 1 , the +1 being the flag to notify that all truth bullet flags have been completed. 
- The Trial Objective only really needs 1 flag, that being that the trial is completed, which should be triggered on the final trial dialogue asset played in the trial.
- A Post-Trial Objective that only really needs 1 flag to represent that the post trial (and therefore the chapter) has been completed.

