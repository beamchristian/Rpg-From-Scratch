using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
        public class Projectile : MonoBehaviour
        {
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        // Used timed deletion instead because code referenced particle system
        // [SerializeField] GameObject[] destroyOnHit = null;
        // [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start() 
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);      
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 1.5f;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.GetComponent<Health>() != target) return;
            if(target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;
            
            onHit.Invoke();

            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            Destroy(gameObject);
        }

    }
}
