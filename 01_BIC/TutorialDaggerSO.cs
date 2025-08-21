using Swift_Blade.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/DaggerTuto")]
    public class TutorialDaggerSO : TutorialWeaponSO
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private PoolPrefabMonoBehaviourSO blastParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO twingcleParticle;

        private Stack<TwingcleParticle> twingcles = new Stack<TwingcleParticle>();

        protected override void PlayParticle()
        {
            MonoGenericPool<MagicBlastParticle>.Initialize(blastParticle);
            MonoGenericPool<TwingcleParticle>.Initialize(twingcleParticle);

            MagicBlastParticle blast = MonoGenericPool<MagicBlastParticle>.Pop();
            blast.transform.SetParent(playerTransform);
            blast.transform.position = playerTransform.position + new Vector3(0, 0.25f, 0);

            TwingcleParticle twingcle = MonoGenericPool<TwingcleParticle>.Pop();
            twingcle.transform.SetParent(playerTransform);
            twingcle.transform.position = playerTransform.position + new Vector3(0, 0.25f, 0);

            twingcles.Push(twingcle);
        }

        protected override void StopParticle()
        {
            if (twingcles.Count <= 0) return;

            while (twingcles.Count != 0)
            {
                TwingcleParticle t = twingcles.Pop();

                if (t != null)
                    MonoGenericPool<TwingcleParticle>.Push(t);
            }

            twingcles.Clear();
        }
    }
}
