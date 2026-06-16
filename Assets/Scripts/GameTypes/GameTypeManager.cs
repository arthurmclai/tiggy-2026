using System.Collections.Generic;
using System.Linq;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Tiggy.Roles;
using UnityEngine;

namespace Tiggy.GameTypes
{
    public abstract class GameTypeManager : MonoBehaviour, MMEventListener<CorgiEngineEvent>
    {
        [Tooltip("How long after being tagged before a player can be tagged again.")]
        public float TagGracePeriod = 0.5f;

        private Dictionary<Character, PlayerRole> _roles = new Dictionary<Character, PlayerRole>();
        private Dictionary<Character, Tagger> _taggers = new Dictionary<Character, Tagger>();

        protected virtual void OnEnable() => this.MMEventStartListening<CorgiEngineEvent>();
        protected virtual void OnDisable() => this.MMEventStopListening<CorgiEngineEvent>();

        public virtual void OnMMEvent(CorgiEngineEvent e)
        {
            if (e.EventType != CorgiEngineEventTypes.LevelStart) return;

            foreach (var player in LevelManager.Instance.Players)
            {
                var playerRole = player.GetComponent<PlayerRole>();
                if (playerRole != null)
                    _roles[player] = playerRole;

                var tagger = player.GetComponent<Tagger>();
                if (tagger != null)
                    _taggers[player] = tagger;
            }

            OnLevelStart();
        }

        protected virtual void OnLevelStart() { }

        protected IEnumerable<Character> AllPlayers => _roles.Keys;

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

        protected void ApplyTagGracePeriod(Character tagger, Character tagged)
        {
            // Set on the newly-It player so they can't immediately tag back in the same physics frame
            if (_taggers.TryGetValue(tagged, out var taggedTagger))
                taggedTagger.StartGracePeriod(tagger, TagGracePeriod);
        }

        protected abstract IEnumerable<RoleEffect> EffectsForRole(Role role);
    }
}
