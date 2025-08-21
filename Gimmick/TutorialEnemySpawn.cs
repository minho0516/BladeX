using Swift_Blade.Enemy;
using Swift_Blade.Level;
using Swift_Blade.Pool;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Swift_Blade
{
    public class TutorialEnemySpawn : Spawner
    {
        [SerializeField] private Transform WallTrm;
        [SerializeField] private Door door;

        private int enemyCount;
        private int enemyCounter;

        bool isActive = false;

        [SerializeField] private DialogueDataSO endingDialogue;
        [SerializeField] private TutorialNPC tutorialNPC;

        protected override void Start()
        {
            InitializeParticle();

            MonoGenericPool<DustUpParticle>.Initialize(doorSpawnParticle);
            doorSpawnDelay = new WaitForSeconds(dustParticleDelay);
        }

        protected override IEnumerator Spawn()
        {
            if (waveCount >= spawnEnemies.Count)
            {
                Debug.LogError("Enemy count exceeded. This shouldn't happen.");
            }
            else
            {
                var currentSpawnInfo = spawnEnemies[waveCount++].spawnInfos;
                enemyCount = currentSpawnInfo.Length;
                
                for (int i = 0; i < currentSpawnInfo.Length; i++)
                {
                    yield return new WaitForSeconds(currentSpawnInfo[i].delay);

                    var newEnemy = Instantiate(currentSpawnInfo[i].enemy,
                        currentSpawnInfo[i].spawnPosition.position,
                        Quaternion.identity);

                    newEnemy.GetHealth().OnDeadEvent.AddListener(TryNextEnemyCanSpawn);

                    PlaySpawnParticle(newEnemy.transform.position);
                }
            }
        }
        private void TryNextEnemyCanSpawn()
        {
            if (isClear) return;

            ++enemyCounter;

            var isCurrentWaveClear = enemyCounter >= enemyCount;
            if (isCurrentWaveClear)
            {
                isClear = waveCount >= spawnEnemies.Count;
                StartCoroutine(isClear ? LevelClear() : Spawn());
            }
        }
        
        protected override void CreateDoor(Node node, Vector3 doorPosition)
        {
            Debug.Log("123213213123213");
            tutorialNPC.CallInteractive(endingDialogue);
            door.UpDoor();
        }
        
        private void SetWall(bool isActive)
        {
            WallTrm.gameObject.SetActive(isActive);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (isActive) return;
                
                isActive = true;
                SetWall(isActive);
                StartCoroutine(Spawn());
            }
        }

        
    }
}
