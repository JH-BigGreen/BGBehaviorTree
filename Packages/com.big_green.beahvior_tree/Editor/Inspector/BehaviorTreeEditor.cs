//----------------------------------------
//author: BigGreen
//date: 2024-01-03 14:55
//----------------------------------------

using System;
using BG.BT;
using UnityEditor;
using UnityEngine;

namespace BG.BTEditor
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : Editor
    {
        private SerializedProperty m_AssetProperty;
        private void OnEnable()
        {
            m_AssetProperty = serializedObject.FindProperty("asset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_AssetProperty);
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();

            if (GUILayout.Button("Open Editor"))
            {
                var tree = (BehaviorTree) target;
                BTTreeWindow.OpenTreeEditor(tree.asset);
            }
        }
    }
}