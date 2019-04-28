using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    [RequireComponent(typeof(Collider))]
    public class BloodMissionPowerUp : MonoBehaviour
    {
        public delegate void GotPowerUp(BloodMissionPowerUp powerup);
        public static event GotPowerUp OnTrigger;
        
        public bool IsFinal = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                OnTrigger?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
