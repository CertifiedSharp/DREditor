# Dialogue End Events
Dialogue End Events are specific middle to large processes that affect the game after a dialogue asset displays all it's lines. Dialogue End events can always be found as the buttons displayed at the very bottom of a dialogue asset.

```REPORT: Dialogue End Events are a part of DREditor's Core, and will likely be reworked in order for more a more flexible structure, and to allow the addition of more End Events.```

## Direct To
If you want to continue playing dialogue and organize dialogue sequencing easier, you can fill in the field of the next dialogue to play, and the game will transition to it as if they were all one dialogue.

## Add Condition (Variables)
You can use Progression Database Objectives and flags to evaluate the condition. Allowing for dialogue to branch off if the condition is true or false. If the condition is false and no dialogue has been filled in for the false field, dialogue will end normally.
You can also use a BoolWithEvent to evaluate the dialogue, and while it isn't _usually_ ever used in development, it's available to the user if they which for better flexibility. 

## Custom Call
Calls a Custom Dialogue Event. Check the Call Break box in the event that you want to force the dialogue system to resolve itself in a semi-broken state to allow other processes to manipulate the dialogue system. **This process should be used with caution.**

## Add Choice
Displays Fields to write the choice text and the dialogues to direct to once that choice is chosen. As well as Custom Call End Event Processes. Just in case!

## Enter Scene
When dialogue ends, automatically freezes the player and moves them to the designated Scene. 

- **Name of Scene**: The name of the scene to move to that does not have a Gate.
- **To Dark**: Fades the screen to black instead of using the normal room transition process visuals.
- **To Menu**: Check this if the scene you're moving to is the main menu to end the game.
- **On Load No Dark**: Check this box if you are loading to the next scene and the screen is still dark but the game is still playing. 
- **Gate Field**: The Gate for the scene you want to move to.
- **Text Field**: The name of the objective you want the game to move to.
- **To Next Natural**: Moves the current objective to the next objective in progression.

## Trigger Flag
Select what flags you want to set as triggered once the dialogue finishes displaying it's assets lines.

## Phase Change
This end event will tell the game state that we are moving to a different phase of the game, like FTE's or Investigation/Deadly Life. This is helpful when you will want to reload the current room you're in to another objectives room state, since normally, you have to leave and come back in order to see the next objective room state. 

- **Next Objective**: When checked, will move the current objective forward to the next objective in progression.
- **Dialogue Field**: The dialogue to play after the game phase changes.
- **ID**: The ID number for what phase to change to. 
     + 0 = Investigation
     + 1 = Free Time