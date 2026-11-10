# Truth Bullets
## Importing Truth Bullets
The Syntax for importing truth bullets with its description is:

TruthBulletName <br/>
//Description

TruthBulletName <br/>
//Description

- Menu Item: DREditor/Import/Truth Bullets
- A window will show up, Close it. The system has Automatically made a database.
- In your project window: Resources/Evidence
- Menu Item: DREditor/Import/Truth Bullets
- In the text area, fill out your truth bullets according to the syntax mentioned above.
- Click Import.
- All your truth bullets should now be made.

## Create a Collection of Evidence
- In your Resources/Evidence folder.
- Right Click: Create/DREditor/Investigations/Evidence
- Left click on the newly made asset.
- In the inspector window, in the most top right, you'll see an unlock icon, click it to turn it to a lock icon.
- In your project window, mass select the truth bullets you made.
- Click and drag them into the truth bullets list in the inspector window.
- Click the lock icon to unlock the inspector window. 
- Click on your evidence database. 
- Click and drag the evidence asset into the list of evidence.
- Ctrl + S to save as usual.

## Setting up the Truth Bullet Menu
- Go to your Pause\_Menu\_System Project Prefab Variant and open it.
- If your Pause\_Menu\_System Prefab has the Prefab Variant icon after opening it, then Right click it: Prefab/Unpack.
- Right Click Pause\_Menu\_Template: DREditor/Create Project Prefab Variant
- Open up your Pause_Menu Project Prefab Variant.
- If your Pause_Menu Prefab has the Prefab Variant icon after opening it, then Right click it: Prefab/Unpack.
- Right Click Truth\_Bullet\_Menu\_Canvas\_Template: DREditor/Create Project Prefab Variant
- Left Click it, find the "Back Group" Field. Click and drag Pause\_UI\_Canvas into it.
- Open up your Truth\_Bullet\_Menu\_Canvas Project Prefab Variant.
- Under the Truth Bullet Menu Script fill in the evidence database field with your evidence database asset.
- Ctrl + S to save the prefab.

### After doing this, you may need to rewire the Pause Menu
- Open up your Pause\_Menu\_System prefab.
- Drop down your Pause_Menu
- Left click on your Pause\_Menu\_System 
- There should be a fields labeled "First Group" and "Pause Visibility" if they are empty, then proceed with these steps, otherwise, you can skip this.
- Click and drag Pause\_UI\_Canvas into the First Group Field.
- Click and drag Pause\_UI\_Canvas into the Pause Visibility Field.
- Drop down your Pause\_UI\_Canvas
- Drop down Main_Group
- Click on your Settings\_UI\_Button, if you see a field saying "None (Menu Group Base)" then proceed with these steps, otherwise, you can skip this.
- Drop down your System_Menu
- Click and drag System\_UI\_Canvas into the previously mentioned field on the Settings\_UI\_Button.
- On your System\_UI\_Canvas if the field labeled "Back Group" is empty, click and drag the Pause\_UI\_Canvas into the field. 

## Giving the player Truth Bullets
- On the dialogue asset you want to give the player a truth bullet.
- Add a new line.
- Set the speaker to the Tutorial.
- For the text you can put: ""XYZ" has been added to your truth bullets."
- Add a new Dialogue Event "Truth Bullet Get" 
- Assign the truth bullet you want to unlock for the player.

## Mass Applying Conditions to Dialogue Assets.
Every dialogue that you have a truth bullet get, you'll want to not only trigger the flag for getting that truth bullet, you'll also want a "Add Condition" on all these dialogues checking to see if all of the truth bullet flags are triggered, and if so, to play a dialogue informing the player that all truth bullets in the area have been found, and to move on to whatever next steps in your story's investigation should take place. Of course, while you'd have to do this for every single dialogue with a truth bullet get, you can mass edit dialogues to apply that condition.

- Mass select all relevant dialogue assets.
- Menu Item: DREditor/Dialogues/Mass Edit Dialogues
- Fill in the information in the wizard, the GUI is the same as if it were on a dialogue asset.
- When done, simply click the "Apply to Dialogues" button. 
- You should find that your settings have been applied to all selected dialogue assets.

