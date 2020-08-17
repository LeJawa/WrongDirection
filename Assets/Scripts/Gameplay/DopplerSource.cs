using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class DopplerSource : MonoBehaviour
    {
        public AudioSource doppler;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                doppler.pitch = Random.Range(0.95f, 1.03f);
                doppler.PlayOneShot(doppler.clip);
                Debug.Log("doppler");
            }
        }
    }
}