using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float X,Y,Z;
	void Start () {	
	}
	void Update () {
	transform.Rotate(new Vector3(X,Y,Z));
	}
}
