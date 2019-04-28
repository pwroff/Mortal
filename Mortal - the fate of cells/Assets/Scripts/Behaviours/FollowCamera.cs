using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target;
        public float viewDistance = 1.0f;
        public float smoothTime = 0.1f;
        public bool lockOffset = false;
        Vector3 currentVelocity = Vector3.zero;
        
        Vector3 offset;

        private void Awake()
        {
            offset = transform.position - target.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 nextPos;
            if (lockOffset)
            {
                nextPos = target.position + offset;
            }
            else
            {
                transform.LookAt(target);
                nextPos = target.position - transform.forward * viewDistance;
            }
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref currentVelocity, smoothTime);
        }
    }
}
