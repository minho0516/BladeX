using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public class ItemOrb : BaseOrb<ItemDataSO>
    {
        [SerializeField] private ItemTableSO entireItemTable;
        protected override bool CanInteract => base.CanInteract && !InventoryManager.Instance.IsAllSlotsFull();
        protected override IReadOnlyList<ItemDataSO> GetReadonlyList => entireItemTable.GetItemDataSO();
        public override IPlayerEquipable GetEquipable => defaultItem.equipmentData;
        [SerializeField] private Material _defaultWhiteColor;

        protected override TweenCallback CollectTweenCallback()
        { 
            return
                () =>
                {
                    InventoryManager.Instance.AddItemToEmptySlot(defaultItem);
                    Destroy(gameObject);
                };
        }
        protected override void Initialize()
        {
            if (itemRenderer != null)
            {
                itemRenderer.material = defaultItem.equipmentData.colorType == ColorType.None ? _defaultWhiteColor: colors[(int)defaultItem.equipmentData.colorType];
            }
        }
    }
}
