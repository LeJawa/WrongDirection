using System;
using UnityEngine;

namespace Lean.Pool.Extras
{
	/// <summary>This component allows you to reset a Rigidbody's velocity via Messages or via Poolable.</summary>
	[RequireComponent(typeof(Rigidbody))]
	[HelpURL(LeanPool.HelpUrlPrefix + "LeanPooledRigidbody")]
	[AddComponentMenu(LeanPool.ComponentPathPrefix + "Pooled Rigidbody")]
	public class LeanPooledRigidbody : MonoBehaviour, IPoolable
	{
		private Rigidbody _rigidbody;
		
		private void Awake()
		{
			SetRigidBody();
		}

		private void SetRigidBody()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void OnSpawn()
		{
		}

		public void OnDespawn()
		{
			if (_rigidbody == null)
			{
				SetRigidBody();
			}
			
			
			_rigidbody.velocity        = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
		}
	}
}