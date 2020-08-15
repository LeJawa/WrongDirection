using System;
using System.Collections;
using GravityGames.MizJam1.ScriptableObjects;
using Lean.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityGames.MizJam1.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private TrafficManager _trafficManager;
        public TrafficData difficulty;

        public SpriteRenderer[] warningArray;
        private void Awake()
        {
            _trafficManager = new TrafficManager(difficulty);

            GameEvents.Instance.OnSignalLane += HandleSignalLaneEvent;
        }

        private void HandleSignalLaneEvent(int lane, float duration)
        {
            StartCoroutine(BlinkWarningCoroutine(lane, duration));
        }

        private IEnumerator BlinkWarningCoroutine(int lane, float duration)
        {
            warningArray[lane].enabled = true;

            yield return new WaitForSeconds(0.05f);

            warningArray[lane].enabled = false;

            yield return new WaitForSeconds(0.05f);
            
            warningArray[lane].enabled = true;
            
            
            yield return new WaitForSeconds(duration - 0.2f);
            
            
            warningArray[lane].enabled = false;
        }


        private void Update()
        {
            if (LeanInput.GetDown(KeyCode.Return))
            {
                _trafficManager.StartSpawningVehicles();
            }
            
            
        }
    }
}