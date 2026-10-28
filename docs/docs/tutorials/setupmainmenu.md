# Setting up the Main Menu
## Bringing the Global Systems over from the Dialogue Scene
- Mass select from your player system to the Room\_Loading\_System. (All of your global systems)
- Right Click: Copy
- In your Project Window, open up your GYM_MainMenu
- Mass select from your player system to the Room\_Loading\_System. (All of your global systems)
- Right Click: Delete
- In the open space in the hierarchy window, Right Click: Paste
- All your global systems set up from your GYM_Dialogue Scene should now be present on your main menu scene.

## Setting up the Main Menu Template
- Left click Main\_Menu\_Template 
- Select the Menu Item: DREditor/UI/Colorize Hierarchy Groups.
- Now we'll be able to see the main menu a bit better. 
- In the hierarchy, Under Main\_Menu\_System\_Template
- Under Difficulty\_UI\_Canvas\_Template
- Select the Start\_Game\_Button\_DRE scroll down to the "Move To Scene" component.
- For the "Scene Name" write the name of the scene where your game starts. 
- Now when starting a new game our system will take us to the right place.
- Right Click Main\_Menu\_System\_Template: DREditor/Create Project Prefab Variant
- The Main Menu should now be usable for you to test starting a new game.

