using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public GameObject Camera;
	public Vector3 targetPosition;
	public GameObject fpsCamera;
	public GameObject Target;
	public float height = 20;
	private Vector3 velocity = Vector3.zero;
	public float smoothTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		targetPosition= new Vector3(Target.transform.position.x,Target.transform.position.y+height,Target.transform.position.z);
		Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position,targetPosition,ref velocity,smoothTime);

		
		
	}
}
