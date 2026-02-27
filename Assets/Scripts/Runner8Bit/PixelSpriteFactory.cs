using UnityEngine;

namespace Runner8Bit
{
    public static class PixelSpriteFactory
    {
        public static Sprite MakeSolidSprite(Color color, int size = 16)
        {
            var texture = NewTexture(size);
            var pixels = new Color[size * size];
            for (var i = 0; i < pixels.Length; i++) pixels[i] = color;
            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }

        public static Sprite MakeGroundTileSprite(int size = 16)
        {
            var texture = NewTexture(size);
            var dark = new Color(0.11f, 0.37f, 0.16f);
            var mid = new Color(0.19f, 0.58f, 0.22f);
            var light = new Color(0.44f, 0.81f, 0.35f);
            var pixels = new Color[size * size];

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var idx = y * size + x;
                    pixels[idx] = y < 3 ? dark : mid;
                    if ((x + y) % 5 == 0) pixels[idx] = light;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }

        public static Sprite MakePlayerSprite(int size = 16)
        {
            var texture = NewTexture(size);
            var pixels = new Color[size * size];
            var transparent = new Color(0f, 0f, 0f, 0f);
            var body = new Color(0.2f, 0.9f, 0.88f);
            var shade = new Color(0.12f, 0.62f, 0.65f);
            var eye = new Color(0.98f, 0.98f, 0.98f);

            for (var i = 0; i < pixels.Length; i++) pixels[i] = transparent;
            for (var y = 2; y < 14; y++)
            {
                for (var x = 3; x < 13; x++)
                {
                    pixels[y * size + x] = body;
                }
            }

            for (var y = 2; y < 14; y++) pixels[y * size + 3] = shade;
            for (var y = 2; y < 14; y++) pixels[y * size + 12] = shade;
            pixels[10 * size + 6] = eye;
            pixels[10 * size + 9] = eye;
            pixels[4 * size + 6] = shade;
            pixels[4 * size + 7] = shade;
            pixels[4 * size + 8] = shade;
            pixels[4 * size + 9] = shade;

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }

        public static Sprite MakeObstacleSprite(int variant, int size = 16)
        {
            var texture = NewTexture(size);
            var dark = new Color(0.42f, 0.14f, 0.12f);
            var mid = new Color(0.87f, 0.28f, 0.22f);
            var light = new Color(0.98f, 0.68f, 0.22f);
            var transparent = new Color(0f, 0f, 0f, 0f);
            var pixels = new Color[size * size];
            for (var i = 0; i < pixels.Length; i++) pixels[i] = transparent;

            if (variant == 0)
            {
                for (var y = 1; y < 12; y++)
                {
                    var left = 7 - y / 2;
                    var right = 8 + y / 2;
                    for (var x = left; x <= right; x++) pixels[y * size + x] = mid;
                }
            }
            else if (variant == 1)
            {
                for (var y = 1; y < 13; y++)
                {
                    for (var x = 5; x < 11; x++) pixels[y * size + x] = mid;
                }
            }
            else
            {
                for (var y = 2; y < 12; y++)
                {
                    for (var x = 3; x < 13; x++) pixels[y * size + x] = mid;
                }
            }

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var idx = y * size + x;
                    if (pixels[idx].a < 0.1f) continue;
                    if (x < size - 1 && pixels[y * size + (x + 1)].a < 0.1f) pixels[idx] = dark;
                    if (y == 11 || y == 12) pixels[idx] = light;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }

        public static Sprite MakeStarSprite(int size = 8)
        {
            var texture = NewTexture(size);
            var transparent = new Color(0f, 0f, 0f, 0f);
            var glow = new Color(0.93f, 0.95f, 0.95f, 0.95f);
            var pixels = new Color[size * size];
            for (var i = 0; i < pixels.Length; i++) pixels[i] = transparent;

            var c = size / 2;
            for (var i = 1; i < size - 1; i++)
            {
                pixels[c * size + i] = glow;
                pixels[i * size + c] = glow;
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }

        private static Texture2D NewTexture(int size)
        {
            return new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
        }
    }
}
