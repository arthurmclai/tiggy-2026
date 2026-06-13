using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Unity.Cinemachine;
using UnityEngine;

namespace Tiggy.Level
{
    public class ConfineCinemachineToBounds : MonoBehaviour, MMEventListener<CorgiEngineEvent>
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;

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
            CinemachineConfiner2D confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
            confiner.BoundingShape2D = MultiplayerLevelManager.Current.GetComponent<BoxCollider2D>();
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
