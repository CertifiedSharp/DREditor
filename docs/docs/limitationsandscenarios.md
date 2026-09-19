# Limitations and Scenarios
These are things that are never directly mentioned within the engine or a tutorial but is relevant enough information that should be stored somewhere.

## Limitations
- Unless you built it yourself, UI templates will not have mouse point and click interactions since the main menus default state is to have the cursor locked. 
- At the current moment, there is no default Free Time Event Minigame for the player to obtain currency, however this may not always be the case in the future.
- When using chapter select, the active save data is based on chapter select. (Note: RTMM = Return To Main Menu)
  * V3: Load => Ch select => RTMM => Continue => data when file originally loaded. 
  * DREditor: Load => Ch select => RTMM => Continue => data of the ch select.

## Limitation Scenarios
- If a ValueWithEvent object has been created for the purpose of updating a runtime state, then a script that implements IToTitleScreen should be made/implement OnDestroy() that resets it's value so that it's state does not carry over during RTMM.
- There might be load inconsistency bugs between exported live builds if aspects from the previous builds dialogue has changed. As an example, if you changed which sprite is displayed in a chapter of a release build. In the new build, the old sprite will likely be shown on an older save file.
- If DEFAULT UNLOCKED Chapter Select Scene Data does not contain the unlock data for it's own point in time, the displayed unlocked chapter selects for the save file that starts on that scene will not display unlocked chapter selects properly until it unlocks another one of the chapter selections through gameplay. (The exception to this would be prologue daily life as this should be unlocked on the first line, which means the scene data asset doesn't need to add chapter select data.)