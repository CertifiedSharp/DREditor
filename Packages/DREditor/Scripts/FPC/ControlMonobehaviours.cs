//Author: Happy30
using DREditor.Cameras;
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FPC
{

    public class ControlMonobehaviours : MonoBehaviour
    {
        [SerializeField] bool DebugOn = true;

        public BoolWithEvent InMenu;

        public BoolWithEvent InDialogue;

        public BoolWithEvent InTPFD;

        public BoolWithEvent InLoading;

        public bool setting;

        public bool tpfdSetting;

        public bool disable;

        public List<MonoBehaviour> MonoBehaviours = new();

        public MonoBehaviour RaycastScript;

        public MonoBehaviour TPFDScript;

        public MonoBehaviour MovePlayer;
        

        private void Start()
        {
            if (!TPFDScript)
                TPFDScript = FindAnyObjectByType<PnCCamera>();
            //TO-DO: This functionality is pretty dependent on PnCCamera to know when to update mono's
            // I'd like it if we could remove this dependency if possible.
        }

        void Update()
        {
            SetDisable(InMenu.Value || InDialogue.Value);

            if (!InLoading.Value && 
                (disable != setting || tpfdSetting != InTPFD.Value || 
                (TPFDScript.enabled != InTPFD.Value && !InDialogue.Value && !InMenu.Value)))
            {
                if (!InTPFD.Value)
                    ThreeDMode();
                else
                    TPFDMode();

                SetSetting(disable);

                tpfdSetting = InTPFD.Value;
            }
        }

        void ThreeDMode()
        {
            EnableBehaviours(!disable);

            if (TPFDScript && TPFDScript.enabled)
                TPFDScript.enabled = false;

            if (DebugOn)
            {
                UnityEngine.Debug.Log("Game should be in Three D mode.");

                UnityEngine.Debug.Log("TPFD Value is: " + InTPFD.Value);
            }

            SetSetting(disable);
        }

        void TPFDMode()
        {
            EnableBehaviours(false);

            if (DebugOn)
                UnityEngine.Debug.Log("" + gameObject.name + " TPFD Value is: " + InTPFD.Value);

            EnableTPFD(!disable);

            if (RaycastScript)
                RaycastScript.enabled = !disable;

            if (MovePlayer)
                MovePlayer.enabled = false;
        }

        public void EnableBehaviours(bool to)
        {
            for (int i = 0; i < MonoBehaviours.Count; i++)
                if (MonoBehaviours[i] != null)
                    MonoBehaviours[i].enabled = to;
        }

        public void EnableTPFD(bool to)
        {
            if (TPFDScript)
                TPFDScript.enabled = to;
        }

        public void SetRaycastScript(MonoBehaviour RaycastScript) => this.RaycastScript = RaycastScript;

        public void SetDisable(bool to) => disable = to;

        public void SetSetting(bool to) => setting = to;
    }
}