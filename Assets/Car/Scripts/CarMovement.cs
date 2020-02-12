using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(Car))]
    public class CarMovement : MonoBehaviour
    {
        [Header("Engine")]
        public float maxEngineForce;
        public float maxReverseForce;

        public float reversePower;
        public float acceleration;
        public float deceleration;


        float m_EnginePower = 0f;
        float m_TargetEnginePower = 0f;
        float m_CurrentMaxEnginePower = 1f;

        [Header("Steering")]
        public float maxSteeringTorque;
        float m_SteeringDirection = 0f;

        [Header("Drifting")]
        float driftFactor;

        public float driftFactorSticky = 0.9f;
        public float driftFactorSlippy = 1;
        public float maxStickyVelocity = 2.5f;
        public float minSlippyVelocity = 1.5f;

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            UpdateEnginePower();

        }

        void UpdateEnginePower()
        {
            float speedChange = acceleration;

            if (m_TargetEnginePower == 0f)
            {
                speedChange = deceleration;
            }

            m_EnginePower = Mathf.MoveTowards(m_EnginePower, m_TargetEnginePower, speedChange * Time.deltaTime);
        }


        private void FixedUpdate()
        {
            ApplyEngineForce();
            ApplySteeringForce();
            ApplyDriftForce();
        }

        private void ApplyEngineForce()
        {
            float engineForce = maxEngineForce;

            if (m_EnginePower < 0f)
            {
                engineForce = maxReverseForce;
            }

            rb.AddForce(transform.up * m_EnginePower * engineForce * m_CurrentMaxEnginePower, ForceMode2D.Force);
        }

        private void ApplySteeringForce()
        {
            //rb.AddTorque(m_SteeringDirection * maxSteeringTorque, ForceMode2D.Force);

            float tf = Mathf.Lerp(0, maxSteeringTorque, rb.velocity.magnitude / 2);
            rb.angularVelocity = m_SteeringDirection * tf;
        }

        public void ApplyDriftForce()
        {
            driftFactor = driftFactorSticky;
            if (RightVelocity().magnitude > maxStickyVelocity)
            {
                driftFactor = driftFactorSlippy;
            }

            rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;
        }

        public void SetEnginePower(float power)
        {
            m_TargetEnginePower = Mathf.Clamp(power, -1f, 1f);
        }


        public void SetSteeringDirectrion(float direction)
        {
            m_SteeringDirection = Mathf.Clamp(direction, -1f, 1f);
        }





        public void OnCollideWithObstacle()
        {
            m_EnginePower = 0f;
        }

        public void OnEnterTrack()
        {
            rb.drag = 10f;
        }

        public void OnEnterGravel()
        {
            rb.drag = 30f;
        }


        public void OnCollideWithOil()
        {
            m_EnginePower = 0f;
            //reduce friction
        }

        public void OnEnterOffCourse()
        {
            m_CurrentMaxEnginePower = 0.25f;
        }

        public void OnExitOffCourse()
        {
            m_CurrentMaxEnginePower = 1f;
        }





        Vector2 ForwardVelocity()
        {
            return transform.up * Vector2.Dot(rb.velocity, transform.up);
        }

        Vector2 RightVelocity()
        {
            return transform.right * Vector2.Dot(rb.velocity, transform.right);
        }





    }
}
