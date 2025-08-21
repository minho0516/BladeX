using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialHealPotion : ItemObject
    {
        [SerializeField] private float increaseAmount;

        public override void ItemEffect(Player player)
        {
            Debug.Log("포션을 먹기를 시도하다");

            if(TutorialInventoryStepMap.Instance != null)
            {
                Debug.Log($"of 포션, 튜토리얼인벤토리맵켜져있냐? : {TutorialInventoryStepMap.Instance != null}");
                if (increaseAmount == 3)
                {
                    //여기 인벤토리 스텝일떄만 실행
                    InventoryStep.Instance.CallUseLargePotion();
                }
                else if (increaseAmount == 1)
                {
                    InventoryStep.Instance.CallUseRagullarPotion();
                }
            }
            
            player.GetEntityComponent<PlayerHealth>().TakeHeal(increaseAmount);
        }

        public override bool CanUse()
        {
            if (InventoryStep.Instance != null)
            {
                if (InventoryStep.Instance.CanUsePotion == false) return false;
            }

            float maxHp = Player.Instance.GetEntityComponent<PlayerHealth>().maxHealth;

            bool isPlayerFullHP = Mathf.Approximately(maxHp, PlayerHealth.CurrentHealth);

            if (isPlayerFullHP)
            {
                string message = LocalizationManager.GetPopupString("HealthFull");
                PopupManager.Instance.LogInfoBox(message);
                return false;
            }

            return true;
        }
    }
}
