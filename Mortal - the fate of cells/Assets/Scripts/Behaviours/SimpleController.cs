using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mortal
{
    [System.Serializable]
    public struct BlockMovement
    {
        public bool forward;
        public bool backwards;
        public bool right;
        public bool left;
    }

    [System.Serializable]
    public struct VelocityLimits
    {
        public float max;
        public float min;
    }

    [System.Serializable]
    public struct AccelerationSpeed
    {
        public float forward;
        public float backwards;
        public float right;
        public float left;
        public float up;
    }

    public class SimpleController : MonoBehaviour
    {
        Rigidbody rb;

        public AccelerationSpeed acceleration;
        public BlockMovement blockMovement;
        public VelocityLimits frontalMovementLimits;
        public VelocityLimits sideMovementLimits;
        public float onHitModifier = 0.95f;

        protected bool IsColliding;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        int totalEntered = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
                totalEntered++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
                totalEntered--;
        }

        protected Vector3 GetVelocityForFrame(float deltaTime)
        {
            Vector3 velocityForFrame = Vector3.zero;
            Vector3 vel = rb.velocity;
            IsColliding = totalEntered > 0;

            if (!blockMovement.forward && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
            {
                velocityForFrame.z += acceleration.forward * deltaTime;
            }
            if (!blockMovement.backwards && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                velocityForFrame.z -= acceleration.backwards * deltaTime;
            }
            if (!blockMovement.left && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                velocityForFrame.x -= acceleration.left * deltaTime;
            }
            if (!blockMovement.right && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                velocityForFrame.x += acceleration.right * deltaTime;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * acceleration.up * deltaTime);
            }
            float xn = Mathf.Clamp(velocityForFrame.x, -1, 1);
            float zn = Mathf.Clamp(velocityForFrame.z, -1, 1);
            transform.localEulerAngles = new Vector3(30 * zn, 0, -30 * xn);
            vel += velocityForFrame;
            vel.z = Mathf.Clamp(vel.z, frontalMovementLimits.min, frontalMovementLimits.max);
            vel.x = Mathf.Clamp(vel.x, sideMovementLimits.min, sideMovementLimits.max);
            return vel;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var vel = GetVelocityForFrame(Time.fixedDeltaTime);
            if (IsColliding)
            {
                vel.z *= onHitModifier;
                vel.x *= onHitModifier;
            }
            rb.velocity = vel;
        }
    }
}
