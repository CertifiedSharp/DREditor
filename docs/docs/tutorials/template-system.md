# Template System
In order to make things simpler for the user, DREditor has many templates, however not all of them are completely filled out. There are tons of templates that need context for your specific project in order to for those systems to function. 

Therefore, you'll notice that a lot of names of game objects will say "Template" or "ETemplate" or "Shell" or Neither. Each of these symbolizes what steps need to be taken in order for that template to be usable or for it to function properly. In order to use these templates to our advantage. **It is essential that anything with a template suffix should be turned into a prefab variant where you replace the "Template" or "ETemplate" part of the name with the acronym of your project. Detailed below.** This is done for multiple reasons, one being organizational purposes, the second being so that the templates in your imported samples folder can be updated and/or replaced. 

As an example, if a new DREditor Version drops and there's mention of a template change, after updating your project through DREditor/Dependencies/Update Package Wizard you can go to the package manager and click on the package that was updated and there should be a Samples section you can click on, and there should be a button that says "Import" or "Re-Import". 
**This will replace your Templates in your Samples folder, therefore it is a rule of thumb in DREditor to never edit a template otherwise when you reimport your samples , your work may be overwritten! This also applies to animations on the templates!**

## No Suffix
If a prefab in DREditor does not have any suffix of "Template" or "ETemplate" then it is usually a system that either does not need setting up, or is already filled out for you by default. That said, if you wish to change anything on these, you should make a Project Prefab Variant of it so that in the future data won't get overwritten. 

## ETemplate
Short for "Editable Template", these are templates that just need to be filled out. Simply check the fields on the scripts on the game object that are empty and fill them in, whatever they may be. A majority of the time, it's databases or simple things that may not even need a tutorial for. 

## Template
There are likely more Templates or ETemplates inside of this template. They are likely a larger system broken up into smaller systems. Where in order to fully "fill out" the template, all of the other templates inside of the template must be filled out or wired to each other _first_ for the template to be considered "filled". Depending on the circumstance, you may need to unpack a template into the shell, _then_ make it into a Project Prefab Variant in order to use it. (See Below)

## Shells
You'll notice that Templates are Unity Prefab Variants already. Meaning there's an underlying prefab that was built off in order to make the Template. This prefab is referred to as the "Shell".  The shells contain very little, like the hollowed out state of whatever process it holds. This tends to be scripts the template uses that we know will never change.

## Making A Project Prefab Variant
A "Project Prefab Variant" is simply the name given to a Unity Prefab Variant of a DREditor Template for your specific project. Project prefabs are commonly suffixed with the acronym of the project.

- Right click on a template in the hierarchy.
- DREditor/Create Project Prefab Variant, A wizard will pop up. 
- The only field you need to fill out is the Suffix. Simply type the acronym for your project.
   - Example 1: Project: Eden's Garden => EG  or PJEG
   - Example 2: Kill/Cure: Beautiful Vice => KC or KCBV
   - Example 3: The Uzujigoku Massacre => TUM
- If you like your acronym, and you know you won't be changing it in the future. You can check the "Permanently Use Settings" check box and from now on you won't have to type your acronym every single time you want to create a project prefab variant.
- If for what ever reason you don't want to auto set this, you can just click "Create".
- At this point the system will have updated the prefab in the hierarchy with your new project prefab variant. 
- When you left click on the object in the hierarchy, in the inspector window to the right, you should see 2 buttons "Select" and "Open". 
- You can click on "Open" and from there you can start working on your prefab and applying changes safely. 

## Unpacking a Template for the Shell.
Sometimes, when trying to edit a template, unity may stop you, saying that you can't mess with part of a prefab structure (See below image). This usually means you'll have to unpack a template into it's shell _then_ make your Project Prefab Variant so you can edit what you need to.
To do this you can Right Click: 

- Prefab/Unpack.  **Do NOT press "Unpack Completely!**
- Then, right click again and follow the steps to make your Project Prefab Variant. 
- Now you should be able to open your Project Prefab Variant, and make the edits you want. 

<img src="https://files.catbox.moe/opnqtr.png" width="25%" alt="prefabinstance">

## Templates with Animations
If you're wanting to use an animation present on the template and edit it. You can do this **if you've already made your prefab variant of the template for your project.** 

- Right click on the game object containing the animator component and go to DREditor/Duplicate Insert Animation. 
- This will let you choose an output folder to put your animation, be sure to keep things as organized as you can!
- Once you've selected your folder, it should duplicate the animation for you to edit it as you see fit, and it should automatically plug in the newly duplicated animator.