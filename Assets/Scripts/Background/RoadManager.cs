using UnityEngine;

namespace GravityGames.MizJam1.Background
{
    public class RoadManager : MonoBehaviour
    {
        public GameObject[] RoadTiles;
        public float scrollingSpeed;
        
        
        private void Update()
        {
            Vector3 tmp;
            
            foreach (var roadTile in RoadTiles)
            {
                tmp = roadTile.transform.position;
                tmp.x -= Time.deltaTime * scrollingSpeed;
 
                if (tmp.x < -80 )
                {
                    tmp.x += 160;
                }

                roadTile.transform.position = tmp;
            }
        }

    }
}