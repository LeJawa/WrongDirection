using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class AnimationManager
    {
        #region Singleton pattern

        private static AnimationManager _current;
        public static AnimationManager Instance
        {
            get
            {
                if (_current == null)
                {
                    _current = new AnimationManager();
                }
                return _current;
            }
        }
        #endregion
        
        
        private Animator _animationAnim;
        private static readonly int ClosePanel = Animator.StringToHash("ClosePanel");
        private static readonly int OpenPanel = Animator.StringToHash("OpenPanel");

        public Animator Animator => _animationAnim;
        
        public void StartClosePanelAnimation()
        {
            if (_animationAnim == null)
            {
                InitializeAnimationManager();
            }

            _animationAnim.SetTrigger(ClosePanel);
        }
        
        public void StartOpenPanelAnimation()
        {
            if (_animationAnim == null)
            {
                InitializeAnimationManager();
            }

            _animationAnim.SetTrigger(OpenPanel);
        }

        public void InitializeAnimationManager() {
            _animationAnim = GameObject.FindGameObjectWithTag("Panel").GetComponent<Animator>();
        }
    }
}