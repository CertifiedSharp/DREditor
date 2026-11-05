# Disabled/Edited NDepend Issues
NDepend is an expensive piece of developer software I've used during DREditor development to identify architectural issues within the DREditor codebase, 
however, some of the issues the system noticed came with issues, either by not noticing that a problem was fixed or the rule being enforced to begin with or the restraints being reasonably modified. 
Below details which NDepend rules DREditor Violates and are ignored for it's development, and the reason behind the decision. 

## Turned OFF
**ND1500** - API Breaking Changes: Types.<br/>
Reason: This system is in it's infancy, API breaking changes are inevitable. 

**ND2102** - Avoid defining multiple types in a source file. <br/>
Reason: The Core DREditor is still unreworked and contains multiple types in a source file. 
So this rule was disabled to focus on more real and pressing issues in the codebase.

**ND1207** - Non-static classes should be instantiated or turned to static.<br/>
Reason: Wouldn't notice that classes WERE being used just through the editor either through runtime or editor runtime.

**ND2802** - Assemblies Referenced in Multiple Versions <br/>
Reason: Got this after upgrading the unity version to unity 6 and I couldn't seem to fix it.

**ND3120** - Software Composition Analysis (SCA) - Security<br/>
Reason: I updated to the correct newtonsoft json package it asked for on ndepend but it wouldn't update properly
after resolving the issue for some reason.

**ND3200** - Classes tagged with InitializeOnLoad should have a static constructor.<br/>
Reason: They do, I don't know why it's saying they don't. The process won't even work _until_ you even do it. So I'm removing it since clearly it won't understand that and it's enforced by VS/Unity anyway.

**ND3204** - Use CreateInstance to create a scriptable object.<br/>
Reason: This was logged for every single class that inherited a class that was a scriptable object. The rule isn't actually being violated. I can only assume it thinks it is because it's considering the class as POCO and when it initializes the base class, since it's a scriptable object it is then thinking it is violating the rule.


## EDITED
**ND1202** - Class Shouldnt Be Too Deep In Inheritance Tree <br/>
Edit: Removed the equals sign from the where baseClasses.Count() >= 4 query. <br/>
Reason: After investigating the wikipidea article it links for "Compisition over Inheritence" 
it literally says that it's not a hard rule or law of coding.
Plus considering that 4 has been the actual max across 1400+ scripts, I'd say we can let this one slide.