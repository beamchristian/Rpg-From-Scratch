using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
		public class DestroyAfterEffect : MonoBehaviour {

		[SerializeField] float destroyTime = 1;
		private float time = 0;

		void Update () 
		{
			time += Time.deltaTime;
			if (time > destroyTime)
			Destroy (gameObject);

		}
	}
}
