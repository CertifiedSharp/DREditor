# Limitations and Scenarios
These are things that are never directly mentioned within the engine or a tutorial but is relevant enough information that should be stored somewhere.

## Limitations
- One of the least flexible UI's in DREditor is the Main Menu. Because of how the system operates, there are 2 menu groups that must be architecturally structured so that the system doesn't break. You can refer to these from the Main Menu Canvas Template.
  * Start Group: The "Press Any Key" Menu Group. While this specific group isn't _required_ it is _highly_ recommended.
  * Title Group: Must contain New Game, and Load Game. 
       + With New Game moving to the Menu Group's New Game Option.
       + Load Game moving to the Save/Load UI, which once a file is loaded through the confirmation pop up will move to the Menu Group.
  * Menu Group: Must contain New Game, and Continue. 
       + With New Game, either you manually set up the script to start the game on the button, or move to the Difficulty UI. (Which outside of the difficulty's start game button, can be set up how you wish)
       + The Continue Button must be present under the new game button. So the first 2 buttons must always be New Game, and Load Game.
- Unless you built it yourself, UI templates will not have mouse point and click interactions since the main menus default state is to have the cursor locked. 
- At the current moment, there is no default Free Time Event Minigame for the player to obtain currency, however this may not always be the case in the future.
- When using chapter select, the active save data is based on chapter select. (Note: RTMM = Return To Main Menu)
  * V3: Load => Ch select => RTMM => Continue => data when file originally loaded. 
  * DREditor: Load => Ch select => RTMM => Continue => data of the ch select.

## Limitation Scenarios
- If a ValueWithEvent object has been created for the purpose of updating a runtime state, then a script that implements IToTitleScreen should be made/implement OnDestroy() that resets it's value so that it's state does not carry over during RTMM.
- There might be load inconsistency bugs between exported live builds if aspects from the previous builds dialogue has changed. As an example, if you changed which sprite is displayed in a chapter of a release build. In the new build, the old sprite will likely be shown on an older save file.
- If DEFAULT UNLOCKED Chapter Select Scene Data does not contain the unlock data for it's own point in time, the displayed unlocked chapter selects for the save file that starts on that scene will not display unlocked chapter selects properly until it unlocks another one of the chapter selections through gameplay. (The exception to this would be prologue daily life as this should be unlocked on the first line, which means the scene data asset doesn't need to add chapter select data.)