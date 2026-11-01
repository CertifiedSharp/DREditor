//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [Serializable]
    public class ExpressionPreviewer
	{
		[SerializeField] protected Material expressionMaterial;
		[SerializeField] protected Rect expressionRect;
        
        public virtual void SetExpression(Expression expression)
		{
			if (expressionMaterial != null || expression == null || expression.Sprite == null)
                return;

            expressionRect = GUILayoutUtility.GetLastRect();

            if (expressionRect.Contains(Event.current.mousePosition))
                expressionMaterial = expression.Sprite;
        }

        public virtual void DrawHoveredSprite()
        {
			if (expressionMaterial == null || !expressionRect.Contains(Event.current.mousePosition))
            {
                expressionMaterial = null;

                return;
            }

            GUI.backgroundColor = Color.white;
            
            GUI.color = Color.white;

            GUIStyle exprstyle = new();

            var expressionTexture = HandyFields.GetMaterialTexture(expressionMaterial);

            exprstyle.normal.background = expressionTexture;

            if (exprstyle != null)
                EditorGUI.LabelField(
                    new Rect(
						Event.current.mousePosition.x,
						Event.current.mousePosition.y,
						expressionTexture.width / 5,
						expressionTexture.height / 5),
                    GUIContent.none, exprstyle);
        }
    }
}
#endif