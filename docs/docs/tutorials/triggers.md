# Triggers
Triggers, are game objects that contain 2 types of scripts. Actuators and Subsequents. 

Simply put, the Trigger system can be understood as objects used in Room Building that can be best understood as: _"If something happens, the do something."_

As an example, by looking at a trigger game object, you can understand what a trigger does simply by reading what it's individual parts do.

Example: Actuator: OnLoadActuator | Subsequents: DialogueSubsequent

Read As: _"When the room loads, start dialogue."_

## Actuators
Stemming from the definition of a mechanical device that moves or controls or **starts** something, Actuators are categorized as the scripts that decide *"How a trigger can be activated, or start."* . There will usually only ever be one Actuator per Game Object.

- OnLoadActuator: Activates the moment the room loads. (You don't need to fill out any info on this script.)
- OLFlagActuator: Activates the moment the room loads, and specific progression flags are or aren't triggered. (Only fill out "Call if Flags are" and "Flags" with the names of the flags that should match "Call if Flags are" in order for the Actuator to activate.)
- HitboxActuator: Activates the moment the player enters the hitbox. (The system will add a hitbox for you to manipulate.)

## Subsequents
Subsequent: The second half of the trigger system, where they are categorized as the scripts that decide *"What a Trigger should do once it's been activated."* or _"The subsequent action after actuation."_ . There is no limit to the amount of Subsequents per game object.

- DialogueSubsequent: Responds to Activation by: Automatically starting dialogue.
- LDialogueSubsequent: Responds to Activation by: Automatically starting dialogue depending on local dialogue evaluation.
- MovePlayerSubsequent: Responds to Activation by: Moving the Player to the position outlined on the subsequent.

## Creating a Trigger
- In the hierarchy, Right Click: Create Empty
- In the inspector window, click the "Add Component" button in the inspector, and then type the names of any of the Actuator scripts mentioned above.
- Click the "Add Component" button in the inspector, and then type the names of any of the Subsequent scripts mentioned above.
- Once both scripts have been filled out. The trigger is considered complete, and can be used in combination with Room Building.

