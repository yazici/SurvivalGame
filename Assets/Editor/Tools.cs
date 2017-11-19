using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using Pamux.Lib.GameObjects;
using Pamux.Lib.WorldGen;

namespace Pamux.Lib.Editors
{
    [CustomEditor(typeof(AssetLibrary)), CanEditMultipleObjects]
    public class AssetLibraryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var assetLibrary = (AssetLibrary)target;

            var prefabs = serializedObject.FindProperty("prefabs");
            var sizeProperty = prefabs.FindPropertyRelative("Array.size");
            EditorGUILayout.LabelField(sizeProperty.intValue.ToString() + " prefabs attached.");

            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(Taggable)), CanEditMultipleObjects]
    public class TaggableEditor : Editor
    {
        [Flags]
        public enum EditorListOption
        {
            None = 0,
            ListSize = 1,
            ListLabel = 2,
            ElementLabels = 4,
            Buttons = 8,
            Default = ListSize | ListLabel | ElementLabels,
            NoElementLabels = ListSize | ListLabel,
            All = Default | Buttons

        }
        

        private static void ShowElement(SerializedProperty element, EditorListOption options)
        {
            var tagProperty = element.FindPropertyRelative("Tag");
            tagProperty.stringValue = EditorGUILayout.TextField(tagProperty.stringValue);

            var weightProperty = element.FindPropertyRelative("Weight");
            weightProperty.floatValue = EditorGUILayout.FloatField(weightProperty.floatValue);

        }

        private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
        private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
        private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
        private static GUIContent addButtonContent = new GUIContent("+", "add element");
        private static void ShowElements(SerializedProperty list, EditorListOption options)
        {
            bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
            bool showButtons = (options & EditorListOption.Buttons) != 0;


            for (int i = 0; i < list.arraySize; ++i)
            {
                if (showButtons)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                ShowElement(list.GetArrayElementAtIndex(i), options);

                
                if (showButtons)
                {
                    ShowButtons(list, i);

                    EditorGUILayout.EndHorizontal();
                }
            }
            if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
            }

        }
        private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
        private static void ShowButtons(SerializedProperty list, int index)
        {
            if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
            {
                list.MoveArrayElement(index, index + 1);
            }
            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
            {
                list.InsertArrayElementAtIndex(index);
            }
            if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
            {
                int oldSize = list.arraySize;
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == oldSize)
                {
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }

        public static class EditorList
        {

            public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.All)
            {
                bool showListLabel = (options & EditorListOption.ListLabel) != 0;
                bool showListSize = (options & EditorListOption.ListSize) != 0;

                int oldIndentLevel = EditorGUI.indentLevel;
                if (showListLabel)
                {
                    EditorGUILayout.PropertyField(list);
                    EditorGUI.indentLevel += 1;
                }
                if (!showListLabel || list.isExpanded)
                {
                    if (showListSize)
                    {
                        EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                    }

                    ShowElements(list, options);
                }

                EditorGUI.indentLevel = oldIndentLevel;
            }
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var taggable = (Taggable) target;

            taggable.DontUseAsTagged = EditorGUILayout.Toggle("Don't Use As Tagged", taggable.DontUseAsTagged);
            EditorGUILayout.LabelField("Opinion", taggable.Opinion);
            EditorGUILayout.LabelField("Path", taggable.PrefabAssetPath);

            EditorList.Show(serializedObject.FindProperty("WeightedTags"));

            serializedObject.ApplyModifiedProperties();

            //GUILayout.BeginHorizontal();
            //    GUILayout.Space(20);
            //    GUILayout.BeginVertical();

            //foreach (var tag in taggable.WeightedTags)
            //{
            //    EditorGUILayout.TextField("Tag", tag.Tag);
            //    EditorGUILayout.FloatField("Weight", tag.Weight);
            //    if (GUILayout.Button("-"))
            //    {

            //    }
            //    //}
            //    if (GUILayout.Button("Add Tag"))
            //    {

            //    }


            //    GUILayout.EndVertical();
            //GUILayout.EndHorizontal();

            //EditorGUILayout.FloatField(weightedTag.Weight);
        }
    }

    public static class Tools
    {
        [MenuItem("Pamux/List Pamux Scripts", false, 0)]
        static public void ListPamuxScripts()
        {
            //string[] searchInFolders  = new string[] { "Assets/Scripts", "Assets/Audio", "Assets/Materials", "Assets/Models", "Assets/Prefabs", "Assets/Resources", "Assets/Scenes", "Assets/Scripts", "Assets/Shaders", "Assets/SpreadSheets", "Assets/Standard Assets", "Assets/StreamingAssets", "Assets/Textures" };
            string[] searchInFolders = new string[] { "Assets/Scripts" };

            var guids = AssetDatabase.FindAssets(@"", searchInFolders);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);


                Debug.Log("Asset: " + path);
            }


        }

        //[MenuItem("Pamux/Save User Decisions", false, 0)]
        //static public void SaveUserDecisions()
        //{
        //    GameObject[] prefabs = null ;
        //    var opinions = new AssetOpinions();
        //    var path = Application.persistentDataPath + "/assetYesNo.dat";
        //    Debug.Log(path);

        //    DoForEachTaggedPrefab(prefabs, (prefab, taggable) => {
        //        opinions.Add(taggable);
        //    });

        //    var serializer = new DataContractJsonSerializer(typeof(AssetOpinions));

        //    using (var file = File.CreateText(path))
        //    {
        //        file.Write(opinions.ToJSON<AssetOpinions>());
        //    }
        //}

        [MenuItem("Pamux/Rename Assets", false, 0)]
        static public void RenameAssets()
        {
            Debug.Assert(false);
            //string[] searchInFolders  = new string[] { "Assets/Scripts", "Assets/Audio", "Assets/Materials", "Assets/Models", "Assets/Prefabs", "Assets/Resources", "Assets/Scenes", "Assets/Scripts", "Assets/Shaders", "Assets/SpreadSheets", "Assets/Standard Assets", "Assets/StreamingAssets", "Assets/Textures" };
            //string[] searchInFolders = new string[] { "Assets/Scripts", "Assets/Scripts/Pamux/Social", "Assets/Scripts/Pamux/Platforms", "Assets/Scripts/Shooter/UI" };
            string[] searchInFolders = new string[] { "Assets/Scripts/Pamux/Social" };

            var guids = AssetDatabase.FindAssets(@"", searchInFolders);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                int lastSlashIndex = path.LastIndexOf("/");
                int dotIndex = path.LastIndexOf(".");
                if (dotIndex == -1)
                {
                    continue;
                }
                var assetPath = path.Substring(0, lastSlashIndex);
                var assetName = path.Substring(lastSlashIndex + 1, dotIndex - lastSlashIndex - 1);



                if (assetName.Contains("_"))
                {
                    int underscoreIndex = assetName.LastIndexOf("_");
                    var newAssetName = assetName.Substring(underscoreIndex + 1);
                    Debug.Log(assetPath + "/" + assetName + ".cs ->" + newAssetName);
                    Debug.Log(AssetDatabase.RenameAsset(assetPath + "/" + assetName + ".cs", newAssetName));
                }
            }
        }


        [MenuItem("Pamux/Move Assets", false, 0)]
        static public void MoveAssets()
        {
            Debug.Assert(false);
            //string[] searchInFolders  = new string[] { "Assets/Scripts", "Assets/Audio", "Assets/Materials", "Assets/Models", "Assets/Prefabs", "Assets/Resources", "Assets/Scenes", "Assets/Scripts", "Assets/Shaders", "Assets/SpreadSheets", "Assets/Standard Assets", "Assets/StreamingAssets", "Assets/Textures" };
            //string[] searchInFolders = new string[] { "Assets/Scripts", "Assets/Scripts/Pamux/Social", "Assets/Scripts/Pamux/Platforms", "Assets/Scripts/Shooter/UI" };
            string[] searchInFolders = new string[] { "Assets/Scripts/Pamux/Social" };

            var guids = AssetDatabase.FindAssets(@"", searchInFolders);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                int lastSlashIndex = path.LastIndexOf("/");
                var assetName = path.Substring(lastSlashIndex + 1);

                //Debug.Log("Assets/Scripts/Pamux/Social/" + assetName + ".cs ->" + "Assets/Scripts/Pamux/SocialNetworks/" + assetName);
                Debug.Log(AssetDatabase.MoveAsset("Assets/Scripts/Pamux/Social/" + assetName, "Assets/Scripts/Pamux/SocialNetworks/" + assetName));
            }
        }



        [MenuItem("Pamux/Insert Namespaces", false, 0)]
        static public void InsertNamespaces()
        {
            Debug.Assert(false);
            string[] searchInFolders = new string[] { "Assets/Scripts", "Assets/Scripts/Pamux/Social", "Assets/Scripts/Pamux/Platforms", "Assets/Scripts/Shooter/UI" };

            var guids = AssetDatabase.FindAssets(@"", searchInFolders);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".cs"))
                {
                    InsertNamespace(@"D:\Workspace\Unity\Zodiac\" + path.Replace('/', '\\'));
                }
            }
        }

        private static void InsertNamespace(string path)
        {
            Debug.Log(path);
            var file = new StreamReader(path);
            string line;
            StringBuilder sb = new StringBuilder();

            bool insertedNamespace = false;
            string indent = "";
            while ((line = file.ReadLine()) != null)
            {
                if (!insertedNamespace)
                {
                    var trimmed = line.Trim();
                    if (trimmed != "" && !trimmed.StartsWith("using"))
                    {
                        sb.AppendLine("namespace Pamux");
                        sb.AppendLine("{");
                        insertedNamespace = true;
                        indent = "  ";
                    }
                }
                sb.AppendLine(indent + line.TrimEnd());
            }

            file.Close();

            if (insertedNamespace)
            {
                sb.AppendLine("}");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}
