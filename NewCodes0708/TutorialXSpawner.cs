using Swift_Blade.Enemy;
using Swift_Blade.Level;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialXSpawner : Spawner
    {
        protected override IEnumerator Spawn()
        {
            yield return null;
        }

        public void Ming(int idx)
        {
            BaseEnemy enemy = spawnEnemies[0].spawnInfos[idx].enemy;
            Vector3 pos = spawnEnemies[0].spawnInfos[idx].spawnPosition.position;
            

            var enem = Instantiate(enemy, pos, Quaternion.identity);
        }
    }
}
