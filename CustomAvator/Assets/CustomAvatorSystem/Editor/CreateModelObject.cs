/**********************************************************
*Author: wangjiaying
*Date: 2016.8.3
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CustomAvator.Editor
{
    public class CreateModelObject
    {
        [MenuItem("Assets/Extract Model Object")]
        public static void CreateModel()
        {
            GameObject o = Selection.activeGameObject;
            if (!o) return;
            ModelImporter importer = ModelImporter.GetAtPath(AssetDatabase.GetAssetPath(o)) as ModelImporter;
            if (!importer)
            {
                EditorUtility.DisplayDialog("Error!", "Model Import Failed!", "OK");
                return;
            }
            //AvatarBuilder.BuildHumanAvatar
        }
    }
}