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
            
            _trafficSpawner = new TrafficSpawner();
            
            _isLaneOccupied = new bool[NumberOfLanes];
            _vehicleSpawnCoroutines = new Coroutine[NumberOfLanes];

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOccupied[lane] = false;
            }

            GameEvents.Instance.OnDespawnVehicle += HandleDespawnVehicleEvent;
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
                    time = Time.time;
                }
                
                int lane = Random.Range(0, NumberOfLanes);
                while (_isLaneOccupied[lane])
                {
                    lane = (lane + 1) % NumberOfLanes;
                }
                
                SpawnVehicle(lane, Direction.Left);
                
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
                
                
            }
        }

        private void SpawnVehicle(int lane, Direction direction)
        {
            _vehicleSpawnCoroutines[lane] = CoroutineHelper.Instance.StartCoroutine(SpawnVehicleCoroutine(lane, direction));
        }


        private IEnumerator SpawnVehicleCoroutine(int lane, Direction direction)
        {
            _isLaneOccupied[lane] = true;
            
            GameEvents.Instance.TriggerSignalLaneEvent(lane, _timeBetweenSpawns);

            var wait = new WaitForSeconds(_timeBetweenSpawns);
            
            yield return wait;
            
            _trafficSpawner.SpawnVehicle(new Vector3(11, 0, lane + 1), direction);
            
            yield return wait;

            _isLaneOccupied[lane] = false;
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

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOccupied[lane] = false;
            }

            foreach (var vehicle in GameObject.FindGameObjectsWithTag("Vehicle"))
            {
                vehicle.GetComponent<Collider>().enabled = false;
            }
            
        }




    }
}