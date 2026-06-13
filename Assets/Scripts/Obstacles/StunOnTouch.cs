using MoreMountains.CorgiEngine;
using Tiggy.Characters;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class StunOnTouch : DamageOnTouch
    {
        public float StunDuration = 1f;

        protected override void OnCollideWithDamageable(Health health)
        {
            base.OnCollideWithDamageable(health);

            if (!health.CanTakeDamageThisFrame())
                return;

            health.GetComponent<Character>()
                  ?.FindAbility<CharacterStunOnHit>()
                  ?.StunFor(StunDuration);
        }
    }
}
