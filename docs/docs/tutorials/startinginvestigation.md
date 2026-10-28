# Starting an Investigation
- On the dialogue asset you want to start the investigation _after_ it finishes. 
- Scroll all the way down and press the "End Event" button. 
- If you check "Next Objective" the room manager will reload the room with the state of the next objective. 
- The dialogue field when filled, is the dialogue you want to play the moment the "Investigation Start" animation would end. Starting the conversation on what the player should do to investigate first.
- The ID is the type of event, 0 is investigation, 1 is Free Time Events.
- The system should now transition to investigation once the dialogue asset finishes.

## Transitioning from Investigation to the Trial Preparation Screen
- On your dialogue asset click the "Enter Scene" button.
- Write "GYM_TrialPrep" or whatever you ended up naming your Trial Prep scene.
- Make sure that your trial preparation scene is in the scene build list. (File/Build: List should be in the window that pops up.)
- Ensure that you have a CG currently active or set to the black screen image available. Otherwise the transition may not be so elegant when moving to the trial preparation screen. 
- Once this criteria has been met, the system should move you to the trial preparation screen for your chapter during gameplay.

