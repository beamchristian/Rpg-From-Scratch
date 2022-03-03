using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf_ParticleControl : MonoBehaviour {

    public GameObject spearSmash;                                               // Magic spell
    public ParticleSystem[] castParticles;
    public Transform spearBase;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpearSmash()
    {
        GameObject newObject = Instantiate(spearSmash, spearBase.position, Quaternion.identity);
        Destroy(newObject, 5.0f);
    }

    public void CastStart()
    {
        for (int i = 0; i < castParticles.Length; i++)
        {
            castParticles[i].Play();
        }
    }

    public void CastEnd()
    {
        for (int i = 0; i < castParticles.Length; i++)
        {
            castParticles[i].Stop();
        }
    }
}
