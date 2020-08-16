using System;
using GravityGames.MizJam1.Controllers;

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
        
        #region Action<int, float> OnSignalLane
        public event Action<int, float> OnSignalLane;

        public void TriggerSignalLaneEvent(int lane, float duration)
        {
            OnSignalLane?.Invoke(lane, duration);
        }
        #endregion
        
        #region Action<Vehicle> OnDespawnVehicle
        public event Action<Vehicle> OnDespawnVehicle;

        public void TriggerDespawnVehicleEvent(Vehicle vehicle)
        {
            OnDespawnVehicle?.Invoke(vehicle);
        }
        #endregion
        
        #region Action OnPlayerCrashed
        public event Action OnPlayerCrashed;

        public void TriggerPlayerCrashedEvent()
        {
            OnPlayerCrashed?.Invoke();
        }
        #endregion
        
        
        #region Action<Vehicle> OnPointBarrierCrossed
        public event Action<Vehicle> OnPointBarrierCrossed;

        public void TriggerointBarrierCrossedEvent(Vehicle vehicle)
        {
            OnPointBarrierCrossed?.Invoke(vehicle);
        }
        #endregion
        

    }
}