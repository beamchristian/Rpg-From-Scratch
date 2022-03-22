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
/// DE Drawer Texture Single Line
/// </summary>
public class DE_DrawerTextureSingleLine : MaterialPropertyDrawer
{
	public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor editor)
	{
		EditorGUI.BeginChangeCheck();
		EditorGUI.showMixedValue = prop.hasMixedValue;

		Texture value = editor.TexturePropertyMiniThumbnail(position, prop, label, string.Empty);

		EditorGUI.showMixedValue = false;
		if (EditorGUI.EndChangeCheck())
		{
			prop.textureValue = value;
		}
	}
}
#endif