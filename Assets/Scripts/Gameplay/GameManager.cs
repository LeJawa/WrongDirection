using System.Collections;
using Cinemachine;
using GravityGames.MizJam1.Controllers;
using GravityGames.MizJam1.ScriptableObjects;
using GravityGames.MizJam1.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
// ReSharper disable Unity.InefficientPropertyAccess

namespace GravityGames.MizJam1.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public PlayerController playerController;
        
        private TrafficManager _trafficManager;
        public TrafficData difficulty;

        public AudioSource menuMusic;
        public AudioSource gameMusic;
        public float slowdownPitch;

        public SpriteRenderer[] warningArray;
        private int _points;

        public Transform HUDTransform;

        public TMP_Text pointsTextObject;
        public Animator pointsTextAnimator;
        private static readonly int AddPoints = Animator.StringToHash("AddPoints");

        public TMP_Text highScoreTextObject;

        public GameObject mainMenu;

        public GameObject prefabTutorial;

        public GameObject newHighScoreObject;

        public float timeScaleOnGameOver = 0.1f;
        private float fixedDeltaTIme;

        private GameState _state = GameState.Menu;

        public CinemachineVirtualCamera menuCamera;
        public CinemachineVirtualCamera gameOverCamera;

        private const string HighScoreKey = "HighScore";
        private int _highScore = 0;

        private float _exitTimer = 0;

        public SpriteRenderer secondsToStartAgain;
        public SpriteMask StartAgainTimerMask;
        public Sprite[] spriteNumberArray;
        public Sprite[] spriteBorderMaskArray;
        
        private int _numberOfTrafficEvents = 0;
        private int _spawnWeightIncreases = 0;

        private void Awake()
        {
            _trafficManager = new TrafficManager(difficulty);

            GameEvents.Instance.OnSignalLane += HandleSignalLaneEvent;
            GameEvents.Instance.OnPointBarrierCrossed += HandlePointBarrierCrossedEvent;
            GameEvents.Instance.OnPlayerCrashed += HandlePlayerCrashedEvent;
            
            AnimationManager.Instance.InitializeAnimationManager();

            if (PlayerPrefs.HasKey(HighScoreKey))
            {
                // PlayerPrefs.DeleteKey(HighScoreKey);
                _highScore = PlayerPrefs.GetInt(HighScoreKey);

                UpdateHighScore();
                if (_highScore > 0)
                {
                    highScoreTextObject.enabled = true;
                }
                
            }

            fixedDeltaTIme = Time.fixedDeltaTime;
        }

        private void UpdateHighScore()
        {
            if (_highScore > 0)
            {
                highScoreTextObject.text = $"High score:\n{_highScore}";
            }
        }

        private void HandlePlayerCrashedEvent()
        {
            EndGame();
        }

        private void EndGame()
        {
            StartGameOverWait();
            
            SetBulletTimeScale();
            gameOverCamera.Priority = 20;

            _trafficManager.StopSpawningVehicles();
            playerController.CanMove = false;

            AnimationManager.Instance.StartOpenPanelAnimation();

            HandleHighScore();
        }

        private void StartGameOverWait()
        {
            _state = GameState.GameOver;
            StartCoroutine(StartGameAfterWaiting());
        }

        private IEnumerator StartGameAfterWaiting()
        {
            StartAgainTimerMask.gameObject.SetActive(true);
            secondsToStartAgain.sprite = spriteNumberArray[3];
            StartAgainTimerMask.sprite = spriteBorderMaskArray[0];
            
            var quarterSecond = new WaitForSecondsRealtime(0.2f);
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[1];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[2];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[3];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[4];
            yield return quarterSecond;
            secondsToStartAgain.sprite = spriteNumberArray[2];
            StartAgainTimerMask.sprite = spriteBorderMaskArray[0];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[1];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[2];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[3];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[4];
            yield return quarterSecond;
            secondsToStartAgain.sprite = spriteNumberArray[1];
            StartAgainTimerMask.sprite = spriteBorderMaskArray[0];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[1];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[2];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[3];
            yield return quarterSecond;
            StartAgainTimerMask.sprite = spriteBorderMaskArray[4];
            yield return quarterSecond;
            secondsToStartAgain.sprite = spriteNumberArray[0];
            StartAgainTimerMask.sprite = spriteBorderMaskArray[0];
            yield return quarterSecond;
            
            StartGame();
        }

        private void SetBulletTimeScale()
        {
            Time.timeScale = timeScaleOnGameOver;
            Time.fixedDeltaTime = Time.fixedDeltaTime * timeScaleOnGameOver;
            gameMusic.pitch = slowdownPitch;
        }

        private void HandleHighScore()
        {
            if (_points > _highScore)
            {
                _highScore = _points;
                PlayerPrefs.SetInt(HighScoreKey, _highScore);
                PlayerPrefs.Save();
                
                newHighScoreObject.SetActive(true);
                UpdateHighScore();
            }
        }

        private void HandlePointBarrierCrossedEvent(Vehicle vehicle)
        {
            if (_state == GameState.Playing)
            {
                AddVehiclePoints(vehicle);
            }
        }

        private void AddVehiclePoints(Vehicle vehicle)
        {
            _points += vehicle.vehicleData.points;
            UpdatePointsTextObject();

            if (_points > ( (250 + 50 * _numberOfTrafficEvents) * (1 + _numberOfTrafficEvents) ))
            {
                GameEvents.Instance.TriggerTrafficEvent(new TrafficEvent(1+_numberOfTrafficEvents, 1f));
                _numberOfTrafficEvents++;
            }

            if (_points > 1000 * (1 + _spawnWeightIncreases))
            {
                _trafficManager.IncreaseSpawnWeights();
                _spawnWeightIncreases++;
            }
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
            if (InputSystem.GetDevice<Keyboard>().enterKey.wasPressedThisFrame)
            {
                Debug.Log("Enter pressed");
                HandleStartPressed();
            }
            if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame)
            {
                HandleEscapePressed();
            }
            _exitTimer += Time.unscaledDeltaTime;
        }

        private void HandleStartPressed()
        {
            if (_state != GameState.Playing)
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

            if (_state != GameState.Menu)
            {
                BackToMenu();
            }
        }

        private void BackToMenu()
        {
            SetNormalTimeScale();
            
            _state = GameState.Menu;
            
            playerController.ResetPlayer();
            ResetHUDObjects();
            pointsTextObject.enabled = false;
            
            mainMenu.SetActive(true);
            if (_highScore > 0)
            {
                highScoreTextObject.enabled = true;
            }
            
            _trafficManager.StopSpawningVehicles();
            playerController.CanMove = false;

            AnimationManager.Instance.StartOpenPanelAnimation();
            
            menuCamera.Priority = 50;
            
            StartCoroutine(AudioFadeOut.FadeOut(gameMusic, 0.5f));
            menuMusic.Play();
        }

        private void StartGame()
        {
            gameOverCamera.Priority = 0;
            _state = GameState.Playing;    
            playerController.ResetPlayer();
            _points = 0;
            _numberOfTrafficEvents = 0;
            ResetHUDObjects();

            StartCoroutine(AudioFadeOut.FadeOut(menuMusic, 0.3f));

            StartCoroutine(StartGameCoroutine());
        }

        private void ResetHUDObjects()
        {
            UpdatePointsTextObject();
            newHighScoreObject.SetActive(false);
            mainMenu.SetActive(false);
            highScoreTextObject.enabled = false;
            StartAgainTimerMask.gameObject.SetActive(false);
            
            StopAllCoroutines();
            foreach (var warning in warningArray)
            {
                warning.enabled = false;
            }
        }

        private IEnumerator StartGameCoroutine()
        {
            AnimationManager.Instance.StartClosePanelAnimation();
            menuCamera.Priority = 5;
            
            SetNormalTimeScale();

            playerController.CanMove = true;

            Instantiate(prefabTutorial, HUDTransform);
            
            gameMusic.Stop();
            
            yield return new WaitForSeconds(1f);
            
            gameMusic.Play();
            
            yield return new WaitForSeconds(1f);
            
            
            _trafficManager.StartSpawningVehicles();

            pointsTextObject.enabled = true;
        }

        private void SetNormalTimeScale()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = fixedDeltaTIme;
            gameMusic.pitch = 1;
        }
    }
}