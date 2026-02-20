using UnityEngine;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.Rendering.Universal
{
    class URPBasicScenePipeline : ISceneTemplatePipeline
    {
        void ISceneTemplatePipeline.AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName)
        {
            //To avoid problematic behavior and warnings in the future, let's remove all missing scripts monobehaviors. 
            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        }

        void ISceneTemplatePipeline.BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName)
        {
            string parentFolderName = "SceneTemplateAssets";
            string commonFolderName = "Common";
            string templateSpecificFolderName = sceneTemplateAsset.templateName;

            string completeTemplateSpecificFolderName = parentFolderName + "/" + sceneTemplateAsset.templateName;
            string completeCommonFolderName = parentFolderName + "/" + commonFolderName;

            Dictionary<string, string> filesToImport;

            switch (sceneTemplateAsset.templateName)
            {
                case "Basic (URP)":
                    // Nothing to import specifically.
                    break;
                case "Standard (URP)":
                    filesToImport = new Dictionary<string, string>();
                    filesToImport.Add("Packages/com.unity.render-pipelines.core/Samples~/Common/Models/UnityMaterialBall.fbx", completeCommonFolderName + "/Models/");
                    foreach (var kvp in filesToImport)
                    {
                        string destPath = "Assets/" + kvp.Value + Path.GetFileName(kvp.Key);
                        if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(kvp.Key) != null)
                            AssetDatabase.CopyAsset(kvp.Key, destPath);
                    }
                    break;
                default:
                    break;
            }

            AssetDatabase.Refresh();
        }

        bool ISceneTemplatePipeline.IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset)
        {

            return true;
        }
    }
}
