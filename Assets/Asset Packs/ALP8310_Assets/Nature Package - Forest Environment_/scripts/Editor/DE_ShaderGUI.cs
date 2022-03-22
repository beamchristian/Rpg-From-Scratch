/*  
    10/13/2021
    support: David Olshefski - http://deenvironment.com/
    support: Cristian Pop - https://boxophobic.com/
*/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DE_ShaderGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var material0 = materialEditor.target as Material;
        //var materials = materialEditor.targets;

        //if (materials.Length > 1)
        //    multiSelection = true;

        DrawDynamicInspector(material0, materialEditor, props);

        //foreach (Material material in materials)
        //{
            
        //}
    }

    void DrawDynamicInspector(Material material, MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var customPropsList = new List<MaterialProperty>();

        for (int i = 0; i < props.Length; i++)
        {
            var prop = props[i];

            if (prop.flags == MaterialProperty.PropFlags.HideInInspector)
                continue;

            customPropsList.Add(prop);
        }

        //Draw Custom GUI
        for (int i = 0; i < customPropsList.Count; i++)
        {
            var prop = customPropsList[i];

            materialEditor.ShaderProperty(prop, prop.displayName);

        }

        GUILayout.Space(10);

        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();
    }
}

