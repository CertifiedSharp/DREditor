# Coding Guidelines
```ALERT: Note that maintainers reserve the right to deny any request regardless of these rules being followed.```

```ALERT: Please do not submit hundreds of requests for "optimization" reasons unless you can present tangible evidence of an impact the project maintainers deem reasonable. Do not request changes just because it's "technically faster".```

```REPORT: Requests for changes that involve the unity addressables require maintainer level permission at minimum but will likely be denied. Reason being that it would be asinine to expect non programmers to keep track of and attempt to understand the addressables system.```

In order to maintain quality and to enforce easy readability for both programmers and non programmers, all non-third party code files must adhere to these rules. If you attempt to contribute code not following these rules, your request will be denied. 

- _"Programmers should not want to blow their head off reading the code"_. 
- _"Someone who does not know how to code, should be able to reasonably piece together what your code does"_. 
- Code must be in the proper namespace based on where it's intended to be located.
- Everything that can, must have XML summaries. (they don't need to be filled out though, the community can do that if they wish)
- All fields visible in the inspector must have tooltips. (they must match the XML summaries)
- The xml /// <inheritdoc/> tag must be used on things like overridden functions and interface implementations. You do not have to follow this rule if you write out the xml summary yourself instead.
- If your script is a monobehaviour that is not abstract, you must use a ClassHeader attribute and use the nameof keyword to list out what the script inherits.
- Code must do their best to follow [**Uncle Bobs Guide to Clean Code.**](https://www.youtube.com/watch?v=7EmboKQH8lM&list=PLmmYSbUCWJ4x1GO839azG_BBw8rkh-zOj)
- Code must do their best to follow [**NDepend Rules.**](https://www.ndepend.com/default-rules/NDepend-Rules-Explorer.html) However, there are some [**caveats.**](https://dreditor.net/docs/ndependchanges.html)
- Logs must do their best to follow [**Log Designations.**](https://dreditor.net/docs/logdesignations.html)
- Author your code at the top with //Author: how you want to be credited
- You must separate lines of code by a new line, 2 lines to separate between fields and functions. This is also done to separate functions implemented from interfaces.
- Unless a valid reason is given, all properties and functions should be virtual with protected or public. This is to allow easier extensions.

An example of a valid script can be seen below.

```cs
//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.FreeTimeSurfacing;
using DREditor.ItemRolling;
using DREditor.Processables;
using DREditor.Utility;
using UnityEngine;

namespace DREditor.GachaSurface
{
    /// <summary>
    /// Summary of the class
    /// </summary>
    [ClassHeader(nameof(SubmittableFTESurfaceCollection) + ", " + nameof(SubmittableFTESurfaceComponent) + ", " + 
        nameof(FTESurfaceComponent) + ", " + nameof(ISubmittable) + ", " + nameof(IComponentsCache) + ", " + 
        nameof(IRollerContainer) +
        "\n\nText Matching XML")]
    public class Gacha : SubmittableFTESurfaceCollection, IRollerContainer
    {
        /// <summary>
        /// Summary of what the field is/what it's for etc.
        /// </summary>
        [Header(nameof(Gacha) + " Fields")]
        [Divider]
        [Tooltip("Text Matching XML")]
        [SerializeField] protected ItemRollerBase roller;


        /// <inheritdoc/>
        public virtual IItemRoller GetRoller() => roller;


        /// <inheritdoc/>
        protected override void OnSubmit()
        {
            RollItem();

            ProcessRollResult();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void RollItem() => roller.RollForItem();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ProcessRollResult()
        {
            if (RollSucceded())
                OnRollSucceded();
            else
                OnRollFailed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool RollSucceded() => roller.TryGetRolledObject(out object _);

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnRollSucceded()
        {
            this.InvokeThisPlusComponents<IItemRollerResponse>((response) => response?.InitializeForResponse(roller));

            base.OnSubmit();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnRollFailed()
        {
            AlertRollFailed();

            base.InvokeCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void AlertRollFailed() =>
            UnityEngine.Debug.LogWarning($"ALERT: The item roller failed in rolling an item! Forcing process as Complete.");

        /// <summary>
        /// Tells the roller to give the rolled item before completing the gacha process.
        /// </summary>
        protected override void InvokeCompleted()
        {
            roller.GivePlayerRolledItem();

            base.InvokeCompleted();
        }
    }
}
```