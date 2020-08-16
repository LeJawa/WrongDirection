using System;
using GravityGames.MizJam1.Controllers;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class PointBarrier : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                GameEvents.Instance.TriggerointBarrierCrossedEvent(other.GetComponent<Vehicle>());
            }
        }
    }
}