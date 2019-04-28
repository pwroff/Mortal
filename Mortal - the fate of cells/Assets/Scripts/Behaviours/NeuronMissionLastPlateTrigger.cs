using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    public class NeuronMissionLastPlateTrigger : MonoBehaviour
    {
        public static NeuronMissionLastPlateTrigger NMLPT { get; private set; }
        public static System.Action gotUser;

        private void Start()
        {
            var highlighter = GetComponentInChildren<CollisionHighlights>();
            highlighter.initialColor = Color.red;

        }

        private void OnCollisionEnter(Collision collision)
        {
            gotUser?.Invoke();
        }
    }
}
