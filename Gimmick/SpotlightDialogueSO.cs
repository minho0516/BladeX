using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "DE_Dialog_Spotlight", menuName = "SO/Dialog/Events/Dialog_Spotlight")]
    public class SpotlightDialogueSO : DialogueEventSO
    {
        public override void InvokeEvent()
        {
            MinimapSpotlight.Instance.SetSpotlight();
        }
    }
}
