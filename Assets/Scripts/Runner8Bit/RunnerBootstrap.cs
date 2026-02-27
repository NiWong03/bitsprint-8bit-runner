using UnityEngine;
using UnityEngine.UI;

namespace Runner8Bit
{
    public class RunnerBootstrap : MonoBehaviour
    {
        private static readonly Color Sky = new Color(0.07f, 0.08f, 0.17f);
        private static readonly Color GroundTint = new Color(1f, 1f, 1f, 1f);

        private void Awake()
        {
            Physics2D.gravity = new Vector2(0f, -30f);

            SetupCamera();
            SetupBackground();
            SetupScene();
            SetupGameSystems();
        }

        private static void SetupCamera()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                var camObj = new GameObject("Main Camera");
                cam = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
            }

            cam.backgroundColor = Sky;
            cam.orthographic = true;
            cam.orthographicSize = 5.5f;
            cam.transform.position = new Vector3(0f, 0f, -10f);
        }

        private static void SetupBackground()
        {
            CreateLayerStrip("FarHillsA", new Vector3(-8f, -0.8f, 0f), new Vector3(24f, 4f, 1f), new Color(0.12f, 0.16f, 0.28f), -1, 0.12f);
            CreateLayerStrip("FarHillsB", new Vector3(16f, -0.8f, 0f), new Vector3(24f, 4f, 1f), new Color(0.12f, 0.16f, 0.28f), -1, 0.12f);

            CreateLayerStrip("NearHillsA", new Vector3(-8f, -1.4f, 0f), new Vector3(24f, 3f, 1f), new Color(0.18f, 0.25f, 0.39f), 0, 0.24f);
            CreateLayerStrip("NearHillsB", new Vector3(16f, -1.4f, 0f), new Vector3(24f, 3f, 1f), new Color(0.18f, 0.25f, 0.39f), 0, 0.24f);

            for (var i = 0; i < 18; i++)
            {
                var star = new GameObject($"Star_{i}");
                var sr = star.AddComponent<SpriteRenderer>();
                sr.sprite = PixelSpriteFactory.MakeStarSprite();
                sr.sortingOrder = -2;
                star.transform.position = new Vector3(Random.Range(-10f, 12f), Random.Range(0.5f, 5f), 0f);
                var parallax = star.AddComponent<RunnerParallaxLayer>();
                var speed = i % 2 == 0 ? 0.04f : 0.07f;
                SetParallax(parallax, speed, -12f, 12f);
            }
        }

        private void SetupScene()
        {
            var ground = new GameObject("Ground");
            ground.transform.position = new Vector3(0f, -2.5f, 0f);
            ground.transform.localScale = new Vector3(42f, 1.2f, 1f);
            var sr = ground.AddComponent<SpriteRenderer>();
            sr.sprite = PixelSpriteFactory.MakeGroundTileSprite();
            sr.color = GroundTint;
            sr.sortingOrder = 1;
            ground.AddComponent<BoxCollider2D>();
            ground.AddComponent<RunnerGround>();

            var player = new GameObject("Player");
            player.transform.position = new Vector3(-6f, -1.25f, 0f);
            player.transform.localScale = new Vector3(1f, 1.2f, 1f);
            var playerRenderer = player.AddComponent<SpriteRenderer>();
            playerRenderer.sprite = PixelSpriteFactory.MakePlayerSprite();
            playerRenderer.sortingOrder = 3;

            var rb = player.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            player.AddComponent<BoxCollider2D>();
            player.AddComponent<RunnerPlayer>();
        }

        private static void SetupGameSystems()
        {
            var systems = new GameObject("GameSystems");
            var manager = systems.AddComponent<RunnerGameManager>();
            var spawner = systems.AddComponent<RunnerSpawner>();

            var player = FindFirstObjectByType<RunnerPlayer>();

            var canvasObj = new GameObject("HUD");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<GraphicRaycaster>();

            var scoreText = CreateText(canvas.transform, new Vector2(18, -18), TextAnchor.UpperLeft, 28, "SCORE: 0");
            var highScoreText = CreateText(canvas.transform, new Vector2(-18, -18), TextAnchor.UpperRight, 26, "HI: 0", true);
            var difficultyText = CreateText(canvas.transform, new Vector2(18, -54), TextAnchor.UpperLeft, 24, "DIFFICULTY: -");
            var centerText = CreateText(canvas.transform, Vector2.zero, TextAnchor.MiddleCenter, 30,
                "8-BIT RUNNER\n1 = EASY  2 = MEDIUM  3 = HARD\nSPACE/UP = JUMP\nSURVIVE AND BEAT YOUR HI-SCORE");

            centerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            centerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            centerText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            centerText.rectTransform.anchoredPosition = Vector2.zero;
            centerText.rectTransform.sizeDelta = new Vector2(980, 520);

            manager.Configure(spawner, player, scoreText, centerText, difficultyText, highScoreText);
        }

        private static Text CreateText(Transform parent, Vector2 anchoredPos, TextAnchor align, int size, string value, bool anchorTopRight = false)
        {
            var obj = new GameObject($"Text_{value.Split('\n')[0]}");
            obj.transform.SetParent(parent, false);
            var text = obj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.alignment = align;
            text.fontSize = size;
            text.color = new Color(0.96f, 0.96f, 0.96f);
            text.text = value;

            var shadow = obj.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.55f);
            shadow.effectDistance = new Vector2(2f, -2f);

            var rect = text.rectTransform;
            rect.anchorMin = anchorTopRight ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.anchorMax = anchorTopRight ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.pivot = anchorTopRight ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = new Vector2(900, 160);

            return text;
        }

        private static void CreateLayerStrip(string name, Vector3 pos, Vector3 scale, Color color, int sortingOrder, float speed)
        {
            var layer = new GameObject(name);
            var sr = layer.AddComponent<SpriteRenderer>();
            sr.sprite = PixelSpriteFactory.MakeSolidSprite(color);
            sr.sortingOrder = sortingOrder;
            layer.transform.position = pos;
            layer.transform.localScale = scale;
            var parallax = layer.AddComponent<RunnerParallaxLayer>();
            SetParallax(parallax, speed, -20f, 20f);
        }

        private static void SetParallax(RunnerParallaxLayer parallax, float speed, float wrapX, float resetX)
        {
            parallax.Configure(speed, wrapX, resetX);
        }
    }
}
