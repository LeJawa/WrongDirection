using System;
using GravityGames.MizJam1.Gameplay;
using GravityGames.MizJam1.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace GravityGames.MizJam1.Controllers
{
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Vehicle : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _initialVelocity;

        public VehicleData vehicleData;


        // Start is called before the first frame update
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            GetComponentInChildren<SpriteRenderer>().sprite = vehicleData.sprite;
            _initialVelocity = vehicleData.initialVelocity;

            gameObject.tag = "Vehicle";
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < -20 || transform.position.x > 20 || transform.position.y < -10 || transform.position.y > 10)
            {
                GameEvents.Instance.TriggerDespawnVehicleEvent(this);
            }
        } 

        public void StartMoving(Direction direction = Direction.Left)
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            switch (direction)
            {
                case Direction.Right:
                    _rigidbody.AddForce(Vector3.right * _initialVelocity, ForceMode.VelocityChange);
                    break;
                case Direction.Left:
                    _rigidbody.AddForce(Vector3.left * _initialVelocity, ForceMode.VelocityChange);
                    break;
            }
            
            
        }
    }
}