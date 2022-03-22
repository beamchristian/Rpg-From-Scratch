/*  
    11/09/2020
    support: David Olshefski - http://deenvironment.com/
    support: Daniel Wipf - daniel@twigly.ch 
    https://www.youtube.com/watch?v=4nC8WjPFnGU
*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
// DE Drawer Toggle No Keyword
/// </summary>
public class DE_DrawerToggleNoKeyword : MaterialPropertyDrawer
{

    public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
    {
        bool value = (prop.floatValue != 0.0f);

        EditorGUI.BeginChangeCheck();
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            value = EditorGUI.Toggle(position, label, value);
            EditorGUI.showMixedValue = false;
        }
        if (EditorGUI.EndChangeCheck())
        {
            prop.floatValue = value ? 1.0f : 0.0f;
        }
    }
}
#endif