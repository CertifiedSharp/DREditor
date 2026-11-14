# Dialogue Events
Dialogue Events are usually processed at the beginning of the line unless otherwise configured within the dialogue system's monobehaviours.

## Simple Dialogue Events
Because Dialogue Events vary in complexity, in order to keep this page organized, dialogue events that can be explained simply will appear on this list.
### Kill Character
Adds the filled out first name of the character to register them as dead to the system. 
### CG Flash
If CG mode is active, Flashes a white overlay on the active CG.
### Unlock Chapter Select Scene
Sends the filled out chapter select point in time to the Chapter Select System to be unlocked on the players save file. 

```ALERT: Chapter Select System Documentation Unavailable.
REASON: At time of writing documentation no outside user recorded project has ever made it to this point in development. If you believe that your project has, please contact the project maintainer and documentation will be prepared for you and uploaded to this page.```

### Cam To Position
Animates the camera to the transform position of the GameObject in the scene, that matches the name sent. With an additional check box on whether to keep the game the currently speaking actors sprite mode active.
### Change Item Select
Changes the selectability of an object in the scene by it's Game Object name.
### Object To Position
Moves a Game Object in the scene to the specified position (and rotation if specified).
### Shake Object
Shakes the Camera or the Current CG using the options provided.
### Show Item
Will display the specified image sliding in using the Show\_Item\_Canvas\_ETemplate. 
### Sprite Flash
The current characters sprite will flash white.
### Trial Tutorial
Tutorial Assets can be made, using this event is how you display these assets during gameplay. 

## Animation Event
Used for calling environmental animations during dialogue. Invokes a Dialogue Event that should correspond with a game object in the scene that has a AnimationEventHandler.cs script filled out. 
The dialogue system will wait for the animation to finish before continuing dialogue. The "View Done" field is purely for visual debugging on whether the animation was considered finished.

```ALERT: Animation Event Handler Documentation Unavailable.
REASON: At time of writing documentation no outside user recorded project has ever made it to this point in development. If you believe that your project has, please contact the project maintainer and documentation will be prepared for you and uploaded to this page.```

## CG Display
- **CG**: The image that will be displayed as the CG. 
  - **If no CG is currently active, and this field is filled, then starts CG mode with this image.** 
  - **If this field is NOT filled, then ends the CG mode on this line.**
- **Instant Set**: Normally, when going from on CG to another, they're crossfaded into each other. Checking this box instantly sets the CG.
- **Use Full Fade**: Check this box if instead of cross fading into another CG, the screen fades to black, and then when it fades out of black, the new CG will be there.
- **FMOD Field**: Fill out this field if you want audio to play during the change between CG's or between a CG starting/ending.
- **GameObject Field**: For GameObject CG's. Custom Game Object prefabs for things like animate CG's can be played by assigning them to this field where they will be spawned. 
- **Play Only**: The name for this will likely be changed, but when checked, instead of the fading transition being black, it's white, this is for flashbacks that will also display dialogues.

## Change Window Pattern
```ALERT: This event is outdated and unused by the system, and while currently obsolete, was considered useful enough to keep within the engine. If you want to use this event, you must program a script that listens to it.```

## Char Intro
**Note: The Character\_Intro\_Canvas\_ETemplate must be filled out in order to use this!** <br/>
Uses the name given to tell the CharacterIntroAnim.cs script the name of the character intro animation to play. The game will wait until the animation has finished before continuing dialogue.

## Character Leave
- **Appear**: Check this box to have the character sprite fade in.
- **Current**: Check this box to apply the character leave process to the current character speaking, otherwise, applys this to the previous character that spoke. 
- **Exit**: Checking this box will teleport the character into the floor **during the line AFTER the current line**. Otherwise they will still be present in the overworld or when the camera pans to another character.

## Custom Events
This Event has 2 abilities, the first, simply allows you to call a dialogue event by it's registered string.<br/>
As an example, you could call the Dialogue Event for an animation event on a Custom Event. Except when called through the Custom Event, the game will not wait for the animation to finish before executing continuing processes. <br/>
This is best used for programmers who have one-off Dialogue Event Listeners for whatever they're needing.


