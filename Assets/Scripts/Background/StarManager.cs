using UnityEngine;
using Random = UnityEngine.Random;

namespace GravityGames.MizJam1.Background
{
    public class StarManager : MonoBehaviour
    {
        public Camera cameraMain;
        public float minDistanceFromCamera = 40f;
        public float maxDistanceFromCamera = 150f;
        public int numberOfStars = 10;
        public float scrollingSpeed = 5;

        public GameObject prefabStar;
        
        private Transform[] _starTransforms;
        private float[] _distances;
        private float[] _xLimits;

        private void Start()
        {
            GenerateStars();
        }

        public void GenerateStars()
        {
            _starTransforms = new Transform[numberOfStars];
            
            _distances = new float[numberOfStars];
            for (int i = 0; i < numberOfStars; i++)
            {
                _distances[i] = Random.Range(minDistanceFromCamera, maxDistanceFromCamera);
            }
            
            for (int i = 0; i < numberOfStars; i++)
            {
                Ray ray = cameraMain.ScreenPointToRay(new Vector2(Random.Range(0, 1920), Random.Range(0, 1080)));
                
                _starTransforms[i] = GameObject.Instantiate(prefabStar, ray.GetPoint(_distances[i]), cameraMain.transform.rotation).transform;
                _starTransforms[i].parent = transform;
            }
            
            _xLimits = new float[numberOfStars];

            Vector3 cameraPos = cameraMain.transform.position;

            for (int i = 0; i < numberOfStars; i++)
            {
                Vector3 pos = _starTransforms[i].position - cameraPos;

                // _xLimits[i] = Mathf.Sqrt(pos.y * pos.y + pos.z * pos.z) / Mathf.Tan((90 - cameraMain.fieldOfView) * Mathf.Deg2Rad);
                _xLimits[i] = Mathf.Sqrt(pos.y * pos.y + pos.z * pos.z) * 0.8f;
            }

        }

        private void Update()
        {
            Vector3 tmp;
            
            for (int i = 0; i < _starTransforms.Length; i++)
            {
                tmp = _starTransforms[i].position;
                tmp.x -= Time.deltaTime * scrollingSpeed / _distances[i];

                if (tmp.x < -_xLimits[i])
                {
                    tmp.x += _xLimits[i] * 2;
                }
                
                _starTransforms[i].position = tmp;
            }
        }
    }
}