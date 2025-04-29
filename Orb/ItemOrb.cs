using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public class ItemOrb : BaseOrb<ItemDataSO>
    {
        [SerializeField] private ItemTableSO entireItemTable;
        private static List<ItemDataSO> cache;
        protected override IReadOnlyList<ItemDataSO> GetReadonlyList => cache ?? (cache = entireItemTable.ToItemDataSOList());
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
                itemRenderer.material = colors[(int)defaultItem.equipmentData.colorType];
            }
        }
    }
}
