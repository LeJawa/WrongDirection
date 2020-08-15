using System;
using GravityGames.MazJam1.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityGames.MizJam1.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private Controls _controls;
        
        private Vector3 _position = new Vector3(-3, 0, 3); 
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            _controls = new Controls();
            _controls.Enable();
            _controls.Player.UpDownMovement.performed += ctx => Move(ctx.ReadValue<float>());
            _controls.Player.UpDownMovement.canceled += ctx => Stop();
        }

        private void Move(float moveInput)
        {
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

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Player crashed");
        }
    }
}
