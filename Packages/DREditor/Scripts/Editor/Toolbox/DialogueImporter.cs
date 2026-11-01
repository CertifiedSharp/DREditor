//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    [Serializable]
    public class DialogueImporter : DialogueImporterBase
    {
        [Tooltip("")]
        public CharacterDatabase database;

        [Tooltip("")]
        public string regexString = @"^(.+?)(?: *)/(?: *)(.+?)(?: *):(?: *)(.*)$";

        [Tooltip("")]
        [SelectType]
        [SerializeReference] protected List<IDialogueCaptureGroup> captureGroups = new()
        {
            new CharacterCaptureGroup(),
            new ExpressionCaptureGroup(),
            new TextCaptureGroup(),
        };

        [Tooltip("")]
        public int maxLinesPerDialogue = 25;

        [Tooltip("")]
        public int maxCharactersPerLine = 200;

        [Tooltip("")]
        public bool newOnEmptyLine = false;

        [Tooltip("")]
        public int emptyLineLimit = 2;


        protected Regex regex;

        protected Line lastLine = null;

        protected int emptyLineCounter = 0;

        protected int currentLine = 0;

        protected bool lineBroke = false;


        protected readonly DialogueEventImporter EventImporter = new();


        protected virtual void ClearEmptyCounter() => emptyLineCounter = 0;

        protected virtual void IncreaseEmptyCounter() => emptyLineCounter++;


        public override bool ImporterValid()
        {
            if (!DatabaseUtility.InitializeDatabase(database, out database))
                return false;

            foreach (var group in captureGroups)
                if (group is IValidCaptureGroup validator && !validator.GroupValid())
                    return false;

            return true;
        }

        protected override T CreateNewDialogue<T>()
        {
            T currentDialogue = base.CreateNewDialogue<T>();

            currentDialogue.SetSpeakers(database);

            return currentDialogue;
        }

        protected override void InitializeImport()
        {
            bytesRead = 0;

            regex = new(regexString);

            ClearEmptyCounter();

            ClearLines();
        }

        protected virtual void ClearLines()
        {
            currentLine = 0;

            lastLine = null;
        }

        protected override bool ImportedDialogueLine<T, T2>(List<T> dialoguesToSave, ref T currentDialogue) =>
            MatchedComment() || OnEmptyLine() || ImportedLine<T, T2>(dialoguesToSave, ref currentDialogue);

        protected virtual bool MatchedComment()
        {
            bool result = line.StartsWith("//");

            if (result)
            {
                EventImporter.EvalEvents(line);

                ClearEmptyCounter();
            }

            return result;
        }

        protected virtual bool OnEmptyLine()
        {
            bool result = line.Trim().Length == 0;

            if (result)
                IncreaseEmptyCounter();

            return result;
        }

        protected virtual bool ImportedLine<T, T2>(List<T> dialoguesToSave, ref T currentDialogue)
            where T : DialogueCore where T2 : Line, new()
        {
            if ((newOnEmptyLine && emptyLineCounter >= emptyLineLimit) || currentDialogue.LineCount + 1 > maxLinesPerDialogue)
                currentDialogue = AddNewDialogueToSave(dialoguesToSave, ClearLines);

            T2 dialogueLine = CreateNewLine<T2>();

            if (regex.IsMatch(line))
            {
                Match match = regex.Match(line);

                ApplyCaptureGroupsWhere((captureGroup) => captureGroup.IsPriority, match, ref dialogueLine);

                ApplyCaptureGroupsWhere((captureGroup) => !captureGroup.IsPriority, match, ref dialogueLine);

                NormalizeLines(currentDialogue, ref dialogueLine);
            }
            else if (LineFormatIncorrect())
                return false;
            else
                NormalizePreviousLine(currentDialogue, ref dialogueLine);

            if (lineBroke == true)
            {
                currentLine++;

                lineBroke = false;
            }

            currentLine++;

            ClearEmptyCounter();

            return true;
        }

        protected virtual void ApplyCaptureGroupsWhere<T>(Func<IDialogueCaptureGroup, bool> evaluation, Match match, ref T dialogueLine)
            where T : Line
        {
            for (int i = 1; i < match.Groups.Count; i++)
                if (evaluation(captureGroups[i - 1]))
                    captureGroups[i - 1].Capture(ref dialogueLine, match.Groups[i].Value);
        }

        protected virtual T CreateNewLine<T>() where T : Line, new() => EventImporter.AddEvents(new T());


        protected virtual void NormalizeLines<T, T2>(T currentDialogue, ref T2 dialogueLine)
            where T : DialogueCore where T2 : Line, new()
        {
            List<T2> normalizedLines = BreakLine<T2>(dialogueLine);

            if (normalizedLines.Count > 0)
                lastLine = normalizedLines[^1];

            currentDialogue.GetLines<T2>().AddRange(normalizedLines);
        }

        protected virtual bool LineFormatIncorrect()
        {
            bool result = lastLine == null;

            if (result)
                EditorUtility.DisplayDialog("Format incorrect", "The file didn't comply with the format\n" +
                        "on line " + currentLine + " \n Line: " + line, "OK");

            return result;
        }

        protected virtual void NormalizePreviousLine<T, T2>(T currentDialogue, ref T2 dialogueLine)
            where T : DialogueCore where T2 : Line, new()
        {
            // Trim whitespaces to ensure that there are none before or after the line break character
            dialogueLine.SetText(lastLine.Text.Trim() + "\n" + line.Trim());

            dialogueLine.SetSpeakerNum(lastLine.SpeakerNumber);

            dialogueLine.SetSpeaker(lastLine.Speaker);

            dialogueLine.SetAliasNumber(lastLine.AliasNumber);

            List<T2> normalizedLines = BreakLine(dialogueLine);

            lastLine.SetText(normalizedLines[0].Text);

            normalizedLines.RemoveAt(0);

            currentDialogue.GetLines<T2>().AddRange(normalizedLines);
        }


        protected virtual List<T> BreakLine<T>(T dialogueLine)
            where T : Line, new()
        {
            int aliasNumber = dialogueLine.AliasNumber;

            string speakerName = dialogueLine.Speaker.FirstName;

            string lineContent = dialogueLine.Text;

            List<T> result = new();

            int lineLenghtOffset = speakerName != null ?
                -1 * (speakerName + ": ").Length : 0;

            if (lineContent.Length + lineLenghtOffset <= maxCharactersPerLine)
                result.Add(CreateBreakLine<T>(dialogueLine, lineContent, aliasNumber + 1));
            else
            {
                // Break the line by words
                string[] words = lineContent.Split(' ');

                T currentLine = CreateBreakLine(dialogueLine, "");

                result.Add(currentLine);

                BreakLineByWords(currentLine, dialogueLine, words, lineLenghtOffset, result);
            }

            return result;
        }

        protected virtual void BreakLineByWords<T>(T currentLine, T dialogueLine, string[] words, int lineLenghtOffset,
            List<T> result)
            where T : Line, new()
        {
            foreach (string word in words)
            {
                if (currentLine.Text.Length + lineLenghtOffset == 0)
                {
                    currentLine.Text += word;

                    lineBroke = true;

                    continue;
                }

                if (word.Contains("\n"))
                {
                    if (currentLine.Text.Length + lineLenghtOffset + word.Length + 1 <= maxCharactersPerLine)
                        currentLine.Text += " " + word;
                    else
                    {
                        string[] parts = word.Split('\n');

                        // Try to fit both words keeping the line break character
                        if (currentLine.Text.Length + lineLenghtOffset + parts[0].Length + 1 <= maxCharactersPerLine)
                        {
                            currentLine.Text += " " + parts[0];

                            if (currentLine.Text.Length + lineLenghtOffset + parts[1].Length + 1 <= maxCharactersPerLine)
                                currentLine.Text += "\n" + parts[1];// Best case scenario. Both words fit properly.
                            else // Second word doesn't fit. It will need to go on a separate line.
                                result.Add(CreateBreakLine<T>(dialogueLine, parts[1]));
                        }
                        else
                        {
                            // If not even the first word fits on the line.
                            // Both words will be on a new line separated by a whitespace instead of a line break
                            result.Add(CreateBreakLine<T>(dialogueLine, parts[0] + " " + parts[1]));
                        }
                    }

                    continue;
                }

                if (currentLine.Text.Length + lineLenghtOffset + word.Length + 1 > maxCharactersPerLine)
                    result.Add(CreateBreakLine<T>(dialogueLine, word));
                else
                    currentLine.Text += ' ' + word;
            }
        }

        protected virtual T CreateBreakLine<T>(T lineInfo, string text, int aliasIndex = -1)
            where T : Line, new()
        {
            T currentLine = new();

            currentLine.SetSpeaker(lineInfo.Speaker);

            currentLine.SetSpeakerNum(lineInfo.SpeakerNumber);

            currentLine.SetText(text);

            currentLine.SetExpression(lineInfo.Expression);

            currentLine.SetExpressionNum(lineInfo.ExpressionNumber);

            if (aliasIndex != -1)
                currentLine.SetAliasNumber(aliasIndex);

            return currentLine;
        }


        protected override void OnImportComplete()
        {
            foreach (var group in captureGroups)
                if (group is IImportCompleteResponse responder)
                    responder.OnImportComplete();
        }
    }
}