using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Characters
{
    public class CharacterStunOnHit : CharacterStun
    {
        public override void Stun()
        {
            if (_condition.CurrentState != CharacterStates.CharacterConditions.Stunned)
                _previousCondition = _condition.CurrentState;

            _condition.ChangeState(CharacterStates.CharacterConditions.Stunned);
            // intentionally not zeroing force — knockback carries naturally, ProcessAbility decelerates it
            AbilityStartFeedbacks?.PlayFeedbacks();
            MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.Stun, MMCharacterEvent.Moments.Start);
        }

        public override void ProcessAbility()
        {
            if (_condition.CurrentState != CharacterStates.CharacterConditions.Stunned)
                return;

            // apply natural deceleration while stunned so knockback bleeds off via physics
            float deceleration = _controller.State.IsGrounded
                ? _controller.Parameters.SpeedDecelerationOnGround
                : _controller.Parameters.SpeedDecelerationInAir;

            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Speed.x, 0f, Time.deltaTime * deceleration));
        }
    }
}
