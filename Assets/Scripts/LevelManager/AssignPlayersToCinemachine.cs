using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Unity.Cinemachine;
using UnityEngine;

public class AssignPlayersToCinemachine : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public CinemachineTargetGroup targetGroup;

    private void Start()
    {
        targetGroup = FindAnyObjectByType<CinemachineTargetGroup>();
    }

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
            targetGroup.AddMember(players[i].transform, 1f, 2f);
        }
    }
}
