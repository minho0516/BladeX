using Swift_Blade.Pool;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace Swift_Blade
{
    [DefaultExecutionOrder(200)]
    public class WeaponOrb : BaseOrb<WeaponSO>
    {
        [SerializeField] private WeaponSO[] weapons;
        [SerializeField] private WeaponOrbParticle weaponOrbParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO blastPrefab;
        protected override bool CanInteract => PlayerWeaponManager.CurrentWeapon != defaultItem;
        protected override IReadOnlyList<WeaponSO> GetReadonlyList => weapons;
        public override IPlayerEquipable GetEquipable => defaultItem;


        protected override void Awake()
        {
            MonoGenericPool<BlastParticle>.Initialize(blastPrefab);
            base.Awake();
        }
        protected override void Initialize()
        {
            WeaponSO currentPlayerWeapon = PlayerWeaponManager.CurrentWeapon;
            IReadOnlyList<WeaponSO> list = GetReadonlyList;
            int listCount = list.Count;
            if (listCount > 1)
            {
                if (defaultItem == null || (currentPlayerWeapon != null && defaultItem == currentPlayerWeapon))
                {
                    int randomIndex = Random.Range(0, listCount);
                    defaultItem = list[randomIndex];
                    if (currentPlayerWeapon != null && defaultItem == currentPlayerWeapon)
                    {
                        int randomIndexExcludingCurrentWeapon = (randomIndex + Random.Range(1, listCount)) % listCount;
                        defaultItem = list[randomIndexExcludingCurrentWeapon];
                        Debug.Assert(defaultItem != currentPlayerWeapon, "player weapon and randomItem is same");
                    }
                }
            }
            else if (defaultItem == null)
            {
                defaultItem = list[0];
            }
            weaponOrbParticle.SetWeapon(defaultItem);
        }
        protected override Tween InteractTween()
        {
            return transform.DOPunchScale(Vector3.one, 0.2f, -1, 0.5f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        private void SetOrbWeapon(WeaponSO weapon)
        {
            Vector3 originalScale = new Vector3(startFadeScale, startFadeScale, startFadeScale);
            transform.localScale = originalScale;
            defaultItem = weapon;
            weaponOrbParticle.SetWeapon(weapon);
            MonoGenericPool<BlastParticle>.Pop().transform.position = transform.position;
        }
        protected override void Interact()
        {
            PlayerWeaponManager playerWeaponManager = Player.Instance.GetEntityComponent<PlayerWeaponManager>();
            WeaponSO previousPlayerWeapon = PlayerWeaponManager.CurrentWeapon;

            playerWeaponManager.SetWeapon(defaultItem);

            SetOrbWeapon(previousPlayerWeapon);
            base.Interact();
        }
    }
}
