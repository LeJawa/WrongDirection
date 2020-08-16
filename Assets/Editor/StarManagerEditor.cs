using GravityGames.MizJam1.Background;
using UnityEditor;
using UnityEngine;

namespace GravityGames.MizJam1.Editor
{
    [CustomEditor(typeof(StarManager))]
    public class StarManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Stars"))
            {
                ((StarManager)target).GenerateStars();
            }
            
        }
    }
}