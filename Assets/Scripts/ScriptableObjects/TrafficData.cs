using UnityEngine;

namespace GravityGames.MizJam1.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TrafficData", menuName = "New traffic data", order = 0)]
    public class TrafficData : ScriptableObject
    {
        public float timeBetweenSpawns = 0.5f;
    }
}