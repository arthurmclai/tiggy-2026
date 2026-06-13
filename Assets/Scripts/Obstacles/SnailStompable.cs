using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Obstacles
{
    public class SnailStompable : Stompable
    {
        [Tooltip("The shell GameObject to activate when stomped (sibling of this GameObject)")]
        public GameObject Shell;

        protected override void PerformStomp(CorgiController corgiController)
        {
            corgiController.SetForce(KnockbackForce);

            corgiController.gameObject.MMGetComponentNoAlloc<Character>()
                ?.FindAbility<CharacterJump>()
                ?.ResetJumpButtonReleased();

            if (Shell != null)
            {
                Shell.transform.position = transform.position;
                Shell.SetActive(true);
                Shell.GetComponent<Shell>()?.Activate();
            }

            gameObject.SetActive(false);
        }
    }
}