The second ability, allows for Programmers to create a Custom Dialogue Event class by applying the CustomDialogueEvent Attribute, and giving a class type that inherits from IDataCapsulate.
Once both these conditions are met. The Event will be present in the Custom Event Dropdown menu. No wiring required!
Additionally the system already has some Native Custom Dialogue Events for you to use:

- **Spawn Object**: Spawn a GameObject into the scene. The position and rotation on the prefab will be it's spawn location.
- **Force Protag View**: Forces the protagonist view to the specified state.
- **Swap Protag Window UI Sprite**: Swaps the current protagonist, using Character UI Sprite Data so that the game knows how to display that specific character as the protagonist in the protag window. 
- **Map Entry Change**: Updates the players Map, allowing you to outline what markers are at which places on the map. This process requires the Map System to be set up. 

```ALERT: Map UI System Documentation Unavailable.
REASON: At time of writing documentation no outside user recorded project has ever made it to this point in development. If you believe that your project has, please contact the project maintainer and documentation will be prepared for you and uploaded to this page.```

## Faint Anim
Applies a fainting animation onto the currently speaking character.

- **Faint**: True: Falling down. False: Getting up.
- **To Y**: The Y position to move the character to (usually a large negative number to move them under the floor.).

## Flashback
Plays each added flashback in CG mode, except no dialogue will play, perfect for sequences where you can add images with a sepia filter applied to it. Once all images have been displayed, immediately moves out of CG mode.
## Last Actor Sprite
Changes the sprite of the last focused character sprite to the one specified. This event is almost exclusively used for scenarios where the current character speaking is NOT the one the camera is currently focused on. <br/>
Example: The protag thinks internal dialogue, and on that same line, the sprite of the character in front of them changes. 

## Move Character
Moves the specified character to the outlined position and rotation.

- **Record Character Position**: While in play mode, you can go into the scene view, and move the Actor in the scene to a specific position. Then, clicking the button will record the current position and rotation of the specified character. 
- **Move All**: Checking this box moves ALL characters in the scene to the specified position and location. (Good for scenarios when all characters in a room leave.)

## Screen Transitions
```ALERT: This event is outdated and unused by the system, and while currently obsolete, was considered useful enough to keep within the engine. If you want to use this event, you must program a script that listens to it.```
```ANALYSIS: This process will likely be applied to the DREditor.Fading/DREditor.Fading.Handling package during a rework of the core system.```

## Toggle Blur
The system mostly deals with this automatically, but whenever we focus on an object with the camera 90% of the time it's an actor, while focusing on an actor, you'll notice that the background becomes blurry. This is the blur that will be toggled by using this event.

## Video Display
Nearly all aspects of Video Display are like CG Display except for:

- **Initial Clip**: A clip that will be played as a way to transition to the main clip. For Example: If you want an entering video, and then you want you want your main clip to play, and then loop the main clip. (Because if you combined the initial clip and the main clip, looping would also play the initial portion of the clip)
- **Play Only**: Plays only the video and then leaves CG mode.
- **Is Looping**: Whether or not the Main Clip should loop. 

## Flair Event
Invokes Flair Events from the flair system, along with settings for invoking flairs. <br/>
Flairs are overlayed visual effects on top of or near characters. With position usually taking the current actors head position into account. 

```ALERT: Flair System Documentation Unavailable.
REASON: At time of writing documentation no outside user recorded project has ever made it to this point in development. If you believe that your project has, please contact the project maintainer and documentation will be prepared for you and uploaded to this page.```

## Truth Bullet Get
Adds the specified Truth Bullet to the players Truth Bullet Inventory using Truth\_Bullet\_Get\_Canvas\_Template and Truth\_Bullet\_Get\_Event\_Handler. Displaying to the player that they've received one. You should always have the Tutorial Character speaking during the line this event is called on.
To learn more about Truth Bullets and Investigations See: [Truth Bullets](https://dreditor.net/docs/tutorials/investigations/truthbulletstutorial.html)