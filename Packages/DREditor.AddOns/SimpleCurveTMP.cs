//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure
using UnityEngine;
using TMPro;

namespace DREditor.AddOns
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TMP_Text))]
    public class SimpleCurveTMP : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected AnimationCurve vertexCurve = new(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float curveScale = 50.0f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float rotationIntensity = 1.0f;


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text textComponent;


        /// <summary>
        ///
        /// </summary>
        const float delta = 0.01f;

        /// <summary>
        ///
        /// </summary>
        const int verticesPerCharacter = 4;


        protected virtual void Awake() => textComponent = GetComponent<TMP_Text>();

        protected virtual void Update() => WarpText();

        /// <summary>
        ///
        /// </summary>
        protected virtual void WarpText()
        {
            textComponent.ForceMeshUpdate();

            if (!WarpedCharactersSuccessfully())
                return;

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool WarpedCharactersSuccessfully()
        {
            TMP_TextInfo textInfo = textComponent.textInfo;

            if (HasNoCharacters(textInfo))
                return false;

            float textWidth = CalculateTextWidth();

            if (IsWidthInvalid(textWidth))
                return false;

            WarpTextCharacters(textInfo, textWidth);

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <returns></returns>
        private bool HasNoCharacters(TMP_TextInfo textInfo) => textInfo.characterCount == 0;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private float CalculateTextWidth() => textComponent.bounds.max.x - textComponent.bounds.min.x;

        /// <summary>
        ///
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        private bool IsWidthInvalid(float width) => width <= 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="textInfo"></param>
        /// <param name="textWidth"></param>
        protected virtual void WarpTextCharacters(TMP_TextInfo textInfo, float textWidth)
        {
            float boundsMinX = textComponent.bounds.min.x;

            for (int characterIndex = 0; characterIndex < textInfo.characterCount; characterIndex++)
            {
                TMP_CharacterInfo characterInfo = textInfo.characterInfo[characterIndex];

                if (!characterInfo.isVisible)
                    continue;

                WarpSingleCharacter(characterInfo, textInfo, boundsMinX, textWidth);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="characterInfo"></param>
        /// <param name="textInfo"></param>
        /// <param name="boundsMinX"></param>
        /// <param name="textWidth"></param>
        protected virtual void WarpSingleCharacter(TMP_CharacterInfo characterInfo, TMP_TextInfo textInfo,
            float boundsMinX, float textWidth)
        {
            Vector3[] vertices = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;

            int baseVertexIndex = characterInfo.vertexIndex;

            float midpointX = GetCharacterMidpointX(vertices, baseVertexIndex);

            float normalizedX = (midpointX - boundsMinX) / textWidth;

            float verticalOffset = vertexCurve.Evaluate(normalizedX) * curveScale;

            float rotationAngle = CalculateSlopeAngle(normalizedX, textWidth);

            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            Vector3 center = CalculateCharacterCenter(vertices, baseVertexIndex, midpointX);

            ApplyTransformationToVertices(vertices, baseVertexIndex, center, rotation, verticalOffset);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="baseVertexIndex"></param>
        /// <returns></returns>
        private float GetCharacterMidpointX(Vector3[] vertices, int baseVertexIndex)
            => (vertices[baseVertexIndex + 0].x + vertices[baseVertexIndex + 2].x) / 2f;

        /// <summary>
        ///
        /// </summary>
        /// <param name="normalizedX"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        protected virtual float CalculateSlopeAngle(float normalizedX, float textWidth)
        {
            float lookAheadX = Mathf.Min(normalizedX + delta, 1.0f);

            float lookBehindX = Mathf.Max(normalizedX - delta, 0.0f);

            float heightAhead = vertexCurve.Evaluate(lookAheadX) * curveScale;

            float heightBehind = vertexCurve.Evaluate(lookBehindX) * curveScale;

            float rise = heightAhead - heightBehind;

            float run = (lookAheadX - lookBehindX) * textWidth;

            float calculatedAngle = Mathf.Atan2(rise, run) * Mathf.Rad2Deg;

            return calculatedAngle * rotationIntensity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="baseVertexIndex"></param>
        /// <param name="midpointX"></param>
        /// <returns></returns>
        protected virtual Vector3 CalculateCharacterCenter(Vector3[] vertices, int baseVertexIndex, float midpointX)
        {
            float midpointY = (vertices[baseVertexIndex + 0].y + vertices[baseVertexIndex + 2].y) / 2f;

            return new Vector3(midpointX, midpointY, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="baseVertexIndex"></param>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="verticalOffset"></param>
        protected virtual void ApplyTransformationToVertices(Vector3[] vertices, int baseVertexIndex,
            Vector3 center, Quaternion rotation, float verticalOffset)
        {
            Vector3 verticalOffsetVector = new(0, verticalOffset, 0);

            for (int vertexOffset = 0; vertexOffset < verticesPerCharacter; vertexOffset++)
            {
                int currentVertexIndex = baseVertexIndex + vertexOffset;

                Vector3 originalVertex = vertices[currentVertexIndex];

                Vector3 localPosition = originalVertex - center;

                Vector3 rotatedPosition = rotation * localPosition;

                vertices[currentVertexIndex] = center + rotatedPosition + verticalOffsetVector;
            }
        }
    }
}