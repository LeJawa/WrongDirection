﻿using System.Collections;
using Cinemachine;
using GravityGames.MizJam1.Controllers;
using GravityGames.MizJam1.ScriptableObjects;
using Lean.Common;
using Lean.Touch;
using TMPro;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public PlayerController playerController;
        
        private TrafficManager _trafficManager;
        public TrafficData difficulty;

        public SpriteRenderer[] warningArray;
        private int _points;

        public Transform HUDTransform;

        public TMP_Text pointsTextObject;
        public Animator pointsTextAnimator;
        private static readonly int AddPoints = Animator.StringToHash("AddPoints");

        public GameObject mainMenu;

        public GameObject prefabTutorial;

        public GameObject highscoreObject;

        public float timeScaleOnGameOver = 0.1f;
        private float fixedDeltaTIme;

        private bool _playing = false;

        public CinemachineVirtualCamera menuCamera;

        private const string HighScoreKey = "HighScore";
        private int _highScore = 0;

        private float _exitTimer = 0;

        private void Awake()
        {
            _trafficManager = new TrafficManager(difficulty);

            GameEvents.Instance.OnSignalLane += HandleSignalLaneEvent;
            GameEvents.Instance.OnPointBarrierCrossed += HandlePointBarrierCrossedEvent;
            GameEvents.Instance.OnPlayerCrashed += HandlePlayerCrashedEvent;
            
            LeanTouch.OnFingerTap += HandleFingerTap;
            
            AnimationManager.Instance.InitializeAnimationManager();

            if (PlayerPrefs.HasKey(HighScoreKey))
            {
                // PlayerPrefs.DeleteKey(HighScoreKey);
                _highScore = PlayerPrefs.GetInt(HighScoreKey);
            }

            fixedDeltaTIme = Time.fixedDeltaTime;
        }

        private void HandleFingerTap(LeanFinger finger)
        {
            HandleStartPressed();
        }

        private void HandlePlayerCrashedEvent()
        {
            EndGame();
        }

        private void EndGame()
        {
            _playing = false;
            
            SetBulletTimeScale();

            _trafficManager.StopSpawningVehicles();
            playerController.CanMove = false;

            AnimationManager.Instance.StartOpenPanelAnimation();

            HandleHighScore();
        }

        private void SetBulletTimeScale()
        {
            Time.timeScale = timeScaleOnGameOver;
            Time.fixedDeltaTime = Time.fixedDeltaTime * timeScaleOnGameOver;
        }

        private void HandleHighScore()
        {
            if (_points > _highScore)
            {
                _highScore = _points;
                PlayerPrefs.SetInt(HighScoreKey, _highScore);
                PlayerPrefs.Save();
                
                highscoreObject.SetActive(true);
            }
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
            UpdatePointsTextObject();
        }

        private void UpdatePointsTextObject()
        {
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
#if UNITY_STANDALONE || UNITY_EDITOR
            if (LeanInput.GetDown(KeyCode.Return))
            {
                HandleStartPressed();
            }
            if (LeanInput.GetDown(KeyCode.Escape))
            {
                HandleEscapePressed();
            }
#endif
            _exitTimer += Time.unscaledDeltaTime;
        }

        private void HandleStartPressed()
        {
            if (!_playing)
            {
                StartGame();
            }
        }

        private void HandleEscapePressed()
        {
            if (_exitTimer < 0.5f)
            {
                Application.Quit();
            }

            _exitTimer = 0;

            BackToMenu();
        }

        private void BackToMenu()
        {
            SetNormalTimeScale();
            
            _playing = false;
            
            playerController.ResetPlayer();
            ResetHUDObjects();
            pointsTextObject.enabled = false;
            
            mainMenu.SetActive(true);
            
            _trafficManager.StopSpawningVehicles();
            playerController.CanMove = false;

            AnimationManager.Instance.StartOpenPanelAnimation();
            
            menuCamera.Priority = 50;
        }

        private void StartGame()
        {
            _playing = true;
            playerController.ResetPlayer();
            _points = 0;
            ResetHUDObjects();

            StartCoroutine(StartGameCoroutine());
        }

        private void ResetHUDObjects()
        {
            UpdatePointsTextObject();
            highscoreObject.SetActive(false);
            mainMenu.SetActive(false);
            
            StopAllCoroutines();
            foreach (var warning in warningArray)
            {
                warning.enabled = false;
            }
        }

        private IEnumerator StartGameCoroutine()
        {
            AnimationManager.Instance.StartClosePanelAnimation();
            menuCamera.Priority = 0;
            
            SetNormalTimeScale();

            playerController.CanMove = true;

            Instantiate(prefabTutorial, HUDTransform);
            
            yield return new WaitForSeconds(2f);
            
            
            _trafficManager.StartSpawningVehicles();

            pointsTextObject.enabled = true;
        }

        private void SetNormalTimeScale()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = fixedDeltaTIme;
        }
    }
}