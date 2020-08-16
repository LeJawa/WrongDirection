using System.Collections;
using GravityGames.MizJam1.Controllers;
using GravityGames.MizJam1.ScriptableObjects;
using Lean.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GravityGames.MizJam1.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private TrafficManager _trafficManager;
        public TrafficData difficulty;

        public Image[] warningArray;
        private int _points;

        public TMP_Text pointsTextObject;
        public Animator pointsTextAnimator;
        private static readonly int AddPoints = Animator.StringToHash("AddPoints");

        public float timeScaleOnGameOver = 0.1f;

        private bool _playing = false; 

        private void Awake()
        {
            _trafficManager = new TrafficManager(difficulty);

            GameEvents.Instance.OnSignalLane += HandleSignalLaneEvent;
            GameEvents.Instance.OnPointBarrierCrossed += HandlePointBarrierCrossedEvent;
            GameEvents.Instance.OnPlayerCrashed += HandlePlayerCrashedEvent;
        }

        private void HandlePlayerCrashedEvent()
        {
            Time.timeScale = timeScaleOnGameOver;
            Time.fixedDeltaTime = Time.fixedDeltaTime * timeScaleOnGameOver;
            
            _trafficManager.StopSpawningVehicles();

            _playing = false;
        }

        private void HandlePointBarrierCrossedEvent(Vehicle vehicle)
        {
            if (_playing)
            {
                AddVehiclePoints(vehicle);
            }
        }

        private void AddVehiclePoints(Vehicle vehicle)
        {
            _points += vehicle.vehicleData.points;
            pointsTextObject.text = _points.ToString();
            pointsTextAnimator.SetTrigger(AddPoints);
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


            if (LeanInput.GetMouseDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(LeanInput.GetMousePosition());
                
                Debug.Log(ray.GetPoint(40f));
            }
            
        }
    }
}