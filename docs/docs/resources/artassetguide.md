# Art Asset Guide
To start, it is highly recommended that **all assets on your project have their dimension sizes divisible by 4.** This is to prevent bloat in your project in the long run, as unity has a built in compressor that needs asset dimensions divisible by 4.

## CG's and Fullscreen Assets
CG's and Fullscreen assets should be 1920x1080, or whatever the default resolution you plan to have for your project. It is highly recommended though to stay within the 1920x1080 rule though.

## Videos
Videos are usually considered a full screen asset. Videos shouldn't usually have sound since they can usually synced by adding the audio to FMOD and just calling in normally through dialogue. 
Note that any audio that is left on will still play in the game, however because it is not passed through FMOD, the volume of the audio for the video cannot be changed so at the very least make sure it is not too loud.
It should also be noted that if you are wanting a video that contains transparency, it **must be exported as a .webm**. Additionally .webm also appears to be relatively optimized so webm's are highly suggested.

## Character Sprites
### Naming Convention
Sprite files should be named appropriately, the best convention being: CharacterFirstName_SpriteCode .
Examples of this would look like: Damon_1a or Damon_Happy .
This convention is followed for two reasons: Ensures no additional work needs to be done when writing dialogue syntax, and so that the auto character importer can properly set the sprite codes for the characters sprite.
Note that while following this convention is not _required_ , be warned that by not doing so you will have to update each sprite's sprite code manually on the character asset.
### Dimension Size
It is HIGHLY recommended that your character sprites be dimensions that are 2048x2048, 1024x1024, 512x512, etc. Note that it must stay consistent throughout ALL characters. Otherwise you will have to do much more extra work. 
The reason is so characters are scaled relative to each other, so that their heights in-game match their actual heights. 
**Therefore, it is highly recommended to create a height chart so that artists know how to size the characters proportionately to each other.**
As an example, here is one of the tallest and one of the shortest characters in Project: Eden's Garden, Wolfgang and Toshiko.


<img src="https://files.catbox.moe/r22vev.png" width="25%" alt="wolfgang">
<img src="https://files.catbox.moe/j7q5zx.png" width="25%" alt="toshiko">



Notice how there's a bunch of empty space around Toshiko? This is so that when we paste both characters on the same canvas, their heights in the game will always be scaled correctly to each other, no matter what. 

Otherwise, character sprites should be good to go!
### Replacing an Imported Sprite
- Make sure that the new sprite is named THE EXACT SAME AS THE TEMPORARY ONE.
- Find the sprite you want to replace in your project window. Right Click: Show in Explorer
- Click and drag your new sprite into the folder that pops up.
- Your computer will ask you if you want to replace the existing file, confirm this action.
- Once it's done replacing, you can close the explorer window and go back into unity, it'll recompile, and once it's done, you should see that your sprite has been updated without breaking anything!

## Nameplates
If you are using image nameplates for your characters, the dimensions size doesn't matter so long as:

- The dimension size is divisible by 4.
- ALL nameplates dimensions sizes are the same.

To make this easier on artists, it is suggested to **make the nameplate for the character with the longest name first**.
Whatever the size to the nearest number divisible by 4 is, _that_ will be the dimension size you use for all nameplates.
A suggested dimension size would be 592x84.