//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class PixelUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="worldHitPoint"></param>
        /// <param name="spriteRenderer"></param>
        /// <returns></returns>
        public static Color GetPixelColorUsingSpriteBounds(Vector2 worldHitPoint, SpriteRenderer spriteRenderer)
        {
            Vector2 normalized = GetNormalizedSpritePosition(worldHitPoint, spriteRenderer);

            normalized = ApplyFlipping(normalized, spriteRenderer);

            Vector2Int atlasCoord = GetAtlasPixelCoordinate(normalized, spriteRenderer);

            return SamplePixelColor(spriteRenderer.sprite.texture, atlasCoord);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="worldHitPoint"></param>
        /// <param name="spriteRenderer"></param>
        /// <returns></returns>
        private static Vector2 GetNormalizedSpritePosition(Vector2 worldHitPoint, SpriteRenderer spriteRenderer)
        {
            Bounds spriteBounds = spriteRenderer.bounds;

            float normalizedX = (worldHitPoint.x - spriteBounds.min.x) / spriteBounds.size.x;

            float normalizedY = (worldHitPoint.y - spriteBounds.min.y) / spriteBounds.size.y;

            return new Vector2(normalizedX, normalizedY);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="normalized"></param>
        /// <param name="spriteRenderer"></param>
        /// <returns></returns>
        private static Vector2 ApplyFlipping(Vector2 normalized, SpriteRenderer spriteRenderer)
        {
            float x = spriteRenderer.flipX ? 1f - normalized.x : normalized.x;

            float y = spriteRenderer.flipY ? 1f - normalized.y : normalized.y;

            return new Vector2(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="normalized"></param>
        /// <param name="spriteRenderer"></param>
        /// <returns></returns>
        private static Vector2Int GetAtlasPixelCoordinate(Vector2 normalized, SpriteRenderer spriteRenderer)
        {
            Texture2D sourceTexture = spriteRenderer.sprite.texture;

            Rect spriteRect = spriteRenderer.sprite.rect;

            float spritePixelX = normalized.x * spriteRect.width;

            float spritePixelY = normalized.y * spriteRect.height;

            int atlasX = Mathf.Clamp(Mathf.FloorToInt(spriteRect.x + spritePixelX), 0, sourceTexture.width - 1);

            int atlasY = Mathf.Clamp(Mathf.FloorToInt(spriteRect.y + spritePixelY), 0, sourceTexture.height - 1);

            return new Vector2Int(atlasX, atlasY);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceTexture"></param>
        /// <param name="atlasCoordinate"></param>
        /// <returns></returns>
        private static Color SamplePixelColor(Texture2D sourceTexture, Vector2Int atlasCoordinate)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                sourceTexture.width, sourceTexture.height, 0,
                RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(sourceTexture, renderTex);

            RenderTexture previous = RenderTexture.active;

            RenderTexture.active = renderTex;

            Texture2D readablePixel = CreateReadableTexture(atlasCoordinate.x, atlasCoordinate.y);

            RenderTexture.active = previous;

            RenderTexture.ReleaseTemporary(renderTex);

            Color resultColor = readablePixel.GetPixel(0, 0);

            MonoBehaviour.Destroy(readablePixel);

            return resultColor;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="atlasX"></param>
        /// <param name="atlasY"></param>
        /// <returns></returns>
        private static Texture2D CreateReadableTexture(int atlasX, int atlasY)
        {
            Texture2D readablePixel = new(1, 1, TextureFormat.RGBA32, false);

            readablePixel.ReadPixels(new Rect(atlasX, atlasY, 1, 1), 0, 0);

            readablePixel.Apply();

            return readablePixel;
        }
    }
}