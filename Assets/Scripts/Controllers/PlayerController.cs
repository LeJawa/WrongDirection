using GravityGames.MazJam1.Controllers;
using GravityGames.MizJam1.Gameplay;
using UnityEngine;

namespace GravityGames.MizJam1.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private Controls _controls;
        
        private readonly Vector3 InitialPosition = new Vector3(-3, 0, 3);
        private Vector3 _position;

        public SpriteRenderer vehicleLights;

        public ParticleSystem crashParticles;

        public AudioSource crashSource;

        public bool CanMove { get; set; } = false;


        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            _controls = new Controls();
            _controls.Enable();
            _controls.Player.UpDownMovement.performed += ctx => Move(ctx.ReadValue<float>());
            _controls.Player.UpDownMovement.canceled += ctx => Stop();
            
            _position = InitialPosition;
        }

        private void Move(float moveInput)
        {
            if (!CanMove) return;
            
            if (moveInput < 0)
            {
                _position.z = Mathf.Clamp(_position.z - 1, 1, 5);
            }
            else
            {
                _position.z = Mathf.Clamp(_position.z + 1, 1, 5);
            }

            transform.position = _position;
        }

        private void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnCollisionEnter(Collision other)
        {
            vehicleLights.enabled = false;
            
            crashParticles.Play();
            crashSource.Play();
            
            GameEvents.Instance.TriggerPlayerCrashedEvent();
        }

        public void ResetPlayer()
        {
            vehicleLights.enabled = true;
            _position = InitialPosition;
            var currentTransform = transform;
            
            currentTransform.position = _position;
            currentTransform.rotation = Quaternion.identity;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
