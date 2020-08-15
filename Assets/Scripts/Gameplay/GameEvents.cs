using System;

namespace GravityGames.MizJam1.Gameplay
{
    public class GameEvents
    {
        #region Singleton pattern
        private static GameEvents _current;
        public static GameEvents Instance
        {
            get
            {
                if (_current == null)
                {
                    _current = new GameEvents();
                }
                return _current;
            }
        }
        #endregion
        
        #region Action<int> OnSignalLane
        public event Action<int, float> OnSignalLane;

        public void TriggerSignalLaneEvent(int lane, float duration)
        {
            OnSignalLane?.Invoke(lane, duration);
        }
        #endregion
        
        

    }
}