using GravityGames.MizJam1.Controllers;
using UnityEngine;
using Lean.Pool;

namespace GravityGames.MizJam1.Gameplay
{
    public class TrafficSpawner
    {
        private GameObject prefabSaucer1;
        private GameObject prefabShip1;


        private GameObject _tempGO;

        private Random _random;


        public TrafficSpawner()
        {
            prefabSaucer1 = Resources.Load<GameObject>(@"Prefabs\Saucer1");
            prefabShip1 = Resources.Load<GameObject>(@"Prefabs\Ship1");
        }


        public void SpawnVehicle(Vector3 position, Direction direction)
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    _tempGO = LeanPool.Spawn(prefabSaucer1);
                    break;
                case 1:
                    _tempGO = LeanPool.Spawn(prefabShip1);
                    break;
                default:
                    _tempGO = LeanPool.Spawn(prefabSaucer1);
                    break;
            }

            _tempGO.transform.position =  position;

            if (direction == Direction.Right)
            {
                // _tempGO.transform.localScale = new Vector3(-1, 1, 1);
            }

            _tempGO.GetComponent<Vehicle>().StartMoving(direction);
            
        }

        public void DespawnVehicle(Vehicle vehicle)
        {
            LeanPool.Despawn(vehicle.gameObject);
        }
    }
}