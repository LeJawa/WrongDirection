using System.Collections;
using GravityGames.MizJam1.Controllers;
using GravityGames.MizJam1.ScriptableObjects;
using GravityGames.MizJam1.Utils;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class TrafficManager
    {
        private TrafficSpawner _trafficSpawner;

        private readonly TrafficData _trafficData;

        private const int NumberOfLanes = 5;
        private bool[] _isLaneOccupied;
        private Coroutine[] _vehicleSpawnCoroutines;

        private float _totalSpawnWeight;
        private float _fastVehicleSpawnWeightRaw;
        private float _mediumVehicleSpawnWeightRaw;
        private float _slowVehicleSpawnWeightRaw;
        private float _fastVehicleSpawnWeight;
        private float _mediumVehicleSpawnWeight;
        private float _slowVehicleSpawnWeight;

        private readonly float _timeBetweenSpawns;
        private readonly float _deltaTimeBetweenSpawns;
        
        private readonly float _speedIncreaseFactor;
        private readonly float _timeBetweenSpeedIncreases;

        private Coroutine _spawningCoroutine;
        private bool _keepSpawning = true;


        public TrafficManager(TrafficData trafficData)
        {
            _trafficData = trafficData;
            _timeBetweenSpawns = _trafficData.timeBetweenSpawns;
            _deltaTimeBetweenSpawns = _trafficData.deltaTimeBetweenSpawns;
            _speedIncreaseFactor = _trafficData.speedIncreaseFactor;
            _timeBetweenSpeedIncreases = _trafficData.timeBetweenSpeedIncreases;
            _fastVehicleSpawnWeightRaw = _trafficData.fastVehicleSpawnWeight;
            _mediumVehicleSpawnWeightRaw = _trafficData.mediumVehicleSpawnWeight;
            _slowVehicleSpawnWeightRaw = _trafficData.slowVehicleSpawnWeight;

            UpdateSpawnWeights();
            
            _trafficSpawner = new TrafficSpawner();
            
            _isLaneOccupied = new bool[NumberOfLanes];
            _vehicleSpawnCoroutines = new Coroutine[NumberOfLanes];

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOccupied[lane] = false;
            }

            GameEvents.Instance.OnDespawnVehicle += HandleDespawnVehicleEvent;

            GameEvents.Instance.OnTrafficEventInvoked += HandleTrafficEventInvoked;
        }

        public void IncreaseSpawnWeights()
        {
            if (_mediumVehicleSpawnWeightRaw > 1)
            {
                _mediumVehicleSpawnWeightRaw -= 1;
            }

            if (_slowVehicleSpawnWeightRaw < 1)
            {
                _slowVehicleSpawnWeightRaw += 0.1f;
            }

            UpdateSpawnWeights();
        }

        private void UpdateSpawnWeights()
        {
            _totalSpawnWeight = _slowVehicleSpawnWeightRaw + _mediumVehicleSpawnWeightRaw + _fastVehicleSpawnWeightRaw;
            _slowVehicleSpawnWeight = _slowVehicleSpawnWeightRaw;
            _mediumVehicleSpawnWeight = _slowVehicleSpawnWeight + _mediumVehicleSpawnWeightRaw;
            _fastVehicleSpawnWeight = _mediumVehicleSpawnWeight + _fastVehicleSpawnWeightRaw;
        }

        private void HandleTrafficEventInvoked(TrafficEvent trafficEvent)
        {
            PauseSpawningVehicles();

            CoroutineHelper.Instance.StartCoroutine(StartTrafficEventCoroutine(trafficEvent));
        }

        private IEnumerator StartTrafficEventCoroutine(TrafficEvent trafficEvent)
        {
            yield return new WaitForSeconds(2f);

            for (int wave = 0; wave < trafficEvent.NumberOfWaves; wave++)
            {
                for (int lane = 0; lane < 5; lane++)
                {
                    if (trafficEvent.vehiclesPerLaneAndWave[wave, lane])
                    {
                        SpawnVehicle(lane);
                    }
                }
                yield return new WaitForSeconds(trafficEvent.TimeBetweenWaves);
            }
            
            yield return new WaitForSeconds(1f);
            
            RestartSpawningVehicles();
        }

        private void RestartSpawningVehicles()
        {
            GameEvents.Instance.TriggerVehicleSpeedIncreased(_speedIncreaseFactor);
            StartSpawningVehicles();
        }

        private void HandleDespawnVehicleEvent(Vehicle vehicle)
        {
            _trafficSpawner.DespawnVehicle(vehicle);
        }

        public void StartSpawningVehicles()
        {
            _keepSpawning = true;
            _spawningCoroutine = CoroutineHelper.Instance.StartCoroutine(StartSpawningVehiclesCoroutine());
        }

        private IEnumerator StartSpawningVehiclesCoroutine()
        {
            var minTimeBetweenSpawns = _timeBetweenSpawns - _deltaTimeBetweenSpawns;
            var maxTimeBetweenSpawns = _timeBetweenSpawns + _deltaTimeBetweenSpawns;

            float time = Time.time;
            
            while (_keepSpawning)
            {
                if (Time.time > time + _timeBetweenSpeedIncreases)
                {
                    GameEvents.Instance.TriggerVehicleSpeedIncreased(_speedIncreaseFactor);

                    if (minTimeBetweenSpawns > 0.3125f)
                    {
                        minTimeBetweenSpawns -= 0.05f;
                        maxTimeBetweenSpawns -= 0.05f;
                    }
                    
                    time = Time.time;
                }
                
                int lane = Random.Range(0, NumberOfLanes);
                while (_isLaneOccupied[lane])
                {
                    lane = (lane + 1) % NumberOfLanes;
                }
                
                SpawnVehicle(lane);
                
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));  
            }
        }

        private void SpawnVehicle(int lane)
        {
            _vehicleSpawnCoroutines[lane] = CoroutineHelper.Instance.StartCoroutine(SpawnVehicleCoroutine(lane));
        }


        private IEnumerator SpawnVehicleCoroutine(int lane)
        {
            _isLaneOccupied[lane] = true;
            
            GameEvents.Instance.TriggerSignalLaneEvent(lane, _timeBetweenSpawns);

            var wait = new WaitForSeconds(_timeBetweenSpawns);
            
            yield return wait;


            var rdm = Random.Range(0, _totalSpawnWeight);
            Vector3 position = new Vector3(11, 0, lane + 1);

            if (rdm < _slowVehicleSpawnWeight)
            {
                _trafficSpawner.SpawnSlowVehicle(position);
            }
            else if (rdm < _mediumVehicleSpawnWeight)
            {
                _trafficSpawner.SpawnMediumVehicle(position);
            }
            else
            {
                _trafficSpawner.SpawnFastVehicle(position);
            }
            
            yield return wait;

            _isLaneOccupied[lane] = false;
        }

        private void PauseSpawningVehicles()
        {
            _keepSpawning = false;
            CoroutineHelper.Instance.StopCoroutine(_spawningCoroutine);
        }

        public void StopSpawningVehicles()
        {
            _keepSpawning = false;
            CoroutineHelper.Instance.StopCoroutine(_spawningCoroutine);

            foreach (var coroutine in _vehicleSpawnCoroutines)
            {
                if (coroutine != null)
                {
                    CoroutineHelper.Instance.StopCoroutine(coroutine);
                }
            }
            
            CoroutineHelper.Instance.StopAllCoroutines();

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOccupied[lane] = false;
            }

            foreach (var vehicle in GameObject.FindGameObjectsWithTag("Vehicle"))
            {
                vehicle.GetComponent<Collider>().enabled = false;
            }
            
            _trafficSpawner.ResetVehicleInitialImpulse();
            
        }




    }
}