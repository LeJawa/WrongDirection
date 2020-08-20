using UnityEngine;

namespace GravityGames.MizJam1.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TrafficData", menuName = "New traffic data", order = 0)]
    public class TrafficData : ScriptableObject
    {
        public float timeBetweenSpawns = 0.5f;
        public float deltaTimeBetweenSpawns = .2f;

        public float fastVehicleSpawnWeight = 1;
        public float mediumVehicleSpawnWeight = 4;
        public float slowVehicleSpawnWeight = 2;

        public float speedIncreaseFactor = 1.1f;
        public float timeBetweenSpeedIncreases = 10f;
    }
}