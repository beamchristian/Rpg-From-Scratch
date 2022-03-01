using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_HalfOrcDemo : MonoBehaviour {

    public Animator[] Animators;
    public SkinnedMeshRenderer MaleBody;
    public SkinnedMeshRenderer MaleHead;
    public SkinnedMeshRenderer FemaleBody;
    public SkinnedMeshRenderer FemaleHead;
    public Material[] MaleMaterials;
    public Material[] FemaleMaterials;

    Material m_Material;
    Material[] m_Materials;

    public Color[] hairColors;
    public Material[] hairMat;
    public Renderer[] hairRenderer;

    public Renderer[] wardrobe;

    public Material[] wardrobeMaterials;

    public void Randomize()
    {
        SetArmorMaterials(Random.Range(1, 4));
        SetHairColor(Random.Range(0, hairColors.Length));
        SetHairColorB(Random.Range(0, hairColors.Length));
        SetBodyMaterial(Random.Range(0, MaleMaterials.Length));
    }

    public void SetArmorMaterials(int matSuffix)
    {
        for (int i = 0; i < wardrobe.Length; i++)
        {
            string matName = wardrobe[i].material.name;
           // Debug.Log("matName: " + matName);
            matName = matName.Replace("1", "");
            matName = matName.Replace("2", "");
            matName = matName.Replace("3", "");
            matName = matName.Replace(" (Instance)", "");
            matName = matName + matSuffix;
            //Debug.Log("NEW matName: " + matName);
            for (int m = 0; m < wardrobeMaterials.Length; m++)
            {
               // Debug.Log("Materials name: " + wardrobeMaterials[m].name);
                if (wardrobeMaterials[m].name == matName)
                {
                    wardrobe[i].material = wardrobeMaterials[m];
                    break;
                }
            }
        }
    }

    public void SetCombatStyle(bool value)
    {
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].SetBool("Combat", value);
        }
    }

    public void SetLocomotion(float value)
    {
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].SetFloat("Locomotion", value);
        }
    }

    public void SetHairColor(int colorIndex)
    {
        hairMat[0].SetColor("_Color", hairColors[colorIndex]);
        hairMat[0].SetColor("_HairColor", hairColors[colorIndex]);
    }

    public void SetHairColorB(int colorIndex)
    {
        hairMat[1].SetColor("_Color", hairColors[colorIndex]);
        hairMat[1].SetColor("_HairColor", hairColors[colorIndex]);
    }

    public void SetBodyMaterial(int matIndex)
    {
        // Male Body
        m_Material = MaleBody.material;
        m_Material = MaleMaterials[matIndex];
        MaleBody.material = m_Material;

        // Female Body
        m_Material = FemaleBody.material;
        m_Material = FemaleMaterials[matIndex];
        FemaleBody.material = m_Material;

        // Male Head
        m_Materials = MaleHead.materials;
        m_Materials[0] = MaleMaterials[matIndex];
        MaleHead.materials = m_Materials;

        // Female Head
        m_Materials = FemaleHead.materials;
        m_Materials[0] = FemaleMaterials[matIndex];
        FemaleHead.materials = m_Materials;
        //m_Materials = FemaleHead.materials;
        //m_Materials[2] = FemaleMaterials[matIndex];
        //FemaleHead.materials = m_Materials;
    }
}
