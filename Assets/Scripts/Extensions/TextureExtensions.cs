using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TextureExtensions
    {
        public static Texture2D CropToSquareAndResample(this Texture2D texture, int size)
        {
            // Default to no cropping
            float uCropped = 1f;
            float vCropped = 1f;
            float uOffset = 0f;
            float vOffset = 0f;

            if (texture.width > texture.height)
            {
                // Crop horizontally
                uCropped = (float)texture.height / texture.width;
                vCropped = 1f;
                uOffset = (1f - uCropped) / 2f;
                vOffset = 0f;
            }
            else if (texture.height > texture.width)
            {
                // Crop vertically
                uCropped = 1f;
                vCropped = (float)texture.width / texture.height;
                uOffset = 0f;
                vOffset = (1f - vCropped) / 2f;
            }

            Color[] pixels = new Color[size * size];

            for (int y = 0; y < size; ++y)
            {
                float vDest = (float)y / size;
                float vSrc = vOffset + vDest * vCropped;

                for (int x = 0; x < size; ++x)
                {
                    float uDest = (float)x / size;
                    float uSrc = uOffset + uDest * uCropped;

                    pixels[y * size + x] = texture.GetPixelBilinear(uSrc, vSrc);
                }
            }

            Texture2D croppedTexture = new Texture2D(size, size, texture.format, false);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }
    }
}