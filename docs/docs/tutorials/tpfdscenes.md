# TPFD Scenes
## Making the Scene & it's Room State Manager
- Go to your GYM_Dialogue scene in the project window. 
- Ctrl + D 
- Select the duplicate and press F2. Or Right Click: Rename
- Rename it to GYM_TPFD or really whatever you wish.
- Double Click to enter the scene. 
- Select the Room State Manager (It currently has data for the dialogue scene) Right Click: Delete
- In the project window go and find your Project Prefab Variant of the Room State Manager.
- Click and drag it into the scene.
- Press the "Initialize Builder" Button

## Clear the Template Environment
- In the hierarchy, there should be a game object called "Environment" with a dropdown. 
- Left click on the first game object named "Cube" 
- Hold Shift and then select the bottom most game object named "Cube" this should mass select all of the cubes.
- Right Click: Delete. This should clear the room to just a simple flat floor.

## Set up the Arrow Canvas for your Project
- In the project window, search for Arrow\_Canvas\_Template
- Click and drag it into the hierarchy window. 
- On it's dropdown in the hierarchy, it should have all the image components for each arrow. You should only ever need 2 images to fill out this Template, an arrow pointing to the right, and an X symbol.
- Assign the images to each Arrow and X_Mark's Image components.
- Right Click Arrow\_Canvas\_Template: DREditor/Create Project Prefab Variant
- You can now drag and drop the newly made template for every TPFD scene you have!

## Setting up the TPFD Manager
- DREditor/Gates/Auto Setup Gate 
- Check the Is TPFD box on the gate that was made and showed up in the inspector.
- In the project window type "TPFD_Manager". Click and Drag it into the hierarchy. Zero out it's position.
- Ctrl + S to save the scene.
- Click on your TPFD_Manager in the hierarchy and in the inspector, check the "Debug Mode" box. 
- On your Project Prefab Variant for Room\_Loading\_System in the scene, hit the drop down and click on the Room_Loader. 
- Check "Debug Load Room On Start".
- Press the Unity Play Button.
- Alt + Tab to free your mouse, and click on the TPFD_Manager in the hierarchy.
- In the inspector window, you can manipulate the camera position and boundaries of the scene in TPFD mode. Adjust them to your liking. 
- Once you're done, **DO NOT EXIT PLAY MODE**, right click the title of the TPFD Manager script in the inspector window, and copy the component values. 
- _Then_ exit play mode. 
- Right click on the title of the TPFD Manager script. 
- Click "Paste Component Values".
- Uncheck the "Debug Mode" box.
- Ctrl + S to save the scene.

If you want to leave a TPFD room by pressing Escape or the R key, you must make a TPFD Door. 

