using UnityEngine;
using UnityEngine.Serialization;

namespace GravityGames.MizJam1.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "New Vehicle", order = 0)]
    public class VehicleData : ScriptableObject
    {
        public Sprite sprite;
        public float mass;

        public int points;
    }
}