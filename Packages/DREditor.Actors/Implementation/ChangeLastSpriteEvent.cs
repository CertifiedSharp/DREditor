//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using DREditor.Talkables;
using DREditor.Characters;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class ChangeLastSpriteEvent : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected WorldSpriteChanger spriteChanger = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void OnEnable() => DialogueEventSystem.StartListening("ChangeLast", ChangeLastEvent);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void OnDisable() => DialogueEventSystem.StopListening("ChangeLast", ChangeLastEvent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ChangeLastEvent(object o = null)
        {
            if (isLoadingFile.Value)
                return;

            LastActorSprite laEvent = (LastActorSprite)o;

            if (laEvent.spriteNum == 0)
            {
                UnityEngine.Debug.Log("NOTICE: Last Actor Sprite Event: Detected you changed it to \"No Sprite\" ");

                return;
            }

            ChangeLastActorSprite(laEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="laEvent"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void ChangeLastActorSprite(LastActorSprite laEvent)
        {
            Character character = characterDatabase.Characters[laEvent.charNum];

            Talkable actor = character != null ?
                ServiceLocator.GetService<IObjectPooler<Talkable>>().GetObjectByName(character.FirstName)
                :
                null;

            Material expressionMaterial = character.Expressions[laEvent.spriteNum - 1].Sprite;

            Sprite expressionSprite = character.Sprites[laEvent.spriteNum - 1].Sprite;

            spriteChanger.WorldSpriteChange(actor, expressionMaterial, expressionSprite);
        }
    }
}