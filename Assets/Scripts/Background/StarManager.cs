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

        private void Start()
        {
            GenerateStars();
        }

        public void GenerateStars()
        {
            _starTransforms = new Transform[numberOfStars];
            
            _distances = new float[numberOfStars];
            for (int i = 0; i < _distances.Length; i++)
            {
                _distances[i] = Random.Range(minDistanceFromCamera, maxDistanceFromCamera);
            }
            
            Camera cameraMain = Camera.main;
            
            for (int i = 0; i < _starTransforms.Length; i++)
            {
                Ray ray = cameraMain.ScreenPointToRay(new Vector2(Random.Range(0, 1920), Random.Range(0, 1080)));
                
                _starTransforms[i] = GameObject.Instantiate(prefabStar, ray.GetPoint(_distances[i]), cameraMain.transform.rotation).transform;
                _starTransforms[i].parent = transform;
            }
            
        }

        private void Update()
        {
            Vector3 tmp;
            
            for (int i = 0; i < _starTransforms.Length; i++)
            {
                tmp = _starTransforms[i].position;
                tmp.x -= Time.deltaTime * scrollingSpeed / _distances[i];

                _starTransforms[i].position = tmp;
            }
        }
    }
}