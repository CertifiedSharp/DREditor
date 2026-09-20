#  Alpha Project Conversion
If your project existed before 2026/the release of the Beta, please follow these steps to reduce the destruction of data in your project, as a majority of data was changed and therefore will likely need to be redone in order to work with the new architecture, usually the case with UI. We apologize for any inconvenience this may cause.
Please stay up to date either in the discord or our social media in the event this information is/needs to be updated!

## Converting Alpha code to Beta
- Convert your Unity Version by simply opening it with a different version through unity hub.
- Say yes to all conversion popups unity mentions.
- If they are sending errors, remove the serialized fields from ISaveManager & ISaveable.
- Remove Text mesh pro's examples and extras scripts
- Re-import TMP by: Window/Text Mesh Pro/Import TMP Essentials.
- Remove previous DREditor through file explorer 
- Add new DREditor (preferably through file explorer as well but the UPM should work.)
- Let it compile, then fix any errors on your project specific scripts. (if applicable)
- If the dependency window shows up follow it's instructions. 
- If they are present in your assets folder delete the DOTween folder and the CharTweener folder.
- Tools/DREditor/Dependencies/Reset Dependencies
- Go through the dependency set up process.
- Commit changes to github.

## Convert all Room Data
- In any scene, place the Room_State_Manager prefab.
- Hit the drop down and select Actor.
- Fill it in with your Character Database, the list field below it should populate.
- Rename it to Room_State_Manager_YOURPROJECTACRONYM Example: Room_State_Manager_EG
- Drag your now named prefab into the project window. 
- Turn it into a prefab variant.
- Commit changes to github.
- Tools/Convert/Convert All Room Data Builders
- Commit changes to github.
- Tools/Convert/Convert Scene Room Managers 
- Fill in the field with your newly made Room_State_Manager_YOURPROJECTACRONYM
- Press the button below.
- Commit changes to github.

## Convert Doors
- Tools/Scriptable Object Assigner
- Under Components just put the Door script.
- Under Scriptable Objects put these:
    - InLeaveProcess
    - HasMenuAccess
    - CurrentControlsKey
    - CheckOverlay
    - OnFinishedLoadingRoom
    - ChangingObserve
    - InMenu
    - IsLoadingToRoom
    - InDialogue
- Press "Assign to All Components in Scenes"

## Actors
- Select all actor prefab variants.
- Click Tools/Convert/Actors/Update Selected Actor Display Names.
- Select all actor prefab variants again.
- Click Tools/Convert/Actors/Update Selected Actor Colliders.

## Convert Triggers
- ONLY DO AFTER ALL ROOM DATA IS CONVERTED: Tools/Convert/Triggers/Convert All Trigger Assets.
- AFTER ALL TRIGGERS CONVERTED CHECK ALL OF THEM to ensure subsequents are actually the class that they are, as there has been an instance where a dialogue subsequent had it's type name correct but it's class was converted to a move player subsequent.
- If you find this to be the case and need help contact Sweden for support.

## Convert Custom Dialogue Events
- Tools/Convert/Replace Asset Text Editor
- The lookfor and replacewith fields should already have what's necessary: 
"ns: , asm: Assembly-CSharp" & "ns: DREditor.CustomEvents, asm: DREditor.CustomEvents"
- So just click Convert.

## Convert Dialogue Events
- Tools/Convert/Replace Asset Text Editor
- Click Update Reworked Dialogue Events.

## Convert Animation Events
- Tools/Convert/Replace Asset Text Editor
- Look For: class: AnimationEvent, ns: , asm: DREditor
- Replace With: class: AnimationEvent, ns: DREditor.Dialogues.Events, asm: DREditor

## Additional Notices
### Notice that Dialogue Events in SaveableStateData Assets may be broken, and while this is highly unlikely, if you find this to be the case please contact Sweden for additional support.

### There is a possibility that All Flash screen White events and original ActorFade/line.Leave bools will be removed and need to be re-applied manually. If you learn this to be the case please contact sweden for support.