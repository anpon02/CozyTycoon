using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Callbacks;

public class GizmoIconUtility {
    [DidReloadScripts]
    static GizmoIconUtility()
    {
        EditorApplication.projectWindowItemOnGUI = ItemOnGUI;
    }

    static void ItemOnGUI(string guid, Rect rect)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);

        Item obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Item)) as Item;

        if (obj != null) {
            rect.height *= 0.8f;
            rect.width = rect.height;

            var texture = obj.GetSprite().texture;
            if (texture == null) return;

            GUI.DrawTexture(rect, texture);
        }
    }
}