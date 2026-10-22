using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Modifiers
{
    [CreateAssetMenu(menuName = "Tiggy/Areas/All Players")]
    public class AllPlayersArea : Area
    {
        public override IEnumerable<Character> GetAffected(Character picker)
        {
            return LevelManager.Instance.Players;
        }
    }
}
