//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Utility
{
    /// <summary>
    /// Utility class for getting pixels from sprites when hitting them with raycasts.
    /// </summary>
    public static class PixelUtility
    {
        /// <summary>
        /// Gets the color of a pixel from a sprite renderer that intersects with a raycasted hitpoint within world space.
        /// </summary>
        /// <param name="worldHitPoint">The point in world space that has intersected with a sprite renderer.</param>
        /// <param name="spriteRenderer">The sprite renderer that has been hit by a raycast.</param>
        /// <returns>The color of a pixel from a sprite renderer that intersects with a raycasted hitpoint within world space.</returns>
        public static Color GetPixelColorUsingSpriteBounds(Vector2 worldHitPoint, SpriteRenderer spriteRenderer)
        {
            Vector2 normalized = GetNormalizedSpritePosition(worldHitPoint, spriteRenderer);

            normalized = ApplyFlipping(normalized, spriteRenderer);

            Vector2Int atlasCoord = GetAtlasPixelCoordinate(normalized, spriteRenderer);

            return SamplePixelColor(spriteRenderer.sprite.texture, atlasCoord);
        }

        /// <summary>
        /// Takes the hit point and normalizes it relative to the bounds of the sprite renderer.
        /// </summary>
        /// <param name="worldHitPoint">The point in world space that has intersected with a sprite renderer.</param>
        /// <param name="spriteRenderer">The sprite renderer that has been hit by a raycast.</param>
        /// <returns>The hit point normalized relative to the bounds of the sprite renderer.</returns>
        private static Vector2 GetNormalizedSpritePosition(Vector2 worldHitPoint, SpriteRenderer spriteRenderer)
        {
            Bounds spriteBounds = spriteRenderer.bounds;

            float normalizedX = (worldHitPoint.x - spriteBounds.min.x) / spriteBounds.size.x;

            float normalizedY = (worldHitPoint.y - spriteBounds.min.y) / spriteBounds.size.y;

            return new Vector2(normalizedX, normalizedY);
        }

        /// <summary>
        /// Inverts a considered normalized hit point on a sprite renderer and updates it if the renderer has any flip settings checked.
        /// </summary>
        /// <param name="normalized">The normalized point on a sprite renderer that has intersected with a world space ray cast.</param>
        /// <param name="spriteRenderer">The sprite renderer that has been hit by a raycast.</param>
        /// <returns>The normalized Vector2 with flipping applied to the relevant axies.</returns>
        private static Vector2 ApplyFlipping(Vector2 normalized, SpriteRenderer spriteRenderer)
        {
            float x = spriteRenderer.flipX ? 1f - normalized.x : normalized.x;

            float y = spriteRenderer.flipY ? 1f - normalized.y : normalized.y;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Takes a considered normalized hit point and converts it to pixel coordinate within a sprite renderers texture atlas.
        /// </summary>
        /// <param name="normalized">The normalized point on a sprite renderer that has intersected with a world space ray cast.</param>
        /// <param name="spriteRenderer">The sprite renderer get the texture atlas from.</param>
        /// <returns>The matching Integer pixel coordinate on the sprite renderer's texture's texture atlas.</returns>
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
        /// Bit block transfers the source texture to a temporary render texture so that the pixel coordinate on the texture can be returned.
        /// </summary>
        /// <param name="sourceTexture">The texture to retrieve the pixel from.</param>
        /// <param name="atlasCoordinate">The coordinate the targeted pixel is located at.</param>
        /// <returns>The color of the pixel at the coordinate on the texture atlas.</returns>
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
        /// Creates a 1 by 1 Texture using the coordinates of the Active Render Texture (ReadPixels()).
        /// </summary>
        /// <param name="atlasX">The X coordinate to take from the Active Render Texture.</param>
        /// <param name="atlasY">The Y coordinate to take from the Active Render Texture.</param>
        /// <returns>A 1 by 1 Texture with the coordinates of the Active Render Texture at the specified axies.</returns>
        private static Texture2D CreateReadableTexture(int atlasX, int atlasY)
        {
            Texture2D readablePixel = new(1, 1, TextureFormat.RGBA32, false);

            readablePixel.ReadPixels(new Rect(atlasX, atlasY, 1, 1), 0, 0);

            readablePixel.Apply();

            return readablePixel;
        }
    }
}