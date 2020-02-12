using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(CarMovement))]
    public class CarCollision : MonoBehaviour
    {
        CarMovement movement;

        public List<Collider2D> triggers = new List<Collider2D>();

        private void Awake()
        {
            movement = GetComponent<CarMovement>();
        }

        private void Update()
        {
            if (triggers.Count == 0)
            {
                Debug.Log("Offtrack");
                movement.OnEnterOffCourse();
            }
            else
            {
                movement.OnExitOffCourse();

            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Colliding with " + collision.collider.name + ". Tag " + collision.collider.tag);

            if (collision.collider.CompareTag("Obstacle") == true)
            {
                movement.OnCollideWithObstacle();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!triggers.Contains(collision))
            {
                triggers.Add(collision);
            }


            if (collision.CompareTag("Oil") == true )
            {
                movement.OnCollideWithOil();
            }

            if (collision.CompareTag("CurrentTrack") == true)
            {
                movement.OnEnterTrack();
            }
            else if (collision.CompareTag("Gravel") == true)
            {
                movement.OnEnterGravel();
            }          
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (triggers.Contains(collision))
            {
                triggers.Remove(collision);
            }
        }



    }
}
