using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "TutorialStatus", menuName = "SO/TutorialStatusSO")]
    public class TutorialStatusSO : ScriptableObject
    {
        public string Description => description2.GetLocalizedString(); // 우측 설명
        public LocalizedString description2;
        public DialogueDataSO dialogue; // NPC 대사
        public TutorialStepHandler stepLogic;
        public GameObject KeyGuideObj;
        public ItemDataSO itemReward;
        public GameObject tutorialStepMap;
    }
}
