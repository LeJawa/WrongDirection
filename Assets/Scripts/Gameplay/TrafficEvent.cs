using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class TrafficEvent
    {
        public int NumberOfWaves { get; private set; }

        public readonly bool[,] vehiclesPerLaneAndWave;

        public float TimeBetweenWaves { get; private set; }

        public TrafficEvent(int numberOfWaves, float timeBetweenWaves)
        {
            NumberOfWaves = numberOfWaves;
            
            vehiclesPerLaneAndWave = new bool[NumberOfWaves,5];

            var minVehiclesPerWave = 2;
            var maxVehiclesPerWave = 4;
            
            RandomizeWavesWithMinAndMaxVehiclesPerWave(minVehiclesPerWave, maxVehiclesPerWave);

            TimeBetweenWaves = timeBetweenWaves;
        }

        private void RandomizeWavesWithMinAndMaxVehiclesPerWave(int minVehiclesPerWave, int maxVehiclesPerWave)
        {
            for (int wave = 0; wave < NumberOfWaves; wave++)
            {
                int vehiclesInWave = Random.Range(minVehiclesPerWave, maxVehiclesPerWave);

                for (int vehicles = 0; vehicles < vehiclesInWave; vehicles++)
                {
                    int lane = Random.Range(0, 5);
                    while (vehiclesPerLaneAndWave[wave, lane])
                    {
                        lane = Random.Range(0, 5);
                    }

                    vehiclesPerLaneAndWave[wave, lane] = true;
                }
            }
        }
    }
}