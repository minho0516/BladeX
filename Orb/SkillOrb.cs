using DG.Tweening;
using Swift_Blade.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class SkillOrb : BaseOrb<SkillData>
    {
        [SerializeField] private SkillTable entireSkillTable;
        protected override IReadOnlyList<SkillData> GetReadonlyList => null;
        protected override void Start()
        {
            if (defaultItem == null)
            {
                int randomColor = Random.Range(0, 3);
                defaultItem = entireSkillTable.GetRandomSkill((ColorType)randomColor);
            }
            base.Start();
        }
        protected override TweenCallback CollectTweenCallback()
        {
            return
                () =>
                {
                    SkillManager.Instance.TryAddSkillToInventory(defaultItem);
                    Destroy(gameObject);
                };
        }
        protected override void Initialize()
        {
            if (itemRenderer != null)
            {
                itemRenderer.material = colors[(int)defaultItem.colorType];
            }
        }
    }
}
