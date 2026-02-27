namespace Runner8Bit
{
    public enum RunnerDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    public readonly struct DifficultySettings
    {
        public readonly float WorldSpeed;
        public readonly float SpawnInterval;
        public readonly float ScoreMultiplier;

        public DifficultySettings(float worldSpeed, float spawnInterval, float scoreMultiplier)
        {
            WorldSpeed = worldSpeed;
            SpawnInterval = spawnInterval;
            ScoreMultiplier = scoreMultiplier;
        }
    }
}
