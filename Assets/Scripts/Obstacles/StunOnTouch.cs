using MoreMountains.CorgiEngine;
using Tiggy.Characters;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class StunOnTouch : DamageOnTouch
    {
        public float StunDuration = 1f;
        [Tooltip("If true, a character set via SetImmune() will be given given a grace period")]
        public bool GiveGracePeriod = false;

        private Character _immuneCharacter;
        private float _immunityExpiry;

        public void SetGracePeriod(Character character, float duration)
        {
            _immuneCharacter = character;
            _immunityExpiry = Time.time + duration;
        }

        protected override void OnCollideWithDamageable(Health health)
        {
            if (GiveGracePeriod && _immuneCharacter != null)
            {
                if (Time.time >= _immunityExpiry)
                    _immuneCharacter = null;
                else if (health.GetComponent<Character>() == _immuneCharacter)
                    return;
            }

            base.OnCollideWithDamageable(health);

            if (!health.CanTakeDamageThisFrame())
                return;

            health.GetComponent<Character>()
                  ?.FindAbility<CharacterStunOnHit>()
                  ?.StunFor(StunDuration);
        }
    }
}
