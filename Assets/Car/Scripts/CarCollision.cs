using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsaacFagg.Race;

namespace IsaacFagg.Cars
{
    [RequireComponent(typeof(CarMovement))]
    public class CarCollision : MonoBehaviour
    {
        CarMovement movement;
        RaceManager rm;
        

        public List<string> triggers = new List<string>();

        private void Awake()
        {
            movement = GetComponent<CarMovement>();

            rm = Object.FindObjectOfType<RaceManager>();
        }

        private void Update()
        {
            if (triggers.Count == 0)
            {
                movement.OnEnterOffCourse();
            }
            else
            {
                movement.OnExitOffCourse();
            }

            if (triggers.Contains("CurrentTrack"))
            {
                movement.OnEnterTrack();
            }
            else if (!triggers.Contains("CurrentTrack") && triggers.Contains("Gravel"))
            {
                movement.OnEnterGravel();
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
            if (!triggers.Contains(collision.tag))
            {
                triggers.Add(collision.tag);
            }

            if (collision.CompareTag("Checkpoint"))
            {
                rm.HitCheckpoint(collision.GetComponent<Checkpoint>());
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (triggers.Contains(collision.tag))
            {
                triggers.Remove(collision.tag);
            }
        }



    }
}
