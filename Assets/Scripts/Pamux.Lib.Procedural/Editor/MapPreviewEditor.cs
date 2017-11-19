using UnityEngine;
using System.Collections;
using UnityEditor;
using Pamux.Lib.Procedural;

namespace Pamux.Lib.Procedural.Editors
{
    [CustomEditor (typeof(MapPreview))]
    public class MapPreviewEditor : Editor {
        public MapPreviewEditor()
        {

        }
        public override void OnInspectorGUI() {
		    MapPreview mapPreview = (MapPreview)target;

		    if (DrawDefaultInspector ()) {
			    if (mapPreview.autoUpdate) {
				    mapPreview.DrawMapInEditor ();
			    }
		    }

		    if (GUILayout.Button ("Generate")) {
			    mapPreview.DrawMapInEditor();
		    }
	    }
    }
}