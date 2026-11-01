//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if FMOD_UNITY
using FMODUnity;
#else
using DREditor.EventObjects;
#endif
using System.Collections;
using UnityEngine;

namespace DREditor.FPC
{

    public class PlayerFootsteps : MonoBehaviour
    {
#if FMOD_UNITY
        public StudioEventEmitter AudioSource;
#else
		public AudioSource AudioSource;

        public AudioClip[] StepSounds;
        
        public BoolWithEvent Running;

        public float Volume;
#endif

        [SerializeField] protected float cooldownDuration = 0.1f;

        protected bool onCooldown;


        public void Step()
        {
            if (onCooldown) 
                return;

#if !FMOD_UNITY
            var sound = StepSounds[Random.Range(0, StepSounds.Length - 1)];

            var pitch = Random.Range(0.9f, 1.1f);

            AudioSource.pitch = pitch;

            AudioSource.clip = sound;

            AudioSource.volume = Running.Value ? Volume : Volume / 3f;
#endif
            AudioSource.Play();

            onCooldown = true;

            StartCoroutine(Cooldown());

        }

        protected virtual IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldownDuration);

            onCooldown = false;
        }
    }
}