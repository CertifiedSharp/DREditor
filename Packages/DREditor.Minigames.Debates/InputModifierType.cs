//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Minigames/Debate/Input Modifier Type")]
    public class InputModifierType : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool counterable = false;
    }


    /* An aspect and what it edits.
     * 
     * 
     * 
     * We wouldn't be able to have a Hit Text color modifier because the color wouldn't change within the editor, 
     * but would maybe change in something like a settings asset for each type.
     * 
     * An attribute to signify that something is read only in a debate editor, but editable in a phrase modifier preset?
     * 
     * These would only be available if the phrase is interactable.
     * 
     * An attribute for only allowing one of that modifier.
     * 
     * EditableOnPreset Attibute + Gradient modifier.
     * 
     * Silencable Visual modifier: Hitable Text color
     * Hitable Visual modifier: Hitable Text color
     * Consent Visual modifier: Hitable Text color 
     * 
     * 
     * 
     * Phrase Modifier Presets via scriptable object, where scriptable object contains a single Phrase Modifier
     * 
     * Interaction Evaluation/Answer Modifiers:
     * Bullet Answer: Uses truth bullet.
     * Stateable Answer: Contains bool with event implying that the state must be true in order for this to be considered a valid answer.
     * Lie Answer(Preset): Bullet Answer, Stateable
     * Pathos Answer: Stateable, Contains the text of the pathos white noise or data pertaining to pathos answer.
     * 
     * Partly Hitable: Modification of phrase text within the panel, modification of Interactable box.
     * 
     * 
     * 
     * 
     *                               
     * Input Modifier: YES : Create Ready :
     * A scriptable object applied to the phrase and the manager for that interaction only recognizes phrases with that modifier.
     * Single Modifier class that uses a database of scriptable objects which are the interaction scriptable objects.
     * right click (hit),
     * left click (silence),
     * absorb (pathos),
     * 
     * 
     * Phrase Presets: YES
     * Interactable: EditableOnPreset gradient modifier, addition of hitable box.
     * Hitable: Interactable, Input Modifier for right click, must contain FU dialogue.
     * Consentable: Hitable, except the Text Color.
     * Silencable: Interactable, Input Modifier for left click. custom gradient modifier.
     * Absorbable: Interactable, Input Modifier for Absorb.
     * Pathosable: Silencable, Absorbable, custom gradient modifier.
     * 
     * Phrase(Preset): PanelText Modifier.
     * White Noise(Preset): NonPanelText Modifier, Silencable, The editor for it's phrase text is different.
     * Pathos White Noise(Preset): White Noise, Absorbable, (may contain additional data)
     * 
     * 
     * 
     * Hit Managers: MAYBE
     * Pathos: Counter, NSD UI Change, Changes truth bullet by using what was absorbed.
     * Lie: Counter, NSD UI Change, Changes truth bullet by inverting it (truth bullet modifiers?)
     * 
     * Evaluation Objects: MAYBE
     * An evaluation object would be a scriptable object, that holds 2 phrase instances, one for matching and one for
     * exclusion, which you'd fill out, and then on an evaluator monobehaviour, you would just assign that scriptable object.
     * If the sent phrase contains the modifiers and doesn't contain the exclusion modifiers, evaluates to true.
     */
}