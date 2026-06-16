using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Tiggy.Roles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tiggy.GameTypes
{
    public class TiggyGameTypeManager : GameTypeManager, MMEventListener<TagEvent>
    {
        [SerializeField] private RoleEffect[] _itEffects;
        [SerializeField] private RoleEffect[] _freeEffects;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.MMEventStartListening<TagEvent>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.MMEventStopListening<TagEvent>();
        }

        protected override void OnLevelStart()
        {
            foreach (var player in AllPlayers)
                SetRole(player, Role.Free, EffectsForRole(Role.Free));

            var players = new List<Character>(AllPlayers);
            var initialIt = players[Random.Range(0, players.Count)];
            SetRole(initialIt, Role.It, EffectsForRole(Role.It));
        }

        public void OnMMEvent(TagEvent e)
        {
            ApplyTagGracePeriod(e.Tagger, e.Tagged);
            SetRole(e.Tagger, Role.Free, EffectsForRole(Role.Free));
            SetRole(e.Tagged, Role.It, EffectsForRole(Role.It));
        }

        protected override IEnumerable<RoleEffect> EffectsForRole(Role role)
        {
            return role == Role.It ? _itEffects : _freeEffects;
        }
    }
}
