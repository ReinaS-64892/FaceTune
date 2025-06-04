using UnityEditorInternal;
using com.aoyon.facetune.Settings;
using System.IO;

namespace com.aoyon.facetune.ui;

internal static class AssetsMenu
{
    private const string BasePath = "Assets/FaceTune/";


    private const string Assets_SelectedClipToFacialExpressionPath = BasePath + "SelectedClipToFacialExpression";
    [MenuItem(Assets_SelectedClipToFacialExpressionPath, true)]
    private static bool ValidateSelectedClipToFacialExpression()
    {
        return Selection.objects.Any(i => i != null && i is AnimationClip);
    }

    [MenuItem(Assets_SelectedClipToFacialExpressionPath, false)]
    private static void SelectedClipToFacialExpression()
    {
        var exportTargetPath = EditorUtility.OpenFolderPanel("FacialExpression save path", "Assets", ""); ;
        if (string.IsNullOrWhiteSpace(exportTargetPath)) { return; }

        foreach (var clip in Selection.objects.UnityOfType<AnimationClip>())
        {
            var name = clip.name + "(converted from clip).prefab";
            var go = new GameObject(name, typeof(FacialExpressionComponent));
            var fe = go.GetComponent<FacialExpressionComponent>();

            var extractedShapeSet = BlendShapeUtility.GetBlendShapeSetFromClip(clip, ClipExcludeOption.ExcludeZeroWeight, new());
            FacialExpressionEditorUtility.UpdateShapes(fe, extractedShapeSet.BlendShapes.ToList());

            PrefabUtility.SaveAsPrefabAsset(go, Path.Combine(exportTargetPath, name));
            GameObject.DestroyImmediate(go, false);
        }
    }
}
