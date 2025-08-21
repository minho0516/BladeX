using UnityEngine;

namespace Swift_Blade
{
    public class BreakableItemGenerator : MonoBehaviour
    {
        [SerializeField] private ItemOrb itemOrb;
        [SerializeField, Range(0f, 100f)]
        private float probability;
        private void OnDestroy()
        {
            float rand = Random.Range(0.0f, 100.0f);

            if (rand <= probability)
            {
                Instantiate(itemOrb, transform.position, Quaternion.identity);
            }
        }
    }
}
