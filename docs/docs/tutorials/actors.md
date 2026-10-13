# Actors
## Assets to Prepare
### Required Minimum for One Character
- A [correctly formatted](https://dreditor.net/docs/resources/artassetguide.html) Neutral sprite of the character. 
### Suggested Assets to Prepare
- As many of the finalized versions of your characters sprites. 
- A [correctly formatted](https://dreditor.net/docs/resources/artassetguide.html) nameplate of the character. (However, if you are not planning on using _image_ nameplates and instead just text, then you can just leave/remove the field with the transparent image on the actor importer.)
- It is suggested that you set up at minimum 2-3 characters, so you'll be able to test the flow of dialogue early. 

## Import Nameplate/Texture
<details>
<summary>Note</summary>

You may notice that after importing your character with their nameplate. It may not show up correctly dimension wise on the UI. Once you've followed the later tutorial that deals with the DREditor Template Structure, once you've made your Dialogue System Prefab for your project, you can go to the nameplate image, assign one of your characters nameplates to the field, and press the **Set to Native Size** button, this should set the dimensions on the UI to the dimensions of the nameplate.
</details>

- In your root project folder, Right Click > Create > Folder named **Art**.
- In your Art folder, Right Click > Create > Folder named **UI**.
- In your UI folder, Right Click > Create > Folder named **Nameplates**.
- In your nameplates folder you can click and drag all character nameplates into the project window. 
- Ctrl+a to select all
- In the inspector window, under texture type, you're going to select "Sprite 2D and UI"
- Under sprite mode, make sure it says "Single" and NOT "Multiple"
- Click apply. Done!

## Importing an Actor/Character
- Project Window: Right Click: Create/Folder name it sprites.
### For each character you want to import. 
- Inside the sprites folder, create a new folder with the characters first name, Open that folder. 
- Click and drag your sprites into the project window folder.
- Ctrl+a to select all
- In the inspector window, under texture type, you're going to select "Sprite 2D and UI"
- Under sprite mode, make sure it says "Single" and NOT "Multiple"
- Click apply.
### Auto Import an Actor/Character

<details>
<summary>Note</summary>

This process assumes that your currently active scene is your DRE_Dialogue duplicate.
</details>

- Ctrl+a to select all the sprites in your character folder.
- DREditor/Characters/Auto Setup Character  A window will appear.
- Fill out the characters first and last name. 
- You can pick a color to correspond with the character so that the dialogue asset UI can be color coded.
- The neutral sprite for your character should be populated, but if it isn't correct, you can assign the Neutral sprite for your character by clicking on the circle to the right of the field.
- Same process above for the Nameplate. 
- When you're ready, you can click the "Create" button. **Make sure that all your characters sprites are selected before pressing the Create button.**

### What the Importer has Done for you
- Each sprite now has a corresponding material. This is because during dialogue, when a character is focused on it displays the sprite, while in the overworld a material is used in order to support things like lighting effects. 
- An actor object in your hierarchy has been created and filled out for you. You can look at them in the scene view.
- Created a Character Database if one was not already present in your project.
- Created and filled out the Character Asset for your character and added it to the Character Database.
- Filled out each characters material and sprite along with setting the name for the sprite code.
- Created a Dialogue Asset of that character showcasing the imported sprites. It can be found at Assets/Dialogues/Debugging/Sprites.

### Assigning a Conversation to an Actor
At this point, you should be able to hit the play button and go up to your character and talk to them. 
It will tell you that there is no conversation present, and to not save your game on the line. We need to give dialogue to the actor so that when we click on them, we can have a conversation. 

- To do this, we can go to our Dialogues folder and find our Debug Sprite Dialogue asset 
- In the Hierarchy, click on your character, In the inspector window, you should see the actor script. 
- There will be a dropdown that says conversations. Hit the plus button.
- You can now drag your debug sprite dialogue asset into the first dialogue field.
- Click play and you can now talk to your character!

### Updating the Actor Body Position
You might notice that depending on the true height of your character, the camera position may not be right. So follow this process to set each characters camera body position.

- In your project window, to the top right, there should be a search bar. Type **Actor_Body_Debugger**.
- You should see it show up in your project window, click and drag it into the heirarchy.
- Click play mode and go talk to one of your characters.
- Press the **v** key.
- use Alt + tab to free your mouse while in play mode. 
- In the hierarchy, go to the character you're speaking to and click the dropdown next to the name.
- Then click the drop down on **Character**. You should see the game object **Body** click it.
- In the inspector window, under the transform component, you can see the XYZ Position of the body.
- Click on the number in the Y position and change it's value; or, click on the letter **Y** and drag horizontally. 
- You should see that the camera's y position is moving. Change the XYZ values for that character until the view is to your liking. 
- Once you've got the position to where you want, in the inspector, Right click on the position label, and select **Copy**. 
- Press the play button again to leave play mode.
- In the inspector window, you'll noticed that your values have been reset, so right click on the position label again, and select **Paste**.
- ctrl + s to save the changes to the scene. Do this process for each character.

### Creating your Character Prefab
As stated before, a character should now be in your scene once you've imported it. We are going to turn this into a asset so that the game can spawn the character when we go into a room where the character is located. 

- Make sure that in the hierarchy, all of your characters should have no conversations on the actor script, the number to the right should say 0. 
- Go to the root folder in your project window. (Assets Folder)
- Right Click > Create > a new folder called **Prefabs**.
- Inside the Prefabs folder, Right Click > Create > a new folder called **Actors**.
- While inside your Actors folder. In your hierarchy window, click and drag the newly made actor gameobject that was imported into the Actors folder.
- It will ask you whether to make a prefab or variant. Select **Prefab Variant**. 
- Press F2 or Right Click > Rename. Remove the " Variant" part of the text, so it's only the characters name on the asset.
- ctrl + s to save your scene.
- Go back to your root assets folder.
- Go through the folders: Resources/Characters
- Click on your character asset.
- Without scrolling, you should be able to see a field titled **Actor Prefab**.
- Click the circle on the right of the field and search for your characters prefab variant you made earlier.
- Once you double click it, it should display that it's been assigned.
- ctrl + s to save. 
- Commit to github at this point if you haven't already.

You will follow this process for each character you wish to import.

## Mass Applying your Character Database to Systems in the Scene
There are Multiple systems in DREditor that need the character database in order to function.
But in order to keep things organized and less stressful for the end user, templates exist in these scenes that are already set up, except for the character database. Now, instead of making you go find all the different objects that need the reference, a tool is available to auto assign the Character Database (as well as other things like the Progression Database) to all systems that need it in the scene.

- At the top Menu, go to DREditor/Utility/Scriptable Object Assigner
- Under the Scriptable Objects Dropdown, click the plus button.
- Fill the field with your Character Database.
- Click the **Assign to All Components in Scenes** button.
