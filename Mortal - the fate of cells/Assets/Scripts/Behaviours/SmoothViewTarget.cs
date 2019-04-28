using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    public class SmoothViewTarget : MonoBehaviour
    {
        public Vector3 targetPoint;
        public float viewDistance = 1.0f;
        public float smoothTime = 0.1f;
        public bool lockY = true;
        Vector3 currentVelocity = Vector3.zero;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(targetPoint);
            Vector3 nextPos = targetPoint - transform.forward * viewDistance;
            if (lockY)
                nextPos.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref currentVelocity, smoothTime);

        }
    }
}
