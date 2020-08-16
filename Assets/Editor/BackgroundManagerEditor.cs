using GravityGames.MizJam1.Background;
using GravityGames.MizJam1.Gameplay;
using UnityEditor;
using UnityEngine;

namespace GravityGames.MizJam1.Editor
{
    [CustomEditor(typeof(BackgroundManager))]
    public class BackgroundManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Build Road Lights"))
            {
                ((BackgroundManager)target).GenerateRoadLights();
            }
            
        }
    }
}