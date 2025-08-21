using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SpEnemyDialogueEvent", menuName = "SO/Dialog/Events/DTutorialEnd")]
    public class TutorialDialogueEndEvent : DialogueEventSO
    {
        public override void InvokeEvent()
        {
            Player.Instance.GetPlayerMovement.AllowInputMove = true;
        }
    }
}
