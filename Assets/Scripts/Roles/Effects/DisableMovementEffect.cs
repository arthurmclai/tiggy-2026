using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    [CreateAssetMenu(menuName = "Tiggy/Role Effects/Disable Movement")]
    public class DisableMovementEffect : RoleEffect
    {
        public override void Apply(Character character)
        {
            character.FindAbility<CharacterHorizontalMovement>()?.PermitAbility(false);
        }

        public override void Revert(Character character)
        {
            character.FindAbility<CharacterHorizontalMovement>()?.PermitAbility(true);
        }
    }
}
