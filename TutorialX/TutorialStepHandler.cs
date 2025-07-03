using System;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class TutorialStepHandler : ScriptableObject
    {
        public static Action clearStepEvent;
        public abstract void Enter(ShinTutorialSystem system);
        public abstract void Update();
        public abstract bool IsCompleted();
    }
}
