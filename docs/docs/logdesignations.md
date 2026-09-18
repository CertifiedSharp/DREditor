# Log Designations
## Types
In DREditor, there are many logs that will be displayed to the console, this page gives context to some of the logs you'll see as you use the tool. Within the unity console, there are 3 types of logs: Logs, Warnings, and Errors. 
If any of these are logged by DREditor, the best way to regard each is as follows:

+ Logs: Simple information being reported, like when specific process like dialogue events are taking place etc. Most of this information can be easily dismissed, and just there to give context.
+ Warnings: Same as logs but if there's any information to keep track of, it'll likely be in a warning. Commonly used with ALERTS to the user.
+ Errors: These will only show up in 2 scenarios, the first being that the user has not plugged something in correctly or has not set something up properly, the second being that there is a bug in the engine, and it is almost never the second. 

## Designations
You'll notice that DREditor will categorize it's logs through specific keywords. This is based off of the PODs from the game Nier: Automata. PODs are floating droids that assist the characters of the game, giving these designations at every response. I found this to be quite charming, and considering DREditor is to act as an assistant to developers, I've decided to implement these designations. Below is a guide on what each designation means. These are all self explanitory, but just in case!

+ REPORT/ANALYSIS: Information simply being told to the user./Information 
+ ALERT/WARNING: Information that majority of the time requires user attention, or to notify the user that something under the hood has occurred. But the game will still continue to play regardless.
+ HYPOTHESIS: Sometimes the system will try to guess what user is trying to achieve.
+ PROPOSAL: Will give suggestions on what the user should do.