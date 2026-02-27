using UnityEngine;

namespace Runner8Bit
{
    public class RunnerSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnX = 14f;
        [SerializeField] private float laneY = -2f;

        private float cooldown;

        private void OnEnable()
        {
            cooldown = 0.5f;
        }

        private void Update()
        {
            var manager = RunnerGameManager.Instance;
            if (manager == null || !manager.IsPlaying) return;

            cooldown -= Time.deltaTime;
            if (cooldown > 0f) return;

            SpawnObstacle();

            var baseInterval = manager.SpawnInterval();
            cooldown = Random.Range(baseInterval * 0.8f, baseInterval * 1.2f);
        }

        private void SpawnObstacle()
        {
            var obstacle = new GameObject("Obstacle");
            obstacle.AddComponent<RunnerObstacle>();

            var sr = obstacle.AddComponent<SpriteRenderer>();
            sr.sprite = PixelSpriteFactory.MakeObstacleSprite(Random.Range(0, 3));
            sr.sortingOrder = 2;

            var box = obstacle.AddComponent<BoxCollider2D>();
            var mover = obstacle.AddComponent<RunnerObstacleMover>();
            _ = mover;

            var typeRoll = Random.value;
            var scale = new Vector3(1f, 1f, 1f);
            var y = laneY + 0.5f;

            var manager = RunnerGameManager.Instance;
            if (manager != null && manager.CurrentDifficulty != RunnerDifficulty.Easy && typeRoll > 0.6f)
            {
                scale = new Vector3(0.8f, 2f, 1f);
                y = laneY + 1f;
            }

            if (manager != null && manager.CurrentDifficulty == RunnerDifficulty.Hard && typeRoll < 0.25f)
            {
                scale = new Vector3(2f, 1f, 1f);
                y = laneY + 0.5f;
            }

            obstacle.transform.position = new Vector3(spawnX, y, 0f);
            obstacle.transform.localScale = scale;
            box.size = Vector2.one;
            box.offset = new Vector2(0f, -0.03f);
        }
    }
}
