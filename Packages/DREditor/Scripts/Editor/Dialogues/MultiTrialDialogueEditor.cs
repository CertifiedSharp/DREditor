//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
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
    [CustomEditor(typeof(MultiTrialDialogue))]
    public class MultiTrialDialogueEditor : DialogueEditorBase<MultiTrialLine>
    {
        protected readonly MultiTrialDialogueDebugToolEditor debugTool = new();

        protected readonly MultiTrialLineEditor<MultiTrialLine> lineEditor = new();

        protected readonly EndEventEditor<MultiTrialLine> endEventEditor = new();


        protected override LineEditorBase LineEditor { get => lineEditor; }

        protected override DialogueDebugToolEditorBase DebugTool { get => debugTool; }


        public virtual void OnEnable()
        {
            SetDialogue(target as MultiTrialDialogue);

            lineEditor.Initialize(dia, serializedObject, this);
        }


        protected override void EditPanels()
        {
            base.EditPanels();

            if (LineEditor.TestLine == -1 && dia is ITrialSequenceSkippable trialDialogue)
                trialDialogue.Skip = HandyFields.IntField("Skip:", trialDialogue.Skip, 20);
        }

        protected override void DisplayFooter()
        {
            endEventEditor.DisplayEndEvents((MultiTrialDialogue)dia);

            base.DisplayFooter();

            endEventEditor.DisplayEndEventsButtons();

            GUILayout.Space(30);
        }

        
    }

    public class MultiTrialDialogueDebugToolEditor : TrialDialogueDebugToolEditor<MultiTrialLine>
    {
        public override bool DebugMode(DialogueCore dia) //*
        {
            base.DebugMode(dia);

            if (dia is MultiTrialDialogue multi)
            {
                AutoImportMultiPanels(multi);

                AssignPanelNums(multi, multi.assignedPanelNumbers, multi.panelCharacterNumbers);

                EditPanelCharacterList(ref multi.assignedPanelNumbers, ref multi.panelCharacterNumbers, multi.Speakers);
            }

            return exitDebugMode;
        }

        public static void AutoImportMultiPanels(MultiTrialDialogue dia)
        {
            if (dia != null && GUILayout.Button("Add Imported CharPanels", GUILayout.Width(160))
                    && EditorUtility.DisplayDialog("Convert", "Are you sure you want to Apply Multiline Panels?" +
                    "\n THIS WILL ADD ON TOP OF ALL PANELS AND CANNOT BE UNDONE!", "Yes", "No"))
            {
                MultiTrialDialogueEditorUtility.AutoImportPanels(dia);
            }

            GUILayout.Space(10);
        }



        protected void AssignPanelNums(MultiTrialDialogue dia, List<int> p, List<int> n)
        {
            dia.SetAutoAssign(HandyFields.BoolField(dia.autoAssignPanelNums, "Auto Assign Panel Nums: "));

            if (dia.autoAssignPanelNums && p.Count >= 2)
            {
                for (int i = 0; i < dia.Lines.Count; i++)
                {
                    if (n.Contains(dia.Lines[i].SpeakerNumber))
                    {
                        int autoNum = p[n.IndexOf(dia.Lines[i].SpeakerNumber)];
                        if (autoNum != -1)
                        {
                            dia.Lines[i].PanelNumber = autoNum;
                        }
                        for (int x = 0; x < dia.Lines[i].multiPanels.Count; x++)
                        {
                            if (n.Contains(dia.Lines[i].multiPanels[x].SpeakerNumber))
                            {
                                int autoPanelNum = p[n.IndexOf(dia.Lines[i].multiPanels[x].SpeakerNumber)];
                                if (autoPanelNum != -1)
                                {
                                    dia.Lines[i].multiPanels[x].SetPanelNum(autoPanelNum);
                                }
                            }

                        }
                    }

                }
            }
        }

        protected void EditPanelCharacterList(ref List<int> p, ref List<int> n, CharacterDatabase d)
        {

            if (GUILayout.Button("Add PanelChar", GUILayout.Width(170)))
            {
                p.Add(p.Count);
                n.Add(-1);
            }
            using (new GUILayout.VerticalScope("Box", GUILayout.Width(170)))
            {
                for (int x = 0; x < p.Count; x++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        p[x] = HandyFields.IntField("", p[x], 25);
                        EditorGUILayout.LabelField(": ", GUILayout.MaxWidth(25));
                        int r = n[x];
                        EditPanelCharacter(ref r, ref d);
                        n[x] = r;

                        GUILayout.Space(5);

                        if (GUILayout.Button("x", GUILayout.Width(25)))
                        {
                            p.Remove(p[x]);
                            n.Remove(n[x]);
                        }
                    }
                    GUILayout.Space(5);
                }

            }


        }
        protected Character EditPanelCharacter(ref int SpeakerNumber, ref CharacterDatabase d)
        {
            var prependedArray = ContainerUtil.PrependedList(d.GetNames().ToArray(), "<No Character>");
            SpeakerNumber = EditorGUILayout.IntPopup(SpeakerNumber, prependedArray, ContainerUtil.Iota(prependedArray.Length, -1), GUILayout.Width(130));
            return SpeakerNumber == -1 ? null : d.Characters[SpeakerNumber];
        }
    }

    [Serializable]
    public class MultiTrialLineEditor<T> : TrialLineEditor<T> where T : MultiTrialLine, new()
    {

        #region MULTITRIALDIALOGUE ADDITIONS

        protected override void DisplayLowerLine()
        {
            base.DisplayLowerLine();

            GUILayout.Space(5);

            EditMultiTrialLineLower(Lines[i]);
        }
        
        protected virtual void EditMultiTrialLineLower(T line)
        {
            line.ShowEditorPanels = EditorGUILayout.BeginFoldoutHeaderGroup(line.ShowEditorPanels,
                                new GUIContent($"Additional Panel Movements",
                                ""));

            if (line.ShowEditorPanels)
            {
                if (GUILayout.Button("Add Panel", GUILayout.Width(170)))
                {
                    line.multiPanels.Add(new MultiPanel());
                }
                using (new GUILayout.VerticalScope("Box", GUILayout.Width(170)))
                {
                    GUILayout.Space(5);
                    using (new EditorGUILayout.HorizontalScope())
                    {

                    }
                    for (int x = 0; x < line.multiPanels.Count; x++)
                    {
                        MultiPanel p = line.multiPanels[x];
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(170)))
                            {
                                EditPanelSpeaker(p);
                                EditPanelCamAnim(p);

                                p.SetPanelNum(HandyFields.IntField("Panel Num: ", p.panelNum, 50));
                            }
                            EditExpression(p);

                        }

                        tcoEditor.EditTCO(p.Speaker, false, p.co);

                        GUILayout.Space(5);
                        if (GUILayout.Button("Remove Panel", GUILayout.Width(170)))
                        {
                            line.multiPanels.Remove(p);
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        #region Panel Camera Anim
        protected void EditPanelCamAnim(MultiPanel currLine)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Cam:", GUILayout.Width(40));
                currLine.CamAnimIdx = EditorGUILayout.IntPopup(currLine.camAnimIdx,
                    cameraAnimDatabase.GetNames().ToArray(), ContainerUtil.Iota(cameraAnimDatabase.anims.Count));
            }
            using (new EditorGUILayout.HorizontalScope())
            {

                if (GUILayout.Button("Anim", GUILayout.Width(50)))
                {
                    if (!cam)
                        cam = MonoBehaviour.FindAnyObjectByType<DRTrialCamera>();
                    if (cam != null)
                    {
                        TestPanelCameraAnim(currLine, false);
                    }
                    else
                        Debug.Log("Unsuccessful");
                }
            }
        }
        protected void TestPanelCameraAnim(MultiPanel line, bool gameTest)
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
            }
            else
            {
                if (!line.Co.enabled && line.Speaker.TrialPosition < 16)
                    cam.SeatFocus = line.Speaker.TrialPosition;
                if (line.Co.enabled)
                {
                    cam.ApplyOverrides(line.Co.height, line.Co.distance, line.Co.seatFocus, false);
                }
                List<string> names = cameraAnimDatabase.GetNames();
                cam.TriggerAnim(names[line.camAnimIdx]);
            }



        }
        #endregion

        #region Speaker
        protected void EditPanelSpeaker(LineBase currLine)
        {
            var prependedArray = ContainerUtil.PrependedList(dia.GetCharacterNames(), "<No Character>");

            currLine.SetSpeakerNum(
                EditorGUILayout.IntPopup(currLine.SpeakerNumber, prependedArray, 
                ContainerUtil.Iota(prependedArray.Length, -1), GUILayout.Width(130)));

            currLine.SetSpeaker(currLine.SpeakerNumber == -1 ? null : dia.Speakers.Characters[currLine.SpeakerNumber]);

            //if (currLine.Speaker)
            //{
            //    var aliasNames = new string[currLine.Speaker.Aliases.Count + 1];
            //    aliasNames[0] = "(No Alias)";

            //    for (int j = 1; j < currLine.Speaker.Aliases.Count + 1; j++)
            //    {
            //        aliasNames[j] = currLine.Speaker.Aliases[j - 1].Name;
            //    }
            //    currLine.AliasNumber = EditorGUILayout.IntPopup(currLine.AliasNumber,
            //        aliasNames, dia.GetIntValues(currLine.Speaker.Aliases.Count + 1),
            //            GUILayout.Width(130));
            //}
        }
        #endregion

        #region Expression
        protected void EditExpression(LineBase currLine)
        {
            if (currLine.Speaker != null)
            {
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    var exprs = currLine.Speaker.Expressions.Count;

                    if (exprs < currLine.ExpressionNumber)
                    {
                        currLine.SetExpressionNum(0);
                    }

                    if (currLine.Expression != null)
                    {
                        GUIStyle expr = new();
                        if (currLine.ExpressionNumber > 0)
                        {
                            var tex = HandyFields.GetMaterialTexture(currLine.Expression.Sprite);
                            if (tex)
                            {
                                expr.normal.background = tex;
                            }
                        }
                        EditorGUILayout.LabelField(GUIContent.none, expr, GUILayout.Width(100),
                            GUILayout.Height(100));
                    }

                    var expressionNames = new string[currLine.Speaker.Expressions.Count + 1];
                    expressionNames[0] = "<No change>";

                    for (int j = 1; j < currLine.Speaker.Expressions.Count + 1; j++)
                    {
                        expressionNames[j] = currLine.Speaker.Expressions[j - 1].Name;
                    }

                    currLine.SetExpressionNum(EditorGUILayout.IntPopup(currLine.ExpressionNumber,
                        expressionNames, dia.GetIntValues(currLine.Speaker.Expressions.Count + 1), GUILayout.Width(100)));

                    if (currLine.ExpressionNumber > 0)
                    {
                        currLine.SetExpression(currLine.Speaker.Expressions[currLine.ExpressionNumber - 1]);
                    }
                    else
                    {
                        currLine.SetExpression(new Expression());
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(108),
                    GUILayout.Height(100));
            }
        }
        #endregion

        #endregion
    }

    public static class MultiTrialDialogueEditorUtility
    {
        public static void AutoImportPanels(MultiTrialDialogue dia)
        {
            List<int> foundChars = new();

            foreach (var line in dia.Lines)
            {
                FindCharactersInLine(dia, line, foundChars, line.SpeakerNumber);

                foreach (var multiPanel in line.multiPanels)
                    FindCharactersInLine(dia, line, foundChars, multiPanel.SpeakerNumber);
            }

            ApplyPanelsOnLines(dia, foundChars);
        }

        private static void FindCharactersInLine(MultiTrialDialogue dia, MultiTrialLine line, List<int> foundChars, int speakerNum)
        {
            if (!foundChars.Contains(speakerNum))
            {
                foundChars.Add(speakerNum);

                if (dia.panelCharacterNumbers.Count == 0)
                {
                    dia.panelCharacterNumbers.Add(line.SpeakerNumber);

                    dia.assignedPanelNumbers.Add(dia.assignedPanelNumbers.Count);
                }
            }
        }

        private static void ApplyPanelsOnLines(MultiTrialDialogue dia, List<int> foundChars)
        {
            foreach (var line in dia.Lines)
            {
                for (int i = 0; i < foundChars.Count; i++)
                {
                    if (foundChars[i] != line.SpeakerNumber)
                    {
                        var multiPanel = new MultiPanel();

                        multiPanel.SetSpeakerNum(foundChars[i]);

                        multiPanel.CamAnimIdx = line.CamAnimIdx;

                        line.multiPanels.Add(multiPanel);
                    }
                }
            }
        }
    }
}
#endif
