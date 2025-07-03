using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SpEnemyDialogueEvent", menuName = "SO/Dialog/Events/DEnemySpawner")]
    public class SpEnemyDialogueEvent : DialogueEventSO
    {
        public override void InvokeEvent()
        {
            Debug.Log("Invoke Dialogue End Event");
            TutorialSpawner.EnemySpawnerEvent?.Invoke();
        }
    }
}
