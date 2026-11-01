//Author: Happy30
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Dialogues
{
    [System.Serializable]
    public class Line : LineBase
    {
        public string translationKey;
        public string TranslationKey { get => translationKey; set => translationKey = value; }

        [TextArea]
        [FormerlySerializedAs("Text")]
        public string text;
        public string Text { get => text; set => text = value; }


#if FMOD_UNITY
        [FMODUnity.EventRef]
        [FormerlySerializedAs("VoiceSFX")]
        public string voiceSFX;
        public string VoiceSFX { get => voiceSFX; set => voiceSFX = value; }

        [FormerlySerializedAs("SFX")]
        [FMODUnity.EventRef] 
        public List<string> SFXs = new();

        [FMODUnity.EventRef]
        [FormerlySerializedAs("EnvSFX")]
        public string envSFX;
        public string EnvSFX { get => envSFX; set => envSFX = value; }
#else

        [FormerlySerializedAs("VoiceSFX")]
        public AudioClip voiceSFX;
        public AudioClip VoiceSFX { get => voiceSFX; set => voiceSFX = value; }

        [FormerlySerializedAs("SFX")]
        public List<AudioClip> SFXs = new();

        [FormerlySerializedAs("EnvSFX")]
        public AudioClip envSFX;
        public AudioClip EnvSFX { get => envSFX; set => envSFX = value; }
#endif

        public List<SceneEvent> Events = new();

        [FormerlySerializedAs("TimeToNextLine")]
        public float timeToNextLine;
        public float TimeToNextLine { get => timeToNextLine; set => timeToNextLine = value; }

        [FormerlySerializedAs("AutomaticLine")]
        public bool automaticLine;
        public bool AutomaticLine { get => automaticLine; set => automaticLine = value; }

        [FormerlySerializedAs("AliasNumber")]
        public int aliasNumber; // additions
        public int AliasNumber { get => aliasNumber; set => aliasNumber = value; }

        [SerializeReference]
        public List<IDialogueEvent> DiaEvents = new();

        [FormerlySerializedAs("MusicChange")]
        public bool musicChange;
        public bool MusicChange { get => musicChange; set => musicChange = value; }

#if FMOD_UNITY
        [FMODUnity.EventRef]
        [FormerlySerializedAs("Music")]
        public string music;
        public string Music { get => music; set => music = value; }
#else

        [FormerlySerializedAs("Music")]
        public AudioClip music;
        public AudioClip Music { get => music; set => music = value; }
#endif
        [FormerlySerializedAs("ShowOptions")]
        public bool showOptions; // For toggling list of Visual effects below
        public bool ShowOptions { get => showOptions; set => showOptions = value; }

        [FormerlySerializedAs("DontPan")]
        public bool dontPan; // Don't change focus
        public bool DontPan { get => dontPan; set => dontPan = value; }

        [FormerlySerializedAs("StopEnv")]
        public bool stopEnv;
        public bool StopEnv { get => stopEnv; set => stopEnv = value; }

        [FormerlySerializedAs("StopSFX")]
        public bool stopSFX;
        public bool StopSFX { get => stopSFX; set => stopSFX = value; }

        [FormerlySerializedAs("PanToChar")]
        public bool panToChar;
        public bool PanToChar { get => panToChar; set => panToChar = value; }

        [FormerlySerializedAs("CharToPanNum")]
        public int charToPanNum;
        public int CharToPanNum { get => charToPanNum; set => charToPanNum = value; }

        [FormerlySerializedAs("CharToPan")]
        public Character charToPan;
        public Character CharToPan { get => charToPan; set => charToPan = value; }
        
        
        public Line() { }
        public Line(Character Speaker, int SpeakerNumber) : base(Speaker, SpeakerNumber) { }
        public Line(string Text)
        {
            this.Text = Text;
        }

        public Line(Character Speaker, int SpeakerNumber, string Text) : base(Speaker, SpeakerNumber)
        {
            this.Text = Text;
        }

        public Line(Character Speaker, int SpeakerNumber, Expression Expression, int ExpressionNumber) :
            base(Speaker, SpeakerNumber, Expression, ExpressionNumber)
        { }

        public Line(Character Speaker, int SpeakerNumber, Expression Expression, int ExpressionNumber, string Text) :
            this(Speaker, SpeakerNumber, Expression, ExpressionNumber)
        {
            this.Text = Text;
        }

        public void SetText(string Text) => this.Text = Text;

#if UNITY_EDITOR
        [SerializeField] protected bool showDiaEvents;

        public bool ShowDiaEvents { get => showDiaEvents; set => showDiaEvents = value; }
#endif

#if FMOD_UNITY
        public virtual void SetVoice(string VoiceSFX) => this.VoiceSFX = VoiceSFX;

        public virtual void SetMusic(string Music)
        {
            if (!string.IsNullOrEmpty(Music))
                MusicChange = true;

            this.Music = Music;
        }

        public virtual void SetEnv(string EnvSFX) => this.EnvSFX = EnvSFX;

#else
        public virtual void SetVoice(AudioClip VoiceSFX) => this.VoiceSFX = VoiceSFX;

        public virtual void SetMusic(AudioClip Music)
        {
            if (Music != null)
                MusicChange = true;

            this.Music = Music;
        }

        public virtual void SetEnv(AudioClip EnvSFX) => this.EnvSFX = EnvSFX;
#endif

        public void SetAliasNumber(int AliasNumber) => this.AliasNumber = AliasNumber;
        public void SetDiaEvents(List<IDialogueEvent> DiaEvents) => this.DiaEvents = DiaEvents;

        public virtual bool ContainsEventType<T>() => DiaEvents.OfType<T>().Any();

        public virtual bool HasEmptySceneEvents()
        {
            if (Events.Count > 0)
                foreach (var sceneEvent in Events)
                    if (sceneEvent == null)
                        return true;

            return false;
        }
    }
}