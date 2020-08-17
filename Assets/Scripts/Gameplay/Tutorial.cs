using System;
using Lean.Common;
using UnityEngine;

namespace GravityGames.MizJam1.Gameplay
{
    public class Tutorial : MonoBehaviour
    {
        private bool wpressed = false;
        private bool spressed = false;

        private void Update()
        {
            if (LeanInput.GetDown(KeyCode.W))
            {
                wpressed = true;
            }

            if (LeanInput.GetDown(KeyCode.S))
            {
                spressed = true;
            }

            if (spressed && wpressed)
            {
                Destroy(gameObject, 1f);
            }
        }
    }
}