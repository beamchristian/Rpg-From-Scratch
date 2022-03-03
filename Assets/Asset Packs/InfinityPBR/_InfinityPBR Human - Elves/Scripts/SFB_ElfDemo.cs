using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_ElfDemo : MonoBehaviour {

    public Animator[] Animators;
    public SkinnedMeshRenderer MaleBody;
    public SkinnedMeshRenderer MaleHead;
    public SkinnedMeshRenderer FemaleBody;
    public SkinnedMeshRenderer FemaleHead;
    public Material[] MaleMaterials;
    public Material[] FemaleMaterials;

    Material m_Material;
    Material[] m_Materials;

    public void SetCombatStyle(float value)
    {
        StartCoroutine(DelayOneSecond(value));
    }

    public void SetLocomotion(float value)
    {
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].SetFloat("Locomotion", value);
        }
    }

    IEnumerator DelayOneSecond(float value)
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].SetFloat("CombatStyle", value);
        }
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
