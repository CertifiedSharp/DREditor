# Art Asset Guide
To start, it is highly recommended that **all assets on your project have their dimension sizes divisible by 4.** This is to prevent bloat in your project in the long run, as unity has a built in compressor that needs asset dimensions divisible by 4.

## CG's and Fullscreen Assets
CG's and Fullscreen assets should be 1920x1080, or whatever the default resolution you plan to have for your project. It is highly recommended though to stay within the 1920x1080 rule though.

## Character Sprites
It is HIGHLY recommended that your character sprites be dimensions that are 2048x2048, 1024x1024, 512x512, etc. Note that it must stay consistent throughout ALL characters. Otherwise you will have to do much more extra work. 

The reason is so characters are scaled relative to each other, so that their heights in-game match their actual heights. 
**Therefore, it is highly recommended to create a height chart so that artists know how to size the characters proportionately to each other.**

As an example, here is one of the tallest and one of the shortest characters in Project: Eden's Garden, Wolfgang and Toshiko.


<img src="https://files.catbox.moe/r22vev.png" width="25%" alt="wolfgang">
<img src="https://files.catbox.moe/j7q5zx.png" width="25%" alt="toshiko">



Notice how there's a bunch of empty space around Toshiko? This is so that when we paste both characters on the same canvas, their heights in the game will always be scaled correctly to each other, no matter what. 

Otherwise, character sprites should be good to go!