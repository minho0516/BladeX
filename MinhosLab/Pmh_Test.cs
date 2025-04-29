using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Swift_Blade
{
    public class Pmh_Test : MonoBehaviour
    {
        [SerializeField] private List<Transform> planets = new List<Transform>();
        [SerializeField] private List<float> forSolaDistance = new List<float>();
        [SerializeField] private List<float> forSolaSpinSpeed = new List<float>();

        [SerializeField] private float systemSpeed = 10.0f;

        [SerializeField] float myAngle = 0;
        
        private void Update()
        {
            if(myAngle  >= 360)
            {
                //myAngle = 0;
            }

            myAngle += Time.deltaTime * systemSpeed;

            for (int i = 0; i < planets.Count; i++)
            {
                float dist = forSolaDistance[i];
                float speed = forSolaSpinSpeed[i];

                float spinAngle = myAngle / speed;

                Vector3 anglePosAngle = new Vector3(Mathf.Cos(spinAngle), 0, Mathf.Sin(spinAngle));
                anglePosAngle *= dist;

                planets[i].transform.localPosition = anglePosAngle;
            }
        }
    }
}
