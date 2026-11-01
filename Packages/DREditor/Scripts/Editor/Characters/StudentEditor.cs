//Original Author: SeleniumSoul
//Edited by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Characters
{
    [CustomEditor(typeof(Student))]
    public class StudentEditor : UnityEditor.Editor
    {
        protected readonly InterfaceEditor<ICharacterData> characterDataEditor = new();

        protected bool CharacterDataFoldout = false;
        protected int characterDataIndex;
        private Student stu;
        private void OnEnable()
        {
            stu = target as Student;
        }

        public override void OnInspectorGUI()
        {
            HandyFields.Label("Character Editor");
            
            NameForm();

            StudentLabel();

            StudentCard();

            DefaultSprite();

            BlackExpression(); //*

            ImportEditorUtility.MissingSprites(stu);

            EditorGUILayout.LabelField("Sprites");

            EditorGUILayout.LabelField("Sprites can be sorted numerically using " +
                "DREditor/Characters/Sort Expressions by their numbers");

            using (new EditorGUILayout.HorizontalScope()) //*
            {
                GUILayout.Label("Show Sprites Mode", GUILayout.Width(120));
                stu.ShowSprites = EditorGUILayout.Toggle(stu.ShowSprites);
            }

            if (GUILayout.Button("Import Materials"))
                ImportEditorUtility.ImportMaterials(stu);

            if (GUILayout.Button("Import Sprites"))
                ImportEditorUtility.ImportSprites(stu);

            if (!stu.ShowSprites) //* Was Sprites();
                CustomLayoutUtility.DisplayHorizontal(Sprites);
            else
                Unlits();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(stu);
            
        }
        
        
        private void NameForm()
        {
            EditorGUILayout.BeginVertical("Box");
            stu.LastName = HandyFields.StringField("Last Name: ", stu.LastName);
            stu.FirstName = HandyFields.StringField("First Name: ", stu.FirstName);
            stu.StudentCard.Color = HandyFields.ColorField(stu.StudentCard.Color);
            stu.Nameplate = TextureFieldLabeledHorizontal("Default Nameplate: ", stu.Nameplate);
            stu.Headshot = TextureFieldLabeledHorizontal("Default Headshot: ", stu.Headshot);
            stu.TrialNameplate = TextureFieldLabeledHorizontal("Trial Nameplate: ", stu.TrialNameplate);
            stu.TrialPortrait = TextureFieldLabeledHorizontal("Trial Portrait: ", stu.TrialPortrait);
            stu.NSDPortrait = TextureFieldLabeledHorizontal("NSD Nameplate: ", stu.NSDPortrait);
            stu.TrialHeight = HandyFields.FloatField("Trial Height: ", stu.TrialHeight);
            stu.TrialPosition = HandyFields.IntField("Trial Position: ", stu.TrialPosition, 30);
            EditorGUILayout.LabelField("Actor Prefab: ");
            stu.ActorPrefab = HandyFields.UnityField(stu.ActorPrefab, 120, 30);
            
            

            EditorGUILayout.EndVertical();
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                AliasList(stu.Aliases);
            }
        }
        private void StudentLabel()
        {
            var bigLabelStyle = new GUIStyle
            {
                fontSize = 25,
                fontStyle = FontStyle.Bold
            };
            GUI.backgroundColor = stu.StudentCard.Color;
            GUILayout.Space(15);
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                GUILayout.FlexibleSpace();

                var labelText = stu.LastName == "" && stu.FirstName == "" ? "No Name" : "";

                GUILayout.Label(labelText + stu.LastName + " " + stu.FirstName, bigLabelStyle);
                GUILayout.FlexibleSpace();
            }
            bigLabelStyle.fontSize = 15;

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Ultimate " + stu.StudentCard.Talent, bigLabelStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.Space(15);
        }
        
        private void StudentCard()
        {
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                stu.StudentCard.Talent = HandyFields.StringField("Talent: ", stu.StudentCard.Talent);
                stu.StudentCard.Picture = TextureFieldLabeledHorizontal("Picture: ", stu.StudentCard.Picture);
                stu.StudentCard.Height = HandyFields.StringField("Height: ", stu.StudentCard.Height);
                stu.StudentCard.Weight = HandyFields.StringField("Weight: ", stu.StudentCard.Weight);
                stu.StudentCard.Chest = HandyFields.StringField("Chest: ", stu.StudentCard.Chest);
                stu.StudentCard.Age = HandyFields.StringField("Age: ", stu.StudentCard.Age);
                stu.StudentCard.BloodType = HandyFields.StringField("Blood Type: ", stu.StudentCard.BloodType);
                stu.StudentCard.DateOfBirth = HandyFields.StringField("D.O.B.: ", stu.StudentCard.DateOfBirth);
                stu.StudentCard.Likes = HandyFields.StringField("Likes: ", stu.StudentCard.Likes);
                stu.StudentCard.Dislikes = HandyFields.StringField("Dislikes: ", stu.StudentCard.Dislikes);
                stu.StudentCard.Notes = HandyFields.StringArea("Description: ", stu.StudentCard.Notes);
                stu.StudentCard.Signature = TextureFieldLabeledHorizontal("Signature: ", stu.StudentCard.Signature);


                using (new EditorGUILayout.HorizontalScope())
                {
                    stu.NotFTECharacter = HandyFields.Option(stu.NotFTECharacter, "Is Not FTE Character", 130);
                }

                DisplayDynamicCharacterData();
            }
        }

        
        protected virtual void DisplayDynamicCharacterData()
        {
            characterDataEditor.Initialize(this, stu.datas, CharacterDataFoldout, characterDataIndex);

            var property = serializedObject.FindProperty("datas");

            if (!stu.NotFTECharacter)
                using (new EditorGUILayout.HorizontalScope())
                    characterDataEditor.DisplayInterfaceInstances(property);

            CharacterDataFoldout = characterDataEditor.showDiaEvents;

            stu.SetCharacterDatas(characterDataEditor.instances);

            characterDataIndex = characterDataEditor.currentlyPickedIndex;
        }

        private void DefaultSprite()
        {
            HandyFields.Label("Default Sprite");

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(140)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        stu.DefaultSprite = TextureField(stu.DefaultSprite);
                        if (GUILayout.Button("X", GUILayout.Width(18)))
                        {
                            stu.DefaultSprite = null;
                        }
                    }
                }
            }
        }

        private void BlackExpression() //*
        {
            // try is because sometimes you'd get
            // ArgumentException: Getting control 2's position in a group with only 2 controls when doing repaint
            try
            {
                HandyFields.Label("Black Cutout Material");

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(140)))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            stu.BlackExpression = MaterialField(stu.BlackExpression);
                            if (GUILayout.Button("X", GUILayout.Width(18)))
                            {
                                stu.BlackExpression = null;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        
        

        private void Sprites()
        {
            var count = 0;
            for (var i = 0; i < stu.Expressions.Count; i++)
            {
                var expr = stu.Expressions[i];
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(140)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        expr.SetSprite(MaterialField(expr.Sprite));

                        DisplayExpressionButtons(expr, i);
                    }

                    expr.SetName(EditorGUILayout.TextField(expr.Name, GUILayout.Width(120)));
                }

                count++;
                if (count > 1)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    count = 0;
                }
            }

            AddExpressionButton();
        }


        protected virtual void DisplayExpressionButtons(Expression expr, int i)
        {
            // try is because sometimes you'd get
            // ArgumentException: Getting control 2's position in a group with only 2 controls when doing repaint
            try
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    if (GUILayout.Button("X", GUILayout.Width(18)))
                    {
                        stu.Expressions.Remove(expr);

                        stu.Sprites.Remove(stu.Sprites[i]);//*
                    }

                    if (i != 0 && GUILayout.Button("^", GUILayout.Width(18)))//*
                    {
                        Expression ex = expr;

                        stu.Expressions[i] = stu.Expressions[i - 1];

                        stu.Expressions[i - 1] = ex;

                        (stu.Sprites[i - 1], stu.Sprites[i]) = (stu.Sprites[i], stu.Sprites[i - 1]);
                    }

                    if (stu.MissingMat != null && stu.MissingTex != null && GUILayout.Button("+^", GUILayout.Width(25)))//*
                        InsertExpressionAt(i);

                    if (stu.MissingMat != null && stu.MissingTex != null && GUILayout.Button("+v", GUILayout.Width(25)))//*
                    {
                        InsertExpressionAt(i);

                        (stu.Expressions[i + 1], stu.Expressions[i]) = (stu.Expressions[i], stu.Expressions[i + 1]);

                        (stu.Sprites[i + 1], stu.Sprites[i]) = (stu.Sprites[i], stu.Sprites[i + 1]);
                    }

                    if (i != stu.Expressions.Count - 1 && GUILayout.Button("v", GUILayout.Width(18)))//*
                    {
                        Expression ex = expr;

                        stu.Expressions[i] = stu.Expressions[i + 1];

                        stu.Expressions[i + 1] = ex;

                        (stu.Sprites[i + 1], stu.Sprites[i]) = (stu.Sprites[i], stu.Sprites[i + 1]);
                    }
                }
            }
            catch { }
        }

        protected virtual void InsertExpressionAt(int i)
        {
            stu.Expressions.Insert(i, new Expression());

            stu.Sprites.Insert(i, new Unlit());
        }

        protected virtual void AddExpressionButton()
        {
            if (GUILayout.Button("Add \n Sprite", GUILayout.Width(100), GUILayout.Height(40)))
            {
                stu.Expressions.Add(new Expression());

                stu.Sprites.Add(null);//*
            }
        }

        #region Unlits
        //* Unlits
        private void Unlits()
        {
            var count = 0;
            using (new EditorGUILayout.HorizontalScope())
            {
                for (var i = 0; i < stu.Sprites.Count; i++)
                {
                    var unlit = stu.Sprites[i];
                    var expr = stu.Expressions[i];
                    using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(140)))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            unlit.SetSprite(HandyFields.UnityField(unlit.Sprite, height: 140));

                            /*if (GUILayout.Button("X", GUILayout.Width(18)))
                            {
                                stu.Expressions.Remove(expr);
                                stu.Sprites.Remove(stu.Sprites[i]);//*
                            }*/
                        }
                        GUILayout.Label(expr.Name, GUILayout.Width(120));
                        unlit.SetName(expr.Name);
                    }

                    count++;
                    if (count > 1)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        count = 0;
                    }
                }

                

            }
        }
        #endregion
        private static Texture2D TextureField(Texture2D texture)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                var style = new GUIStyle(GUI.skin.label)
                {
                    fixedWidth = 70
                };
                return HandyFields.UnityField(texture);
            }
        }

        private static Material MaterialField(Material mat)
        {
            Material result;
            using (new EditorGUILayout.HorizontalScope())
            {
                result = HandyFields.UnityField(mat, height:140);
            }

            if (mat != null)
            {
                var myTexture = AssetPreview.GetAssetPreview(HandyFields.GetMaterialTexture(mat));
                if(myTexture)
                {
                    GUILayout.Label(myTexture);
                }
            }
            return result;
        }

        private static Texture2D TextureFieldLabeledHorizontal(string label, Texture2D texture)
        {
            Texture2D result;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(label);
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                result = HandyFields.UnityField(texture, 70, 70);
            }
            return result;
        }

        private static void AliasList(List<Alias> aliasList)
        {
            if (GUILayout.Button("Add Alias", GUILayout.Width(120)))
            {
                aliasList.Add(new Alias());
            }
            if (aliasList != null)
            {
                for (int i = 0; i < aliasList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    aliasList[i].Name = HandyFields.StringField("Alias " + i + ": ",aliasList[i].Name);
                    var removed = GUILayout.Button("x", GUILayout.Width(20));
                    EditorGUILayout.EndHorizontal();
                    aliasList[i].Nameplate = TextureFieldLabeledHorizontal("Nameplate:", aliasList[i].Nameplate);
                    aliasList[i].TrialNameplate = TextureFieldLabeledHorizontal("Trial Nameplate:", aliasList[i].TrialNameplate);
                    aliasList[i].TrialPortrait = TextureFieldLabeledHorizontal("Trial Portrait:", aliasList[i].TrialPortrait);
                    if (removed)
                    {
                        aliasList.RemoveAt(i);
                    }
                }
            }
        }
    }
}
#endif

/*
        private void TrialMaterial() //*
        {
            HandyFields.Label("Trial Material");

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(140)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        stu.TrialMaterial = MaterialField(stu.TrialMaterial);
                        if (GUILayout.Button("X", GUILayout.Width(18)))
                        {
                            stu.BlackExpression = null;
                        }
                    }
                }
            }
        }
        */