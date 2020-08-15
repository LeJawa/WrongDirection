using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityGames.MizJam1.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private TrafficManager _trafficManager;

        private void Start()
        {
            _trafficManager = new TrafficManager();
        }


        private void Update()
        {
        }
    }
}