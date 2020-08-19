using System;
using GravityGames.MizJam1.Gameplay;
using GravityGames.MizJam1.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GravityGames.MizJam1.Controllers
{
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Vehicle : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _initialVelocity;
        private float _minVelocity;
        private float _maxVelocity;

        public VehicleData vehicleData;

        public ParticleSystem crashParticles;


        // Start is called before the first frame update
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            GetComponentInChildren<SpriteRenderer>().sprite = vehicleData.sprite;
            ResetVelocity();

            gameObject.tag = "Vehicle";

            GameEvents.Instance.OnVehicleSpeedIncreased += increaseFactor =>
            {
                _minVelocity *= increaseFactor;
                _maxVelocity *= increaseFactor;
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < -40 || transform.position.x > 40 || transform.position.y < -10 || transform.position.y > 10)
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

            var velocity = Random.Range(_minVelocity, _maxVelocity);
            Debug.Log($"Velocity is {velocity}");
            
            switch (direction)
            {
                case Direction.Right:
                    _rigidbody.AddForce(Vector3.right * velocity, ForceMode.VelocityChange);
                    break;
                case Direction.Left:
                    _rigidbody.AddForce(Vector3.left * velocity, ForceMode.VelocityChange);
                    break;
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnCollisionEnter(Collision other)
        {
            crashParticles.Play();
        }

        public void ResetVelocity()
        {
            _initialVelocity = vehicleData.initialVelocity;
            _minVelocity = 0.8f * _initialVelocity;
            _maxVelocity = 1.2f * _initialVelocity;
        }
    }
}