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

        private TrafficData _trafficData;

        private const int NumberOfLanes = 5;
        private bool[] _isLaneOcupied;
        private Coroutine[] _vehicleSpawnCoroutines;

        private readonly float _timeBetweenSpawns;
        private readonly float _deltaTimeBetweenSpawns;

        private Coroutine _spawingCoroutine;
        private bool _keepSpawing = true;

        
        public TrafficManager(TrafficData trafficData)
        {
            _trafficData = trafficData;
            _timeBetweenSpawns = _trafficData.timeBetweenSpawns;
            _deltaTimeBetweenSpawns = _trafficData.deltaTimeBetweenSpawns;
            
            _trafficSpawner = new TrafficSpawner();
            
            _isLaneOcupied = new bool[NumberOfLanes];
            _vehicleSpawnCoroutines = new Coroutine[NumberOfLanes];

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOcupied[lane] = false;
            }

            GameEvents.Instance.OnDespawnVehicle += HandleDespawnVehicleEvent;
        }

        private void HandleDespawnVehicleEvent(Vehicle vehicle)
        {
            _trafficSpawner.DespawnVehicle(vehicle);
        }

        public void StartSpawningVehicles()
        {
            _keepSpawing = true;
            _spawingCoroutine = CoroutineHelper.Instance.StartCoroutine(StartSpawningVehiclesCoroutine());
        }

        private IEnumerator StartSpawningVehiclesCoroutine()
        {
            var minTimeBetweenSpawns = _timeBetweenSpawns - _deltaTimeBetweenSpawns;
            var maxTimeBetweenSpawns = _timeBetweenSpawns + _deltaTimeBetweenSpawns;
            
            
            while (_keepSpawing)
            {
                int lane = Random.Range(0, NumberOfLanes);
                while (_isLaneOcupied[lane])
                {
                    lane = Random.Range(0, NumberOfLanes);
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
            _isLaneOcupied[lane] = true;
            
            GameEvents.Instance.TriggerSignalLaneEvent(lane, _timeBetweenSpawns * 2);

            var wait = new WaitForSeconds(_timeBetweenSpawns * 2);
            
            yield return wait;
            
            _trafficSpawner.SpawnVehicle(new Vector3(11, 0, lane + 1), direction);
            
            yield return wait;

            _isLaneOcupied[lane] = false;
        }

        public void StopSpawningVehicles()
        {
            _keepSpawing = false;
            CoroutineHelper.Instance.StopCoroutine(_spawingCoroutine);

            foreach (var coroutine in _vehicleSpawnCoroutines)
            {
                if (coroutine != null)
                {
                    CoroutineHelper.Instance.StopCoroutine(coroutine);
                }
            }

            for (int lane = 0; lane < NumberOfLanes; lane++)
            {
                _isLaneOcupied[lane] = false;
            }
            
        }




    }
}