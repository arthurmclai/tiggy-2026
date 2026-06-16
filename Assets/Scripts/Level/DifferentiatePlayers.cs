using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Tiggy.Roles;
using UnityEngine;
using UnityEngine.UI;

namespace Tiggy.Level
{
    public class DifferentiatePlayers : MonoBehaviour, MMEventListener<CorgiEngineEvent>
    {
        [SerializeField] private Color[] _playerColors = new Color[]
        {
            new Color(1f, 0.4f, 0.4f),
            new Color(0.7f, 1f, 0.4f),
            new Color(0.4f, 0.7f, 1f),
            new Color(1f, 1f, 0.4f),
        };

        [Tooltip("PlayerName Text labels in player order (Player1, Player2, ...). Leave empty slots null for inactive players.")]
        [SerializeField] private Text[] _playerNameLabels;

        private void OnEnable()
        {
            this.MMEventStartListening<CorgiEngineEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<CorgiEngineEvent>();
        }

        public void OnMMEvent(CorgiEngineEvent e)
        {
            if (e.EventType != CorgiEngineEventTypes.LevelStart)
                return;

            var players = LevelManager.Instance.Players;
            for (int i = 0; i < players.Count; i++)
            {
                Color color = _playerColors[i % _playerColors.Length];
                foreach (var sr in players[i].GetComponentsInChildren<SpriteRenderer>())
                    sr.color = color;
                if (_playerNameLabels != null && i < _playerNameLabels.Length && _playerNameLabels[i] != null)
                    _playerNameLabels[i].color = color;
                var markerConfig = players[i].GetComponent<MarkerConfiguration>();
                if (markerConfig != null)
                    markerConfig.PlayerColor = color;
            }
        }
    }
}
