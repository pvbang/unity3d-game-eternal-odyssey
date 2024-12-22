using UnityEditor;
using UnityEngine;
public class MoveAnimationKeyFrames : EditorWindow
{
    [MenuItem("Tools/Move Animation Keyframes to Frame 0")]
    public static void MoveKeyframesToZero()
    {
        // Lấy các AnimationClip đang được chọn
        var selectedClips = Selection.GetFiltered<AnimationClip>(SelectionMode.DeepAssets);
        if (selectedClips.Length == 0)
        {
            Debug.LogWarning("Không có Animation Clip nào được chọn!");
            return;
        }

        foreach (var clip in selectedClips)
        {
            var curves = AnimationUtility.GetAllCurves(clip, true);
            foreach (var curve in curves)
            {
                Keyframe[] keyframes = curve.curve.keys;

                // Tính toán khoảng cách để keyframe đầu tiên về 0
                float offset = keyframes[0].time;
                for (int i = 0; i < keyframes.Length; i++)
                {
                    keyframes[i].time -= offset;
                }

                // Áp dụng keyframes mới
                AnimationCurve newCurve = new AnimationCurve(keyframes);
                AnimationUtility.SetEditorCurve(clip, curve.path, curve.type, curve.propertyName, newCurve);
            }
            Debug.Log($"Keyframes đã được di chuyển về frame 0 trong clip: {clip.name}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
