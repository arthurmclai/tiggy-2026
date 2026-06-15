using System.Collections.Generic;
using System.Linq;
using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    public abstract class GameTypeManager : MonoBehaviour
    {
        private Dictionary<Character, PlayerRole> _roles = new Dictionary<Character, PlayerRole>();

        protected virtual void Start()
        {
            foreach (var player in LevelManager.Instance.Players)
            {
                var playerRole = player.GetComponent<PlayerRole>();
                if (playerRole != null)
                    _roles[player] = playerRole;
            }
        }

        public Role GetRole(Character character)
        {
            return _roles.TryGetValue(character, out var playerRole) ? playerRole.CurrentRole : Role.None;
        }

        public List<Character> GetPlayersWithRole(Role role)
        {
            return _roles
                .Where(pair => pair.Value.CurrentRole == role)
                .Select(pair => pair.Key)
                .ToList();
        }

        protected void SetRole(Character character, Role role, IEnumerable<RoleEffect> effects = null)
        {
            if (_roles.TryGetValue(character, out var playerRole))
                playerRole.SetRole(role, effects);
        }

        protected abstract IEnumerable<RoleEffect> EffectsForRole(Role role);
    }
}
