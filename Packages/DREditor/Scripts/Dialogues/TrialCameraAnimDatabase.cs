//Author: KHeartz
//Trial Camera Animation Database for DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Dialogues
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Trials/Camera Anim Database", fileName = "CameraAnimDatabase")]
    public class TrialCameraAnimDatabase : ScriptableObject
    {
        public RuntimeAnimatorController controller;
        public List<AnimationClip> anims = new();
        public List<string> animNames = new();
        public List<bool> animValues = new();
        public List<string> GetNames()
        {
            var names = new List<string>();
            animNames.ForEach(anim => names.Add(anim));
            return names;
        }


        public virtual void ClearAll()
        {
            anims.Clear();

            animNames.Clear();

            animValues.Clear();
        }

        public virtual void Add()
        {
            anims.Add(new AnimationClip());

            animNames.Add("");

            animValues.Add(false);
        }

        public virtual void Add(AnimationClip clip)
        {
            anims.Add(clip);

            animNames.Add("");

            animValues.Add(false);
        }


        public virtual void SetController(RuntimeAnimatorController controller) => this.controller = controller;
    }
}