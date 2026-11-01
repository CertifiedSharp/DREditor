//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Actors.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CircleDrawer
    {
        /// <summary>
        /// 
        /// </summary>
        [Range(0, 360f)]
        [Tooltip("")]
        [SerializeField] protected float totalDegrees = CircleUtility.DegreesInCircle;

        /// <summary>
        /// 
        /// </summary>
        [Range(-360f, 360f)]
        [Tooltip("")]
        [SerializeField] protected float startingDegreeOffset = -90f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color circleColor = new(1f, 1f, 1f, 0.2f);

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aroundCircleWidth = 120f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aroundCircleHeight = 20f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float circleAreaHeight = 600f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float dimensionMultiplier = 0.4f;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aroundXOffset = 113;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float aroundYOffset = 40;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float addHeight = 0;


        /// <summary>
        /// 
        /// </summary>
        protected Rect areaRect;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 center;

        /// <summary>
        /// 
        /// </summary>
        protected float radius;

        /// <summary>
        /// 
        /// </summary>
        private const float CenteringHalf = 0.5f;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="colored"></param>
        public virtual void Draw(bool colored = true)
        {
            if (colored)
                CircleUtility.DrawColoredCircle(DrawCircle, circleColor);
            else
                DrawCircle();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DrawCircle()
        {
            areaRect = CircleUtility.CreateDefaultCircleRect(circleAreaHeight + addHeight);

            center = areaRect.center;

            center.y += addHeight;

            radius = Mathf.Max(0f, MinimumRadius() * dimensionMultiplier);

            CircleUtility.DrawCircle(center, Vector3.forward, radius);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual float MinimumRadius() => Mathf.Min(areaRect.width - aroundCircleWidth, areaRect.height - aroundCircleHeight);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectCount"></param>
        /// <param name="drawObject"></param>
        /// <param name="onChanged"></param>
        public virtual void DrawObjectsAround(int objectCount, Action<Rect, int> drawObject, Action onChanged)
        {
            for (int slot = 0; slot < objectCount; slot++)
                DrawObjectAroundAt(objectCount, slot, drawObject, onChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectCount"></param>
        /// <param name="slot"></param>
        /// <param name="drawObject"></param>
        /// <param name="onChanged"></param>
        protected virtual void DrawObjectAroundAt(int objectCount, int slot, Action<Rect, int> drawObject, Action onChanged)
        {
            float angle = CalculateAngleStep(objectCount, slot);

            Vector2 pos = GetPositionAt(angle);

            float x = CalculateCoordinate(pos.x, aroundCircleWidth);

            float y = CalculateCoordinate(pos.y, aroundCircleHeight);

            Rect aroundCircleRect = new(x, y, aroundCircleWidth, aroundCircleHeight);

            EditorGUI.BeginChangeCheck();

            drawObject(aroundCircleRect, slot);

            if (EditorGUI.EndChangeCheck())
                onChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectCount"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        protected virtual float CalculateAngleStep(int objectCount, int slot) =>
            (startingDegreeOffset + totalDegrees * slot / objectCount) * Mathf.Deg2Rad;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected virtual Vector2 GetPositionAt(float angle)
        {
            Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));

            Vector2 pos = center + direction * radius;

            pos.x += direction.x * aroundXOffset;

            pos.y += direction.y * aroundYOffset;

            return pos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="subtractedOffset"></param>
        /// <returns></returns>
        protected virtual float CalculateCoordinate(float coord, float subtractedOffset) => coord - subtractedOffset * CenteringHalf;
    }
}