using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(Car))]
    public class CarMovement : MonoBehaviour
    {
        Car car;
        CarModel cm;
        Rigidbody2D rb;


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
        public float driftFactorSticky = 0.9f;
        public float driftFactorSlippy = 1;
        public float maxStickyVelocity = 2.5f;
        public float minSlippyVelocity = 1.5f;

        bool tempDrift = false;
        public float m_DriftingForce = 0f;

        [Header("Obstacles")]
        public float trackFriction = 1f;
        public float gravelFriction = 3f;
        public float oilSlipTime = 3f;

        Vector2 centreOfMass = new Vector2(0, 0.5f);


        private void Awake()
        {
            car = GetComponent<Car>();
            cm = GetComponent<CarModel>();
            rb = GetComponent<Rigidbody2D>();

            rb.centerOfMass = centreOfMass;
        }

        private void Update()
        {
            UpdateEnginePower();
            cm.WheelTurn(m_SteeringDirection);
        }

        void UpdateEnginePower()
        {
            float speedChange = acceleration;

            if (m_TargetEnginePower == 0f)
            {
                speedChange = deceleration;
            }

            float targetEnginePower = m_TargetEnginePower * m_CurrentMaxEnginePower;

            m_EnginePower = Mathf.MoveTowards(m_EnginePower, targetEnginePower, speedChange * Time.deltaTime);
        }


        private void FixedUpdate()
        {
            if (car.carState == CarState.On )
            {
                ApplyEngineForce();
                ApplySteeringForce();
                ApplyDriftForce();
            }

        }

        private void ApplyEngineForce()
        {
            //From back tires
            float engineForce = maxEngineForce;

            if (m_EnginePower < 0f)
            {
                engineForce = maxReverseForce;
            }

            Vector3 totalForce = transform.up * m_EnginePower * engineForce;


            //rb.AddForce(totalForce, ForceMode2D.Force);

            rb.AddForceAtPosition(totalForce/2, cm.wheel_BL.transform.position, ForceMode2D.Force);
            rb.AddForceAtPosition(totalForce/2, cm.wheel_BR.transform.position, ForceMode2D.Force);


        }

        private void ApplySteeringForce()
        {
            //rb.AddTorque(m_SteeringDirection * maxSteeringTorque, ForceMode2D.Force);

            float tf = Mathf.Lerp(0, maxSteeringTorque, rb.velocity.magnitude / 2);
            rb.angularVelocity = m_SteeringDirection * tf;
        }

        public void ApplyDriftForce()
        {
            SetDriftForce(driftFactorSticky);

            if (RightVelocity().magnitude > maxStickyVelocity)
            {
                SetDriftForce(driftFactorSlippy);
            }

            rb.velocity = ForwardVelocity() + RightVelocity() * m_DriftingForce;
        }

        public void SetEnginePower(float power)
        {
            m_TargetEnginePower = Mathf.Clamp(power, -1f, 1f);
        }


        public void SetSteeringDirectrion(float direction)
        {
            m_SteeringDirection = Mathf.Clamp(direction, -1f, 1f);
        }

        public void SetDriftForce(float force)
        {
            if (!tempDrift)
            {
                m_DriftingForce = Mathf.Clamp(force, 0f, 1f);
            }
        }

        IEnumerator TempDriftLerp(float countdown)
        {
            Debug.Log("temp drift");
            tempDrift = true;
            m_DriftingForce = driftFactorSlippy;
            yield return new WaitForSeconds(countdown);
            tempDrift = false;
        }


        public void OnCollideWithObstacle()
        {
            m_EnginePower = 0f;
        }

        public void OnEnterTrack()
        {
            rb.drag = trackFriction;
        }

        public void OnEnterGravel()
        {
            rb.drag = gravelFriction;
        }


        public void OnCollideWithOil()
        {
            StartCoroutine(TempDriftLerp(oilSlipTime));
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
