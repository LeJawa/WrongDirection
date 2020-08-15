using UnityEngine;

namespace GravityGames.MizJam1.Utils
{
    public class CoroutineHelper : MonoBehaviour
    {
        #region SINGLETON PATTERN
        private static CoroutineHelper _current;

        public static CoroutineHelper Instance
        {
            get
            {
                if (_current != null) return _current;
                Instance = GameObject.FindObjectOfType<CoroutineHelper>();
                if (_current != null) return _current;
                var container = new GameObject("CoroutineHelper");
                Instance = container.AddComponent<CoroutineHelper>();
                return _current;
            }
            private set => _current = value;
        }
        #endregion
        
        
        private void Start()
        {
            _current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}