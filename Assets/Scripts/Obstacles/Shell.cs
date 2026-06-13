using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class Shell : MonoBehaviour
    {
        [Tooltip("The snail body GameObject to re-enable after the respawn timer expires")]
        public GameObject SnailBody;
        [Tooltip("Speed applied to the shell when kicked by a player")]
        public float SlideSpeed = 8f;
        [Tooltip("How quickly the shell decelerates (units/s²)")]
        public float Deceleration = 4f;
        [Tooltip("Seconds in the Shelled (stopped) state before respawning the snail")]
        public float RespawnDelay = 3f;
        [Tooltip("Seconds the kicker is immune to the shell's stun after kicking")]
        public float KickGracePeriod = 0.3f;

        private CorgiController _controller;
        private StunOnTouch _stunOnTouch;

        private enum ShellState { Shelled, Sliding }
        private ShellState _state;
        private float _slideDirection;
        private float _slideSpeed;
        private float _respawnTimer;

        protected virtual void Awake()
        {
            _controller = GetComponent<CorgiController>();
            _stunOnTouch = GetComponent<StunOnTouch>();
        }

        protected virtual void OnEnable()
        {
            _controller.SetForce(Vector2.zero);
        }

        public void Activate()
        {
            EnterShelled();
        }

        public void HandleStomp(CorgiController stomperController)
        {
            Character stomper = stomperController.gameObject.MMGetComponentNoAlloc<Character>();
            if (_state == ShellState.Sliding)
                EnterShelled();
            else if (_state == ShellState.Shelled)
                StartSliding(stomperController.transform.position.x > transform.position.x ? -1f : 1f, stomper);
        }

        protected virtual void Update()
        {
            if (_state == ShellState.Shelled)
            {
                _respawnTimer -= Time.deltaTime;
                if (_respawnTimer <= 0f)
                    Respawn();
                return;
            }

            if (_controller.State.IsCollidingLeft && _slideDirection < 0f)
                _slideDirection = 1f;
            else if (_controller.State.IsCollidingRight && _slideDirection > 0f)
                _slideDirection = -1f;

            _slideSpeed = Mathf.MoveTowards(_slideSpeed, 0f, Deceleration * Time.deltaTime);
            _controller.SetHorizontalForce(_slideDirection * _slideSpeed);

            if (_slideSpeed < 0.1f)
                EnterShelled();
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (_state != ShellState.Shelled)
                return;

            Character kicker = other.gameObject.MMGetComponentNoAlloc<Character>();
            if (kicker == null)
                return;

            float direction = other.transform.position.x > transform.position.x ? -1f : 1f;
            StartSliding(direction, kicker);
        }

        private void EnterShelled()
        {
            _state = ShellState.Shelled;
            _slideSpeed = 0f;
            _stunOnTouch.enabled = false;
            _controller.SetForce(Vector2.zero);
            _respawnTimer = RespawnDelay;
        }

        private void StartSliding(float directionX, Character kicker)
        {
            _state = ShellState.Sliding;
            _slideDirection = directionX;
            _slideSpeed = SlideSpeed;
            _stunOnTouch.enabled = true;
            _stunOnTouch.SetGracePeriod(kicker, KickGracePeriod);
            _controller.SetForce(new Vector2(directionX * SlideSpeed, 0f));
        }

        private void Respawn()
        {
            if (SnailBody != null)
            {
                SnailBody.transform.position = transform.position;
                SnailBody.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
