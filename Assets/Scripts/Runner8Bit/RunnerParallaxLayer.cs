using UnityEngine;

namespace Runner8Bit
{
    public class RunnerParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float speedMultiplier = 0.2f;
        [SerializeField] private float wrapAtX = -22f;
        [SerializeField] private float resetToX = 22f;

        public void Configure(float speed, float wrapX, float resetX)
        {
            speedMultiplier = speed;
            wrapAtX = wrapX;
            resetToX = resetX;
        }

        private void Update()
        {
            var manager = RunnerGameManager.Instance;
            var baseSpeed = manager != null ? manager.WorldSpeed() : 5f;
            transform.Translate(Vector3.left * baseSpeed * speedMultiplier * Time.deltaTime, Space.World);

            if (transform.position.x <= wrapAtX)
            {
                var pos = transform.position;
                pos.x = resetToX;
                transform.position = pos;
            }
        }
    }
}
