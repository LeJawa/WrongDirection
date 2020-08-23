using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

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


        public void FadeOutRoads(float duration)
        {
            StartCoroutine(FadeOutRoadsCoroutine(duration));
        }

        private IEnumerator FadeOutRoadsCoroutine(float duration)
        {
            var steps = 100;
            
            WaitForSeconds wait = new WaitForSeconds(duration/steps);
            
            Tilemap[] tilemaps = new Tilemap[RoadTiles.Length];

            for (int i = 0; i < tilemaps.Length; i++)
            {
                tilemaps[i] = RoadTiles[i].GetComponent<Tilemap>();
            }
            
            Color color = new Color(1,1,1,1);

            for (int i = 0; i < steps; i++)
            {
                float alpha = 0.99f - i * 0.01f;
                color.a = alpha;
                
                foreach (var tilemap in tilemaps)
                {
                    tilemap.color = color;
                }

                yield return wait;
            }

            foreach (var roadTile in RoadTiles)
            {
                Destroy(roadTile);
            }
            
            Destroy(gameObject);
            
        }
    }
}