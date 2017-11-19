using UnityEngine;
using System.Collections;
using UnityEditor;
using Pamux.Lib.Procedural.Data;

namespace Pamux.Lib.Procedural.Editors
{
    [CustomEditor(typeof(UpdatableData), true)]
    public class UpdatableDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var data = target as UpdatableData;

            if (GUILayout.Button("Update"))
            {
                data.NotifyOfUpdatedValues();
                EditorUtility.SetDirty(target);
            }
        }
    }
}