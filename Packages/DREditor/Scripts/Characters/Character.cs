//Original Author: Happy30
//Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Characters
{    
    [System.Serializable]
    public class Character : ScriptableObject
    {
        public string translationKey;

        [FormerlySerializedAs("LastName")]
        public string lastName = "";
        public string LastName { get => lastName; set => lastName = value; }

        [FormerlySerializedAs("FirstName")]
        public string firstName = "";
        public string FirstName { get => firstName; set => firstName = value; }

        [FormerlySerializedAs("DefaultSprite")] 
        public Texture2D defaultSprite; 
        public Texture2D DefaultSprite { get => defaultSprite; set => defaultSprite = value; }

        [FormerlySerializedAs("TrialPortrait")] 
        public Texture2D trialPortrait; 
        public Texture2D TrialPortrait { get => trialPortrait; set => trialPortrait = value; }

        [FormerlySerializedAs("NSDPortrait")] 
        public Texture2D nSDPortrait; 
        public Texture2D NSDPortrait { get => nSDPortrait; set => nSDPortrait = value; }//*

        [FormerlySerializedAs("ActorPrefab")] 
        public GameObject actorPrefab; 
        public GameObject ActorPrefab { get => actorPrefab; set => actorPrefab = value; }//*

#if UNITY_EDITOR
        [FormerlySerializedAs("ShowSprites")]
        public bool showSprites = false; // For editor window's use
        public bool ShowSprites { get => showSprites; set => showSprites = value; }
#endif
        [FormerlySerializedAs("BlackExpression")] 
        public Material blackExpression; 
        public Material BlackExpression { get => blackExpression; set => blackExpression = value; }//*

        [FormerlySerializedAs("TrialMaterial")] 
        public Material trialMaterial; 
        public Material TrialMaterial { get => trialMaterial; set => trialMaterial = value; }

        [FormerlySerializedAs("MissingMat")] 
        public Material missingMat; 
        public Material MissingMat { get => missingMat; set => missingMat = value; }//*

        [FormerlySerializedAs("MissingTex")] 
        public Sprite missingTex; 
        public Sprite MissingTex { get => missingTex; set => missingTex = value; }//*

        public List<Expression> Expressions = new(); 

        public List<Unlit> Sprites = new();// for using sprite renderers


        public List<Alias> Aliases = new(); 


        [FormerlySerializedAs("Nameplate")] 
        public Texture2D nameplate; 
        public Texture2D Nameplate { get => nameplate; set => nameplate = value; }

        [FormerlySerializedAs("Headshot")] 
        public Texture2D headshot; 
        public Texture2D Headshot { get => headshot; set => headshot = value; }

        [FormerlySerializedAs("TrialNameplate")] 
        public Texture2D trialNameplate; 
        public Texture2D TrialNameplate { get => trialNameplate; set => trialNameplate = value; }

        [FormerlySerializedAs("TrialHeight")] 
        public float trialHeight = 7.88f; 
        public float TrialHeight { get => trialHeight; set => trialHeight = value; }

        [FormerlySerializedAs("TrialPosition")] 
        public int trialPosition = 0; 
        public int TrialPosition { get => trialPosition; set => trialPosition = value; }


        [FormerlySerializedAs("NotFTECharacter")] 
        public bool notFTECharacter = false; 
        public bool NotFTECharacter { get => notFTECharacter; set => notFTECharacter = value; } // if false then FTE character

        [SelectType]
        [SerializeReference]
        public List<ICharacterData> datas = new();


        public virtual void SetCharacterDatas(List<ICharacterData> datas) => this.datas = datas;


        public virtual T Get<T>() where T : ICharacterData
        {
            foreach (var data in datas)
                if (data is T soughtData)
                    return soughtData;

            return default;
        }

        public virtual bool Has<T>(out T soughtData) where T : ICharacterData
        {
            soughtData = default;

            foreach (var data in datas)
                if (data is T foundData)
                    soughtData = foundData;

            return soughtData is not null;
        }

        public void SetExpressions(List<Expression> Expressions) => this.Expressions = Expressions;

        public void SetSprites(List<Unlit> Sprites) => this.Sprites = Sprites;

        public int[] GetExpressionIntValues()
        {

            int[] values = new int[Expressions.Count + 1];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = i;
            }
            return values;
        }

        public Texture GetSpriteByName(string name)
        {
            foreach(Unlit u in Sprites)
            {
                if (u.Name == name)
                    return u.Sprite.texture;
            }
            UnityEngine.Debug.LogWarning("COULDN'T GET SPRITE BY NAME: " + name);
            return null;
        }

        public string GetSpriteLabelByTexName(string name)
        {
            foreach (Unlit u in Sprites)
            {
                if (u.Sprite.texture.name == name)
                    return u.Name;
            }
            UnityEngine.Debug.LogWarning("COULDN'T GET SPRITE LABEL BY TEX NAME: " + name);
            return null;
        }

        public Expression GetExpressionMatching(string expressionName)
        {
            foreach (Expression expression in Expressions)
                if (expression.Name == expressionName)
                    return expression;

            return null;
        }
    }
}
