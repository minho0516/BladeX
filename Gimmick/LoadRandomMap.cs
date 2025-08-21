using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class LoadRandomMap : MonoBehaviour
    {
        [SerializeField] private List<Transform> maps;

        private void OnEnable()
        {
            int randIdx = Random.Range(0, maps.Count);
            Instantiate(maps[randIdx]);
        }
    }
}
