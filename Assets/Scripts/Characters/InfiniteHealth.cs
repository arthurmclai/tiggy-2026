using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Characters
{
    public class InfiniteHealth : Health
    {
        public override void Kill()
        {
            SetHealth(InitialHealth, this.gameObject);
        }
    }
}
