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

        public VehicleData vehicleData;

        public ParticleSystem crashParticles;


        // Start is called before the first frame update
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = vehicleData.mass;

            GetComponentInChildren<SpriteRenderer>().sprite = vehicleData.sprite;

            gameObject.tag = "Vehicle";
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < -40 || transform.position.x > 40 || transform.position.y < -10 || transform.position.y > 10)
            {
                GameEvents.Instance.TriggerDespawnVehicleEvent(this);
            }
        } 

        public void StartMoving(float impulseForce)
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
                _rigidbody.AddForce(Vector3.left * impulseForce, ForceMode.Impulse);
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnCollisionEnter(Collision other)
        {
            crashParticles.Play();
        }
    }
}