using UnityEngine;

namespace Swift_Blade
{
    public class TutorialDestruc : DestructibleObject
    {
        protected override void TutorialDestroyed(GameObject createdObject)
        {
            //WeaponStep.IsBreakabled = true;
;           Destroy(createdObject, 1f); // 1�� �Ŀ� ������Ʈ ����
        }
    }
}
