using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Modifiers
{
    [CreateAssetMenu(menuName = "Tiggy/Powers/Gravity Flip")]
    public class GravityFlipPower : Power
    {
        public override void Apply(Character character)
        {
            character.FindAbility<CharacterGravity>()?.SetGravityAngle(180f);
        }

        public override void Revert(Character character)
        {
            character.FindAbility<CharacterGravity>()?.ResetGravityToDefault();
        }
    }
}
