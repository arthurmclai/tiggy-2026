using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class ShellStompable : Stompable
    {
        private Shell _shell;

        protected override void Start()
        {
            base.Start();
            _shell = GetComponent<Shell>();
        }

        protected override void PerformStomp(CorgiController corgiController)
        {
            corgiController.SetForce(KnockbackForce);
            corgiController.gameObject.MMGetComponentNoAlloc<Character>()
                ?.FindAbility<CharacterJump>()
                ?.ResetJumpButtonReleased();

            _shell.HandleStomp(corgiController);
        }
    }
}
