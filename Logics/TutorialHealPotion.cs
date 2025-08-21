using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialHealPotion : ItemObject
    {
        [SerializeField] private float increaseAmount;

        public override void ItemEffect(Player player)
        {
            Debug.Log("������ �Ա⸦ �õ��ϴ�");

            if(TutorialInventoryStepMap.Instance != null)
            {
                Debug.Log($"of ����, Ʃ�丮���κ��丮�������ֳ�? : {TutorialInventoryStepMap.Instance != null}");
                if (increaseAmount == 3)
                {
                    //���� �κ��丮 �����ϋ��� ����
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
