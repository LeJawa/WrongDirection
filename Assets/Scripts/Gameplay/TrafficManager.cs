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

        private float _globalTimer = 0;
        private float _timeBetweenSpawns = 0.5f;

        private Coroutine _spawingCoroutine;
        private bool _keepSpawing = true;

        
        public TrafficManager(TrafficData trafficData)
        {
            _trafficData = trafficData;
            _timeBetweenSpawns = _trafficData.timeBetweenSpawns;
            
            _trafficSpawner = new TrafficSpawner();
            
            _isLaneOcupied = new bool[NumberOfLanes];

            for (int i = 0; i < _isLaneOcupied.Length; i++)
            {
                _isLaneOcupied[i] = false;
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
            var wait = new WaitForSeconds(_timeBetweenSpawns);
            while (_keepSpawing)
            {
                int lane = Random.Range(0, NumberOfLanes);
                while (_isLaneOcupied[lane])
                {
                    lane = Random.Range(0, NumberOfLanes);
                }
                
                SpawnVehicle(lane, Direction.Left);
                
                yield return wait;
            }
        }

        private void SpawnVehicle(int lane, Direction direction)
        {
            CoroutineHelper.Instance.StartCoroutine(SpawnVehicleCoroutine(lane, direction));
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

        private void StopSpawningVehicles()
        {
            _keepSpawing = false;
            CoroutineHelper.Instance.StopCoroutine(_spawingCoroutine);
        }




    }
}