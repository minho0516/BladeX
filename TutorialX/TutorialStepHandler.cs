using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Swift_Blade
{
    public abstract class TutorialStepHandler : ScriptableObject
    {
        [field: SerializeField] public List<LocalizedString> stepDescriptionCollection;

        public static Action clearStepEvent;
        public abstract void Enter(ShinTutorialSystem system);
        public abstract void Update();
        public abstract bool IsCompleted();
    }
}
