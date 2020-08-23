using System.Collections;
using Cinemachine;
using GravityGames.MizJam1.Background;
using TMPro;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class StoryManager : MonoBehaviour
    {
        public CinemachineVirtualCamera cam1;
        public CinemachineVirtualCamera cam2;
        
        public int cameraChangeIndex = 3;
        
        public RoadManager RoadManager;

        public TMP_Text[] storyTextArray;

        public float[] waitTimeInSeconds;

        private bool _roadsDestroyed = false;
        
        public void StartStory()
        {
            cam1.Priority = 150;
            StartCoroutine(StoryCoroutine());
        }

        private IEnumerator StoryCoroutine()
        {
            yield return new WaitForSecondsRealtime(1.5f);
            storyTextArray[0].enabled = true;

            if (!_roadsDestroyed)
            {
                RoadManager.FadeOutRoads(18f);
                _roadsDestroyed = true;
            }
            
            yield return new WaitForSecondsRealtime(waitTimeInSeconds[0]);

            for (int i = 1; i < storyTextArray.Length; i++)
            {
                storyTextArray[i-1].enabled = false;

                if (i == cameraChangeIndex)
                {
                    cam1.Priority = -1;
                    cam2.Priority = 150;
                    yield return new WaitForSecondsRealtime(1f);
                }
                
                storyTextArray[i].enabled = true;
                
                yield return new WaitForSecondsRealtime(waitTimeInSeconds[i]);
            }
            
            GameEvents.Instance.TriggerStoryEndedEvent();
            
            cam2.Priority = -2;
            storyTextArray[storyTextArray.Length -1].enabled = false;

        }
        
        
    }
}