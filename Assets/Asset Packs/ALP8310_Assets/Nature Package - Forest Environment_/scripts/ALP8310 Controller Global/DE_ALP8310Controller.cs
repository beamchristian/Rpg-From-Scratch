/*
    11/06/2021
    support: David Olshefski - http://deenvironment.com/
    support: Aleksey Pakhalchyk - https://www.artstation.com/aleksey8310
*/

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

public static class GUIExtentions
{
    /// <summary>
    /// Editor GUI Property Field
    /// </summary>
    /// <param name="property">SerializedProperty Value</param>
    /// <param name="name">Displayed Name</param>
    /// <param name="tooltip">Tool Tip</param>
    /// <param name="style">Default GUI Style is EditorStyles.miniLabel</param>
    /// <returns></returns>
    public static void PropertyField(this SerializedProperty property, string name, string tooltip = "", GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.label;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent(name, tooltip), style, GUILayout.Width(Screen.width / 4));
        EditorGUILayout.PropertyField(property, GUIContent.none);
        GUILayout.Space(3);
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Editor GUI Float Slider Field
    /// </summary>
    /// <param name="property">SerializedProperty Value</param>
    /// <param name="min">Min Float Value</param>
    /// <param name="max">Max Float Value</param>
    /// <param name="name">Displayed Name</param>
    /// <param name="tooltip">Tool Tip</param>
    /// <param name="style">Default GUI Style is EditorStyles.miniLabel</param>
    /// <returns></returns>
    public static void SliderField(this SerializedProperty property, float min, float max, string name, string tooltip = "", GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.label;
        property.floatValue = EditorGUILayout.Slider(property.floatValue, min, max);
    }
}

#endif

namespace DE_ALP8310
{
    public static class ALP8310Extentions
    {
        /// <summary>
        /// Get Global float Shader Value
        /// </summary>
        /// <param name="property">Shader property name</param>
        /// <returns></returns>
        public static float GetGlobalFloat(this string property)
        {
            return Shader.GetGlobalFloat(property);
        }

        /// <summary>
        /// Set Global float Shader Value
        /// </summary>
        /// <param name="property">Shader property name</param>
        /// /// <param name="value">float value</param>
        /// <returns></returns>
        public static void SetGlobalFloat(this string property, float value)
        {
            Shader.SetGlobalFloat(property, value);
        }

        /// <summary>
        /// Set Global int Shader Value
        /// </summary>
        /// <param name="property">Shader property name</param>
        /// <param name="value">int value</param>
        /// <returns></returns>
        public static void SetGlobalInt(this string property, int value)
        {
            Shader.SetGlobalInt(property, value);
        }
    }
}