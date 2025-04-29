using Swift_Blade.Pool;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace Swift_Blade
{
    public class WeaponOrb : BaseOrb<WeaponSO>
    {
        [SerializeField] private WeaponSO[] weapons;
        [SerializeField] private WeaponOrbParticle weaponOrbParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO blastPrefab;
        protected override bool CanInteract => PlayerWeaponManager.CurrentWeapon != defaultItem;
        protected override IReadOnlyList<WeaponSO> GetReadonlyList => weapons;

        protected override void Awake()
        {
            MonoGenericPool<BlastParticle>.Initialize(blastPrefab);
            base.Awake();
        }
        protected override void Initialize()
        {
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
