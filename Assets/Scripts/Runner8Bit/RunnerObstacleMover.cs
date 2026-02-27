using UnityEngine;

namespace Runner8Bit
{
    public class RunnerObstacleMover : MonoBehaviour
    {
        private void Update()
        {
            var manager = RunnerGameManager.Instance;
            if (manager == null || !manager.IsPlaying) return;

            transform.Translate(Vector3.left * manager.WorldSpeed() * Time.deltaTime, Space.World);

            if (transform.position.x < -20f)
            {
                Destroy(gameObject);
            }
        }
    }
}
