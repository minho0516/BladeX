using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/NormalSwordTuto")]
    public class TutorialWeaponSO : WeaponSO
    {
        public override Action GetSpecialBehaviour(Player entity)
        {
            Action result = default;

            if (playerTransform == null)
                playerTransform = entity.GetPlayerTransform;

            switch (ColorType)
            {
                case ColorType.RED:
                    result = () =>
                    {
                        if(WeaponStep.Instance != null)
                        {
                            WeaponStep.Instance.UseRedWeaponAbil();
                        }

                        entity.GetStateMachine.ChangeState(PlayerStateEnum.Parry);

                    };
                    break;
                case ColorType.GREEN:
                    result = () =>
                    {
                        if (WeaponStep.Instance != null)
                        {
                            WeaponStep.Instance.UseGreenWeaponAbil();
                        }

                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.HEALTH,
                            nameof(StatType.HEALTH), 5, 3,
                            PlayParticle, StopParticle);

                        entity.GetSkillController.UseSkill(SkillType.Shield);
                    };
                    break;
                case ColorType.BLUE:
                    result = () =>
                    {
                        if (WeaponStep.Instance != null)
                        {
                            WeaponStep.Instance.UseBlueWeaponAbil();
                        }

                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.ATTACKSPEED, nameof(StatType.ATTACKSPEED), 4, 2.5f, PlayParticle, StopParticle);
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.MOVESPEED, nameof(StatType.MOVESPEED), 4, 2.5f);

                        entity.GetSkillController.UseSkill(SkillType.SpeedUp);
                    };
                    break;
                default:
                    throw new NotImplementedException($"color type {ColorType} is not implemented");
            }
            return result;
        }
    }
}
