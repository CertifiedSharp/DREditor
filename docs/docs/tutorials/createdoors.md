# Create a Door
## Create a ThreeD Door
- In the Hierarchy, Right click: 3D Object/Quad. Name it whatever.
- Pressing the T key, you can resize it to something resembling a door.
- Set the game object layer to the Door layer. 
- Add the Door component through the "Add Component" button.
- Set the "Door Name" Field to the exact same name as the game object. 
- Fill out the gate field with the gate you want the door to lead too.
- Ctrl + S to save the scene.

## Setting the From Position on a 3D Door
- In your 3D scene start up Play mode. 
- Walk over to where your door is. 
- Position your character as if you just came out of the door. 
- Alt + Tab out and go to the menu item: DREditor/Gates/Set Gate From Location
- The system will set your player position as the position to set the player when coming from that door from another scene. 

## Create a TPFD Door
**In order for this system to work, you MUST complete the Leave Ask Dialogue Setup Process!**

- In the Hierarchy, Right click: Create Empty. Name it whatever.
- Set the game object layer to the Door layer. 
- Add the Door component through the "Add Component" button.
- Set the "Door Name" Field to the exact same name as the game object. 
- Plug in the gate to the area you want to exit to when the player is in the TPFD room and hits the escape key or the R key.
- Check the "Is TPFD" box on the door component. 
- Ctrl + S to save the scene.

## Create a Hitbox Door
- In the Hierarchy, Right click: Create Empty. Name it whatever.
- Set the game object layer to the Door layer. 
- Add the Door component through the "Add Component" button.
- Set the "Door Name" Field to the exact same name as the game object. 
- Fill out the gate field with the gate you want the door to lead too.
- Check the "Is Hitbox" box on the door component.
- Add A Box Collider component through the "Add Component" button. Edit it to fit your needs.
- Box Collider Component should have the "Is Trigger" option checked.
- Ctrl + S to save the scene.

## Test Moving From One Scene to the Other using Doors
- In your Dialogue scene in the Hierarchy select your Room\_Loading\_System
- Check the "Debug Test On Load" box.
- You can now test moving from one area to the other in play mode.

## Lock Data
- Unlocked Dialogues: The conversation that plays once all the flags have been triggered. 
- Flags: List of strings, copy the names of all the flags that need to be complete in order for the door to unlock. 
- Locked Dialogues: The conversation that plays if all the flags are NOT triggered.
- Make sure that after you edit this data you overwrite the room save you're setting this data on.

