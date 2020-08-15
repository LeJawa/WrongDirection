using System.Collections;
using GravityGames.MizJam1.Utils;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class TrafficManager
    {
        private TrafficSpawner _trafficSpawner;

        private const int NumberOfLanes = 5;
        private float[] _timerArray;

        private float _globalTimer = 0;
        private const float TimeBetweenSpawns = 1f;

        private Coroutine _spawingCoroutine;
        private bool _keepSpawing = true;

        public TrafficManager()
        {
            _trafficSpawner = new TrafficSpawner();
            
            _timerArray = new float[NumberOfLanes];

            for (int i = 0; i < _timerArray.Length; i++)
            {
                _timerArray[i] = 0;
            }

            StartSpawningVehicles();

        }

        private void StartSpawningVehicles()
        {
            _keepSpawing = true;
            _spawingCoroutine = CoroutineHelper.Instance.StartCoroutine(StartSpawningVehiclesCoroutine());
        }

        private IEnumerator StartSpawningVehiclesCoroutine()
        {
            var wait = new WaitForSeconds(TimeBetweenSpawns);
            while (_keepSpawing)
            {
                _trafficSpawner.SpawnVehicle(new Vector3(11, 0, Random.Range(1, 6)), Direction.Left);
                
                
                

                yield return wait;
            }
        }

        private void StopSpawningVehicles()
        {
            _keepSpawing = false;
            CoroutineHelper.Instance.StopCoroutine(_spawingCoroutine);
        }




    }
}