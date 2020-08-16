using System.Collections.Generic;
using UnityEngine;

namespace GravityGames.MizJam1.Background
{
    public class BackgroundManager : MonoBehaviour
    {
        public GameObject prefabRoadLights;

        private List<GameObject> _roadLights = new List<GameObject>();
        public Transform roadLightContainer;

        public float scrollingSpeed = 5f;
        public int numberOfRoadLights = 5;
        public float rightMostPositionOfRoadLight = 12f;

        private void Start()
        {
            GenerateRoadLights();
        }

        public void GenerateRoadLights()
        {
            ClearRoadLights();

            float xPos = rightMostPositionOfRoadLight;
            float deltaPos = rightMostPositionOfRoadLight * 2 / numberOfRoadLights;

            for (int i = 0; i < numberOfRoadLights; i++)
            {
                var roadLights = GameObject.Instantiate(prefabRoadLights, new Vector3(xPos, 0, 0), Quaternion.identity);
                roadLights.transform.parent = roadLightContainer;
                _roadLights.Add(roadLights);

                xPos -= deltaPos;
            }
        }

        private void ClearRoadLights()
        {
            _roadLights.Clear();
            if (Application.isEditor)
            {
                DestroyImmediate(roadLightContainer.gameObject);
            }
            else
            {
                Destroy(roadLightContainer.gameObject);
            }

            var go = new GameObject("RoadLightContainer");
            
            roadLightContainer = Instantiate(go, transform).transform;
            
            if (Application.isEditor)
            {
                DestroyImmediate(go);
            }
            else
            {
                Destroy(go);
            }
        }


        private void Update()
        {
            Vector3 tmp;
            
            foreach (var roadLight in _roadLights)
            {
                tmp = roadLight.transform.position;
                tmp.x -= Time.deltaTime * scrollingSpeed;
 
                if (tmp.x < -rightMostPositionOfRoadLight )
                {
                    tmp.x += rightMostPositionOfRoadLight * 2;
                }

                roadLight.transform.position = tmp;
            }
        }
    }
}