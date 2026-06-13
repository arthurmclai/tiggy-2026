using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class SnailStompable : Stompable
    {
        protected override void PerformStomp(CorgiController corgiController)
        {
            corgiController.SetForce(KnockbackForce);

            corgiController.gameObject.MMGetComponentNoAlloc<Character>()
                ?.FindAbility<CharacterJump>()
                ?.ResetJumpButtonReleased();

            GetComponent<SnailBody>()?.EnterShell();
        }
    }
}
