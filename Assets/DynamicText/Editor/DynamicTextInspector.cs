/// Copyright 2014,2015 Jetro Lauha (Strobotnik Ltd)

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DynamicText)), CanEditMultipleObjects]
public class DynamicTextInspector : Editor
{

    public override void OnInspectorGUI()
    {
        Color defaultColor = GUI.color;
        Color warningColor = new Color(1, 0.25f, 0.25f, 1);
        DynamicText dt = (DynamicText)target;

        serializedObject.Update();

        bool prefabs = false;
        if (!serializedObject.isEditingMultipleObjects)
        {
            prefabs = (PrefabUtility.GetPrefabType(dt) == PrefabType.Prefab);
        }
        else
        {
            foreach (DynamicText t in targets)
            {
                if (PrefabUtility.GetPrefabType(t) == PrefabType.Prefab)
                    prefabs = true;
            }
        }

        if (prefabs)
        {
            EditorGUILayout.HelpBox("(No camera reference in prefabs)", MessageType.Info);
        }
        else
        {
            if (dt.cam == null)
                GUI.color = warningColor;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cam"));
            GUI.color = defaultColor;
            if (dt.cam == null)
                EditorGUILayout.HelpBox("Camera reference is missing!", MessageType.Error);
        }


        EditorGUILayout.LabelField("Text");
        string text = "—"; // shown when editing multiple objects
        if (!serializedObject.isEditingMultipleObjects)
        {
            if (dt.internal_GetVersion() < 1024 && prefabs) // backward compatibility
                text = dt.internal_GetDeprecatedText();
            else
            {
                if (prefabs)
                    text = dt.serializedText;
                else
                    text = dt.GetText();
            }
        }
        string newText = EditorGUILayout.TextArea(text);
        if (!text.Equals(newText))
        {
            foreach (DynamicText t in targets)
            {
                serializedObject.FindProperty("serializedText").stringValue = newText;
                if (PrefabUtility.GetPrefabType(t) != PrefabType.Prefab)
                    t.SetText(newText);
            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("offsetZ"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lineSpacing"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("letterSpacing"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anchor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("alignment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tabSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fontStyle"));

        if (dt.font == null)
            GUI.color = warningColor;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
        GUI.color = defaultColor;
        if (dt.font == null)
            EditorGUILayout.HelpBox("Font is missing!", MessageType.Error);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoSetFontMaterial"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baselineRefChar"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("metricsRefChars"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pixelSnapTransformPos"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minFontPxSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFontPxSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFaceCam"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
