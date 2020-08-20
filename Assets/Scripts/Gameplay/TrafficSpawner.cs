using GravityGames.MizJam1.Controllers;
using UnityEngine;
using Lean.Pool;

namespace GravityGames.MizJam1.Gameplay
{
    public class TrafficSpawner
    {
        private GameObject prefabSaucer1;
        private GameObject prefabSaucer2;
        private GameObject prefabShip1;
        private GameObject prefabTruck1;
        private GameObject prefabTruck2;

        private float _initialImpulseForce = 30;
        private float _impulseForce;
        
        private GameObject _tempGO;

        private Random _random;


        public TrafficSpawner()
        {
            prefabSaucer1 = Resources.Load<GameObject>(@"Prefabs\Saucer1");
            prefabSaucer2 = Resources.Load<GameObject>(@"Prefabs\Saucer2");
            prefabShip1 = Resources.Load<GameObject>(@"Prefabs\Ship1");
            prefabTruck1 = Resources.Load<GameObject>(@"Prefabs\Truck1");
            prefabTruck2 = Resources.Load<GameObject>(@"Prefabs\Truck2");

            _impulseForce = _initialImpulseForce;

            GameEvents.Instance.OnVehicleSpeedIncreased += speedFactor => _impulseForce *= speedFactor;
        }

        public void SpawnVehicle(GameObject prefabVehicle, Vector3 position)
        {
            _tempGO = LeanPool.Spawn(prefabVehicle);
            _tempGO.transform.position =  position;

            _tempGO.GetComponent<Vehicle>().StartMoving(_impulseForce);
            _tempGO.GetComponent<Collider>().enabled = true;

        }

        public void SpawnSlowVehicle(Vector3 position)
        {
            if (Random.Range(0,2) == 0)
            {
                SpawnVehicle(prefabTruck1, position);
            }
            else
            {
                SpawnVehicle(prefabTruck2, position);
            }
        }

        public void SpawnFastVehicle(Vector3 position)
        {
            SpawnVehicle(prefabShip1, position);
        }

        public void SpawnMediumVehicle(Vector3 position)
        {
            if (Random.Range(0,2) == 0)
            {
                SpawnVehicle(prefabSaucer1, position);
            }
            else
            {
                SpawnVehicle(prefabSaucer2, position);
            }
        }

        public void DespawnVehicle(Vehicle vehicle)
        {
            LeanPool.Despawn(vehicle.gameObject);
        }

        public void ResetVehicleInitialImpulse()
        {
            _impulseForce = _initialImpulseForce;
        }
    }
}