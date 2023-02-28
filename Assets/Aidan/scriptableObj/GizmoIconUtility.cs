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
            

            GUI.DrawTexture(rect, obj.GetSprite().texture);
        }
    }
}