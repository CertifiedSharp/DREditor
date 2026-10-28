# Non-Stop Debate
```ALERT: You must have Truth Bullets and a Difficulty State Database made in order to use this functionality!```
## Create your NSD Manager
- In your project window, in the search bar, look for NSD\_Manager\_Template
- Click and Drag it into the hierarchy.
- Set it's X and Z position to 0
- Set it's Y position to 2.01
- Right Click NSD\_Manager\_Template: DREditor/Create Project Prefab Variant
- Now Right Click: Delete the NSD Manager in the hierarchy.
- In your project window, go to where your trial scene is.
- Left Click to select it, then Ctrl + D to duplicate it.
- Press F2 to rename it GYM_NSD
- Open up GYM_NSD.
- In your project window, Search for your projects NSD_Manager and click and drag it into the scene.
- Press the Open Button
- In the Hierarchy Search Bar search for Trial\_Camera\_Animation\_Handler and select it.
- Fill in the TCAD field with the Trial Camera Anim Database you made in the previous tutorials.
- Select your Trial_Manager in the hierarchy. Uncheck "Play on Start"
- On the Global\_Fade\_Canvas Press the "Toggle Fade State" Button.
- Ctrl + S to save the scene. 

## Setting Up the NSD Minigame Type
- In your Resources/Trials folder.
- Right Click: Create/DREditor/Trials/Minigame Type
- Name it: Non-Stop Debate
- Under the "Type Name" field set it to the same name: Non-Stop Debate
- Set the controls key to: nsd
- Lock the inspector by clicking the lock icon in the top right of the window.
- Click on your NSD_Manager Project Prefab Template in the hierarchy.
- Click the "Select" button.
- It should have updated your project window, showing you where the Prefab is. 
- Click and drag it into the "Prefab" field in the inspector window. 
- Press the "+" button displayed under the previously mentioned prefab field.
- In the game object field, click and drag your prefab again.
- Click on the "No Function" drop down.
- Hover over "NSDManager"
- Select "StartMinigame" under the Dynamic ScriptableObject section.
- You can now add this to an minigame type database.

## Setting Up Dialogue and NSD Gradients
- In your project window, go to the folder path: TextMesh Pro/Examples & Extras/Resources/Color Gradient Presets
- Ctrl + D any of the existing gradients
- F2 and rename it to: HitText
- Repeat the last 2 processes twice, naming them: ConsentText and ResponseText
- Set all their color modes to Vertical Gradient, and set each top and bottom color.
- HitText: Top: #FEBF2DFF Bottom: #F78C1EFF
- ConsentText: Top: #14B8FFFF Bottom: #317BFFFF
- ResponseText: Top: #46CBE6FF Bottom: #FFFFFFFF
- Hittable Text will not display properly in NSD and in regular dialogue, encapsulating dialogue text with the @ symbol will color it with the HitText Gradient. Ex: @Dialogue Text@ Note: You do not need to use the @ symbol when making NSD Text/Panels.

## Create the Phrase Preset Database
- In an organized folder, Right Click: Create/DREditor/Minigames/Phrases/Phrase Preset Database
- In the inspector window of the newly created asset. In the top right there should be a symbol that looks like dials and sliders. 
- Clicking on it should allow you to select a preset. There should be one available to you, double click it. 
- This should fill out the list of presets.

## Creating an NSD Instance
- In a project folder you've organized: Right Click: Create/DREditor/Minigames/NSD
- Common Naming Convetion is: NSD\_ChapterNumber\_NSDNumber (NSD\_0\_0)
- On the surface, the debate is basically no different from regular dialogue, and can be filled out in the exact same way. 

## Creating the End Panel Dialogue
- Right Click: Create/DREditor/Dialogues/Trial Dialogue
- Naming Convention is: NSD\_ChapterNumber\_NSDNumber\_END (NSD\_0\_0\_END)
- Put minimum of one dialogue line from the protagonist. 
- On your NSD Asset Fill in the "Ending Panel Group Dialogue" Field with the END Trial Dialogue asset you've just made. If the player makes it to the end of the conversation, this is the conversation that will play before the NSD restarts. 

## Filling out the NSD Instance
After Filling out the debate conversation like you would normal dialogue, we can then edit the details of each panel/line and animate the words being spoken during the debate.
But because there is literally so much information surrounding NSD, you can view a Full Showcase video going over almost every single detail that goes into creating an instance of these minigames. 
Keep and mind that this recording was made many months before DREditor Release during a Dev log.
So some things may be slightly different, but a majority should still be relevant.

[DREditor Beta Preview: NSD Showcase](<https://youtu.be/1Ghg2DkyvBU?si=9-C7XRA9W6iaGfdW&t=404>)

[Fangan Project Tutorial Scouting: NSD Timestamp](<https://youtu.be/NjZxHMCHC_U?si=ZTiiSDsewK7mHNuB&t=7037>)

