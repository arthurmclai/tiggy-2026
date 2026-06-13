using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class SnailBody : MonoBehaviour
    {
        public GameObject Shell;
        [Tooltip("Objects on these layers will trigger the shell transition on contact")]
        public LayerMask ObstacleMask;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (ObstacleMask.value == 0) return;
            if ((ObstacleMask.value & (1 << other.gameObject.layer)) == 0) return;

            // kick shell away from the obstacle
            float direction = other.transform.position.x < transform.position.x ? 1f : -1f;
            EnterShell(direction);
        }

        // Called by SnailStompable — shell enters stationary Shelled state
        public void EnterShell()
        {
            if (Shell == null) return;
            Shell.transform.position = transform.position;
            Shell.SetActive(true);
            Shell.GetComponent<Shell>()?.Activate();
            gameObject.SetActive(false);
        }

        // Called by obstacle contact — shell immediately slides in the given direction
        private void EnterShell(float kickDirection)
        {
            if (Shell == null) return;
            Shell.transform.position = transform.position;
            Shell.GetComponent<Shell>()?.ActivateAndKick(kickDirection);
            gameObject.SetActive(false);
        }
    }
}
