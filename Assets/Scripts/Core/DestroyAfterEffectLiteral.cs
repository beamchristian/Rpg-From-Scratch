using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.Core
{
    public class DestroyAfterEffectLiteral : MonoBehaviour 
    {
        [SerializeField] GameObject targetToDestroy = null;

        void Update() 
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}