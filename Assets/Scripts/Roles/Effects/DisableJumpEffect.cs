using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    [CreateAssetMenu(menuName = "Tiggy/Role Effects/Disable Jump")]
    public class DisableJumpEffect : RoleEffect
    {
        public override void Apply(Character character)
        {
            character.FindAbility<CharacterJump>()?.PermitAbility(false);
        }

        public override void Revert(Character character)
        {
            character.FindAbility<CharacterJump>()?.PermitAbility(true);
        }
    }
}
