using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExportAssetBundles
{

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
    static void ExportResource()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", Selection.activeObject.name + "." + Globals.BUNDLEVERSION, "assetbundle");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            // Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            // BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            // Selection.objects = selection;

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.Android);
        }
    }
}
