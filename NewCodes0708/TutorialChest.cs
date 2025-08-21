using Swift_Blade.Level;

namespace Swift_Blade
{
    public class TutorialChest : Chest
    {
        public bool IsPotionChest = false;
        private void Start()
        {
            SetRandomChestType(100,0,0);
        }

        protected override void OpenChest()
        {
            base.OpenChest();
            if (InventoryStep.Instance != null)
            {
                InventoryStep.Instance.OpenChestCallback();
            }
            
            //InventoryStepImage.Instance.ChestIsOpened();
        }
    }
}
