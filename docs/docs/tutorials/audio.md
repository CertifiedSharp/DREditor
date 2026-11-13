# Audio
For best optimization with audio, it's best to have your audio files be of type .ogg. However, this isn't _required_ but something to keep in mind is considering Trial Gameplay is commonly fully voiced, the storage and resources add up over time. It's usually best to nip this kind of issue early so that further down development, audio issues are less likely to arise.
It should also be noted that this is an extremely simplified way to use FMOD, there are likely better tutorials online, but this note is for people who will likely be audio engineers on your project. Everything on this page is all the information you'll need in order to use FMOD within your Unity Project.

## Patch the Studio Event Emitter
There's a minor bug in the current version of FMOD, but DREditor supplies a patch to the code for you. The tool can be applied from the menu item: DREditor/Dependencies/FMOD/Patch StudioEventEmitter

## Opening up FMOD Studio
- On Unity Hub, find your project and select the 3 dots on the right.
- Click Show in Explorer.
- Look for the folder named dre_audio and go into it. 
- Double click the dre_audio.fspro

## Importing Usable Audio to Unity through FMOD
In order to use our audio assets in unity through FMOD, we must follow these process so that it will appear in FMOD audio fields in unity. 

### Create an Audio Event
- Click and drag you audio asset into the FMOD project.
- It will ask you what kind of event you want. 90% of the time you'll want 2D Timeline. 3D Timeline would only really be used when environmental objects are emitting sound. Ex: A Generator's Hum getting quieter as you walk away from it. 
- Click Create
- Your event should now be made and you can now organizing it within the given folders based on what the audio will be used for. 

### Assign your Audio to a Bank
- Right Click your newly made FMOD event/Assign to bank/Browse/
- You will then pick a bank that best suits your audio. To my knowledge there is no right or wrong answer here, so just do your best. Once you select a bank the #unassigned tag on the event should disappear. 

### Assigning Events to Busses through the Mixer
- Now, on the top bar go to Window/Mixer
- The mixer window should appear, and you should see your events to the left, you will click and drag them into the groups that best describe them. Music, SFX, Dialogue. Whatever is under any of these 3 groups, will be affected by the audio setting for that audio type. Example: Putting Music in the Music Group, will have it's volume controlled by your games Music Setting in the options menu.
- You can now close out of the Mixer Window. 

### Save and Build to use Audio in Unity
- Ctrl + S to Save your project.
- Go to File/Build
- After your project finishes building. Go back into unity, an FMOD window will appear and start a count down. Do not do anything while this window is present, just let it do it's thing. 
- Once it finishes, your Audio events will appear in the FMOD Audio Fields around DREditor.
- You can also now fill in your Playlist/Jukebox to play music that will have it's title show up in the Music UI.

## Using FMOD Audio in Unity
- An FMOD field has a Magnifying glass to the right. 
- Clicking on it lets you search and select for your audio event to play. 

## Committing FMOD Changes to Github
You commit as you normally would. But what I want to note here is that there will be a lot of XML and weirdly named files being committed, that's just how FMOD is, so don't worry about those.

## Create a Loop Region (Looping Music)
- Select the FMOD event you wish to apply this to.
- On the FMOD Timeline for your event. Right click the blue audio.
- Add Loop Region
- You can now set the region for how music should loop after the first full playthrough.

## Make Music Fade out when Changing
- Select the FMOD event you wish to apply this to.
- Below the timeline, there is a vertical bar that says "Fader", it has a knob in the middle, right click it.
- Add Modulation/AHDSR
- A panel with 7 red dots along a red line will appear.
- Click and drag the 3rd red dot all the way to the left.
- Ctrl + S to save your project. 
- File/Build to build.