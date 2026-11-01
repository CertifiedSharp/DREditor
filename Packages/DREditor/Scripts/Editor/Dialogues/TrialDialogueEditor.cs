//Original Author: SeleniumSoul
//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    using Debug = UnityEngine.Debug;
    [CustomEditor(typeof(TrialDialogue))]
    public class TrialDialogueEditor : DialogueEditorBase<TrialLine>
    {
        protected readonly TCOEditor tcoEditor = new();

        protected readonly TrialDialogueDebugToolEditor<TrialLine> debugTool = new();

        protected readonly TrialLineEditor<TrialLine> lineEditor = new();

        protected readonly EndEventEditor<TrialLine> endEventEditor = new();


        protected override LineEditorBase LineEditor { get => lineEditor; }

        protected override DialogueDebugToolEditorBase DebugTool { get => debugTool; }


		public virtual void OnEnable()
        {
            SetDialogue(target as TrialDialogue);

            lineEditor.Initialize((TrialDialogue)dia, serializedObject, this);
        }

        protected override void EditPanels()
        {
            base.EditPanels();

            if (LineEditor.TestLine == -1 && dia is ITrialSequenceSkippable trialDialogue)
                trialDialogue.Skip = HandyFields.IntField("Skip:", trialDialogue.Skip, 20);
        }

        protected override void DisplayFooter()
        {
            endEventEditor.DisplayEndEvents((TrialDialogue)dia);

            base.DisplayFooter();

            endEventEditor.DisplayEndEventsButtons();

            GUILayout.Space(30);
        }
    }
    [Serializable]
    public class TrialLineEditor<T> : LineEditor<T> where T : TrialLine, new()
    {
        protected readonly float DropdownArrowPadding = 25f;

        protected DRTrialCamera cam;

        protected TrialCameraAnimDatabase cameraAnimDatabase;

        protected readonly TCOEditor tcoEditor = new();

        protected readonly bool displayText = true;

        public TrialLineEditor() { }

        public TrialLineEditor(bool displayText) => this.displayText = displayText;

        protected override void DisplayLowerLine()
        {
            CustomLayoutUtility.DisplayHorizontal(AnimButton, EditCamAnim);

            EditTCO();

            base.DisplayLowerLine();
        }

        protected void EditCamAnim()
        {
            if (cameraAnimDatabase == null && !DatabaseUtility
                .InitializeDatabase(cameraAnimDatabase, out cameraAnimDatabase))
                return;

            HandyFields.Label("Cam:");

            string[] animNames = cameraAnimDatabase.GetNames().ToArray();

            Lines[i].CamAnimIdx = EditorGUILayout.IntPopup(Lines[i].CamAnimIdx,
                cameraAnimDatabase.GetNames().ToArray(), ContainerUtil.Iota(cameraAnimDatabase.anims.Count),
                HandyFields.CalculateWidthOption(animNames[Lines[i].CamAnimIdx], DropdownArrowPadding));
        }

        protected virtual void AnimButton()
        {
            if (GUILayout.Button("Anim", GUILayout.Width(50)))
            {
                if (!cam)
                    cam = UnityEngine.Object.FindAnyObjectByType<DRTrialCamera>();
                if (cam != null)
                {
                    TestCameraAnim(Lines[i], false);
                }
                else
                    Debug.Log("Unsuccessful");
            }
        }

        protected void TestCameraAnim(TrialLine line, bool gameTest)
        {
            if (!Application.isPlaying)
            {
                EditorUtility.DisplayDialog("Must be in Play Mode",
                    "You must be in play mode with a DRTrialCamera to test lines!", "Ok");
                return;
            }
            if (gameTest)
            {
                //cam.InvokeTestLine(line);
                DialogueCore.InvokeDebugLine(dia, line);
            }
            else
            {
                if (!line.Co.enabled && line.Speaker.TrialPosition < 16)
                    cam.SeatFocus = line.Speaker.TrialPosition;

                if (line.Co.enabled)
                    cam.ApplyOverrides(line.Co.height, line.Co.distance, line.Co.seatFocus, line.DontPan);

                List<string> names = cameraAnimDatabase.GetNames();

                cam.TriggerAnim(names[line.CamAnimIdx]);
            }
        }


        protected virtual void EditTCO()
        {
            if (Lines[i] is not ITrialLine cameraLine)
                return;

            using (new EditorGUILayout.VerticalScope())
            {
                if (cameraLine.Co == null)
                    return;

                if (cameraLine.Co.enabled)
                    tcoEditor.EditTCO(Lines[i].Speaker, EvaluateDontPan(Lines[i]), cameraLine.Co);
                else if (HandyFields.Button("Add Camera Override"))
                {
                    cameraLine.Co.SetEnabled(true);

                    cameraLine.Co.SetDistance(8.11f);

                    if (Lines[i].Speaker != null)
                    {
                        cameraLine.Co.SetSeat(Lines[i].Speaker.TrialPosition);

                        cameraLine.Co.SetHeight(Lines[i].Speaker.TrialHeight);
                    }
                }
            }
        }

        protected virtual bool EvaluateDontPan(LineBase line) => line is Line castedLine && castedLine.DontPan;


        protected override bool SkipDisplayingText() => base.SkipDisplayingText() && !displayText;
    }

    [Serializable]
    public class TrialDialogueDebugToolEditor<T> : DialogueDebugToolEditor<T> where T : TrialLine, new()
	{
        protected readonly TCOEditor tcoEditor = new();
        protected TrialCameraAnimDatabase cameraAnimDatabase;

        public override bool DebugMode(DialogueCore dia) //*
        {
            base.DebugMode(dia);

            if (GUILayout.Button("Randomize Camera Anims"))
                CheckRandomizeCamAnim();

            DebugApplyTCOToChar();

            return exitDebugMode;
        }



        #region Apply TCO to all lines with this character
        protected bool applytcoDebug = false;
        protected TCO coDebug;
        protected Character debugSpeaker = null;
        protected int SpeakerNumber = 0;

        protected CharacterDatabase speakers = null;
        protected virtual void DebugApplyTCOToChar()
        {
            if (!applytcoDebug && GUILayout.Button("Apply TCO to All of a Character", GUILayout.Width(200)))
                applytcoDebug = true;

            if (applytcoDebug)
            {
                if (GUILayout.Button("Exit", GUILayout.MaxWidth(50)))
                    applytcoDebug = false;

                if (!DatabaseUtility.InitializeDatabase(speakers, out speakers))
                    return;

                var prependedArray = ContainerUtil.PrependedList(speakers.GetNames().ToArray(), "<No Character>");
                SpeakerNumber = EditorGUILayout.IntPopup(
                    SpeakerNumber, prependedArray, ContainerUtil.Iota(prependedArray.Length, -1), GUILayout.Width(130));
                debugSpeaker = SpeakerNumber == -1 ? null : speakers.Characters[SpeakerNumber];
                coDebug ??= new TCO();
                coDebug.SetEnabled(true);
                tcoEditor.EditTCO(null, false, coDebug);
                if (GUILayout.Button("Apply", GUILayout.MaxWidth(50)))
                    DebugSetTCOToChar();
            }
        }
        protected virtual void DebugSetTCOToChar()
        {
            foreach (TrialLine l in dia.GetLines<T>())
            {
                if (l.Speaker == debugSpeaker)
                {
                    l.Co = coDebug;
                }
            }

            ExitDebugMode();
        }
        #endregion

        protected virtual void CheckRandomizeCamAnim()
        {
            if (cameraAnimDatabase == null && !DatabaseUtility
                .InitializeDatabase(cameraAnimDatabase, out cameraAnimDatabase))
                return;

            if (EditorUtility.DisplayDialog("Randomize Cam Anims?",
                        "WARNING!: This will overwrite ALL Camera Animations on every line in the asset! " +
                        "Are you sure you want to continue?", "Yes", "No"))
            {
                RandomizeCamAnims();

                ExitDebugMode();
            }

        }

        protected virtual void RandomizeCamAnims()
        {
            for (int i = 0; i < dia.LineCount; i++)
                dia.GetLines<T>()[i].CamAnimIdx = UnityEngine.Random.Range(0, cameraAnimDatabase.anims.Count);
        }
    }

    #region Trial Camera Overrides
    public class TCOEditor
    {
        readonly float tcoMinWidth = 200;

        TCODatabase presetbase;

        int presetNum = 0;

        string presetName;
        TCO o;
        bool dontPan;
        Character speaker;
        public void EditTCO(Character speaker, bool dontPan, TCO o)
        {
            this.o = o;

            this.dontPan = dontPan;

            this.speaker = speaker;

            RemoveTCOButton();

            using (new EditorGUILayout.HorizontalScope())
            {
                DisplayTCO();

                DisplayTCOPresets();
            }
        }


        protected virtual void RemoveTCOButton()
        {
            if (GUILayout.Button("X", GUILayout.Width(20)))
                o.Clear();
        }

        protected virtual void DisplayTCO()
        {
            using (new GUILayout.VerticalScope("Box", GUILayout.MinWidth(120)))
            {
                using (new EditorGUILayout.HorizontalScope(GUILayout.MinWidth(tcoMinWidth)))
                {
                    if (speaker == null || !dontPan)
                    {
                        o.SetSeat(HandyFields.FloatField("SeatFocus: ", o.seatFocus, 30, 7));
                        if (speaker != null && GUILayout.Button("Use Char", GUILayout.Width(100)))
                        {
                            o.SetSeat(speaker.TrialPosition);
                        }
                    }
                    else if (o.seatFocus != 0)
                        o.SetSeat(0);
                }
                using (new EditorGUILayout.HorizontalScope(GUILayout.MinWidth(tcoMinWidth)))
                {
                    o.SetDistance(HandyFields.FloatField("Distance: ", o.distance, 30));
                    if (GUILayout.Button("Use Default", GUILayout.Width(100)))
                    {
                        o.SetDistance(8.11f);
                    }
                }
                using (new EditorGUILayout.HorizontalScope(GUILayout.MinWidth(tcoMinWidth)))
                {
                    o.SetHeight(HandyFields.FloatField("Height: ", o.height, 30));
                    if (speaker != null && GUILayout.Button("Use Char", GUILayout.Width(100)))
                    {
                        o.SetHeight(speaker.TrialHeight);
                    }
                }
            }
        }

        protected virtual void DisplayTCOPresets()
        {
            using (new GUILayout.VerticalScope("Box"))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Preset Base:", GUILayout.MaxWidth(100));

                    presetbase = HandyFields.UnityField(presetbase, 170, 20);
                }

                if (presetbase != null)
                {
                    if (presetbase.Presets.Count != 0)
                    {
                        presetNum = EditorGUILayout.IntPopup(presetNum, presetbase.GetNames(), presetbase.GetInts());
                        if (GUILayout.Button("Apply"))
                        {
                            TCO preset = presetbase.Presets[presetNum].Preset;
                            o.SetSeat(preset.seatFocus);
                            o.SetDistance(preset.distance);
                            o.SetHeight(preset.height);
                        }
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("New Preset Name:", GUILayout.MaxWidth(125));
                        presetName = EditorGUILayout.TextField(presetName);
                    }

                    if (GUILayout.Button("Create New Preset", GUILayout.MaxWidth(150)))
                    {
                        presetbase.Presets.Add(new TCOPreset(presetName, new TCO(o.seatFocus, o.height, o.distance)));
                    }
                }
            }
        }
    }

#endregion
}
#endif