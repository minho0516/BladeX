using DG.Tweening;
using Swift_Blade.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class SkillOrb : BaseOrb<SkillData>
    {
        [SerializeField] private SkillTable entireSkillTable;
        //protected override bool CanInteract => base.CanInteract && SkillManager.Instance.CanAddSkillInventory;
        protected override IReadOnlyList<SkillData> GetReadonlyList => null;
        public override IPlayerEquipable GetEquipable => defaultItem;

        protected override TweenCallback CollectTweenCallback()
        {
            return
                () =>
                {
                    //SkillManager.Instance.TryAddSkillToInventory(defaultItem);
                    Destroy(gameObject);
                };
        }
        protected override void Initialize()
        {
            Debug.Assert(itemRenderer != null, "item Renderer is null");
            if (defaultItem == null)
            {
                int randomColor = Random.Range(0, 3);
                defaultItem = entireSkillTable.GetRandomSkill((ColorType)randomColor);
            }
            itemRenderer.material = colors[(int)defaultItem.colorType];

        }
    }
}
