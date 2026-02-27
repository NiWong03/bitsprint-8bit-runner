using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Runner8Bit
{
    public class RunnerGameManager : MonoBehaviour
    {
        public static RunnerGameManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private RunnerSpawner spawner;
        [SerializeField] private RunnerPlayer player;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text centerText;
        [SerializeField] private Text difficultyText;
        [SerializeField] private Text highScoreText;

        private readonly Dictionary<RunnerDifficulty, DifficultySettings> difficultyMap = new Dictionary<RunnerDifficulty, DifficultySettings>()
        {
            { RunnerDifficulty.Easy, new DifficultySettings(5f, 1.7f, 1f) },
            { RunnerDifficulty.Medium, new DifficultySettings(7f, 1.2f, 1.5f) },
            { RunnerDifficulty.Hard, new DifficultySettings(9f, 0.85f, 2f) }
        };

        private DifficultySettings current;
        private bool started;
        private bool gameOver;
        private bool initialized;
        private float score;
        private int highScore;

        private const string HighScoreKey = "Runner8Bit_HighScore";

        public bool IsPlaying => started && !gameOver;
        public RunnerDifficulty CurrentDifficulty { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
            EnsureInitialized();
            if (initialized)
            {
                ShowMenu();
                return;
            }

            StartCoroutine(DeferredInit());
        }

        private void Update()
        {
            if (!initialized) return;

            if (!started)
            {
                if (IsDigitPressed(1)) StartRun(RunnerDifficulty.Easy);
                if (IsDigitPressed(2)) StartRun(RunnerDifficulty.Medium);
                if (IsDigitPressed(3)) StartRun(RunnerDifficulty.Hard);
                return;
            }

            if (gameOver)
            {
                if (IsRestartPressed())
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                }
                return;
            }

            score += Time.deltaTime * current.ScoreMultiplier * 10f;
            UpdateScoreUi();
        }

        public float WorldSpeed() => current.WorldSpeed;
        public float SpawnInterval() => current.SpawnInterval;

        public void Lose(string reason)
        {
            if (!initialized) return;
            if (gameOver) return;

            gameOver = true;
            var scoreInt = (int)score;
            if (scoreInt > highScore)
            {
                highScore = scoreInt;
                PlayerPrefs.SetInt(HighScoreKey, highScore);
                PlayerPrefs.Save();
            }
            UpdateHighScoreUi();
            centerText.text = $"GAME OVER\n{reason}\nSCORE: {(int)score}\nPress R to restart";
            centerText.enabled = true;
            spawner.enabled = false;
            player.enabled = false;
        }

        private void ShowMenu()
        {
            if (!initialized) return;
            started = false;
            gameOver = false;
            score = 0;
            UpdateScoreUi();
            UpdateHighScoreUi();
            difficultyText.text = "DIFFICULTY: -";
            centerText.text = "8-BIT RUNNER\n1 = EASY  2 = MEDIUM  3 = HARD\nSPACE/UP = JUMP\nSURVIVE AND BEAT HI-SCORE";
            centerText.enabled = true;
        }

        private void StartRun(RunnerDifficulty difficulty)
        {
            if (!initialized) return;
            CurrentDifficulty = difficulty;
            current = difficultyMap[difficulty];
            started = true;
            gameOver = false;
            centerText.enabled = false;
            difficultyText.text = $"DIFFICULTY: {difficulty.ToString().ToUpper()}";
            spawner.enabled = true;
            player.enabled = true;
        }

        public void Configure(RunnerSpawner runnerSpawner, RunnerPlayer runnerPlayer, Text scoreUi, Text centerUi, Text difficultyUi, Text highScoreUi = null)
        {
            spawner = runnerSpawner;
            player = runnerPlayer;
            scoreText = scoreUi;
            centerText = centerUi;
            difficultyText = difficultyUi;
            highScoreText = highScoreUi;

            EnsureInitialized();
            if (initialized) ShowMenu();
        }

        private void EnsureInitialized()
        {
            if (initialized) return;

            if (spawner == null) spawner = FindFirstObjectByType<RunnerSpawner>();
            if (player == null) player = FindFirstObjectByType<RunnerPlayer>();
            if (scoreText == null || centerText == null || difficultyText == null)
            {
                var texts = FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var t in texts)
                {
                    if (t == null || string.IsNullOrEmpty(t.text)) continue;
                    if (scoreText == null && t.text.StartsWith("SCORE")) scoreText = t;
                    if (difficultyText == null && t.text.StartsWith("DIFFICULTY")) difficultyText = t;
                    if (highScoreText == null && t.text.StartsWith("HI:")) highScoreText = t;
                    if (centerText == null && t.text.Contains("8-BIT RUNNER")) centerText = t;
                }
            }

            initialized = spawner != null && player != null && scoreText != null && centerText != null && difficultyText != null;
            if (!initialized) return;

            spawner.enabled = false;
            player.enabled = false;
        }

        private IEnumerator DeferredInit()
        {
            // Allow one frame for bootstrap/configure ordering differences.
            yield return null;
            EnsureInitialized();
            if (initialized)
            {
                ShowMenu();
                yield break;
            }

            Debug.LogWarning("RunnerGameManager is missing required references. Ensure RunnerBootstrap is in the scene.");
        }

        private void UpdateScoreUi()
        {
            if (scoreText != null) scoreText.text = $"SCORE: {(int)score}";
        }

        private void UpdateHighScoreUi()
        {
            if (highScoreText != null) highScoreText.text = $"HI: {highScore}";
        }

        private static bool IsDigitPressed(int digit)
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard == null) return false;
            if (digit == 1) return keyboard.digit1Key.wasPressedThisFrame;
            if (digit == 2) return keyboard.digit2Key.wasPressedThisFrame;
            if (digit == 3) return keyboard.digit3Key.wasPressedThisFrame;
            return false;
#else
            if (digit == 1) return Input.GetKeyDown(KeyCode.Alpha1);
            if (digit == 2) return Input.GetKeyDown(KeyCode.Alpha2);
            if (digit == 3) return Input.GetKeyDown(KeyCode.Alpha3);
            return false;
#endif
        }

        private static bool IsRestartPressed()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            return keyboard != null && keyboard.rKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.R);
#endif
        }
    }
}
