DREditor.CamerasPostProcess

// Start this from
// >Creating a new Empty Game Object, add the post process profile to it.
// >Add this script

// >For best use, bring the PlayerSystem prefab into the scene with this game object
// >In the context menu call the "Scan" Command, this'll load the cameras default states to be edited.

// >Manipulate the values based off of the scanned default settings,
// >> in P:EG, We used the Context Menus Enable All Post Processing Command,
// >> Set the Cameras to:
// >> Blur Camera Should be Untouched
// >> Dialogue Camera Should remove the Default layer and add the Post Processing layer
// >> For the rest of the cameras just add the post processing layer.

// >At this point if this is just for a Scenes permanent Post Processing you can save your scene and your done.
// >If you're changing currently existing versions, of the scene over time:
// >> Add the spawnable component to this scripts GameObject
// >> Turn this object into a prefab
// >> Save the Room Save with this and It'll load properly on Load, or you can even spawn it from dialogue!

// >> For each different instance of this script you make, you need to add it to PostProcessingHSVSpriteHandlers event strings.