using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCameraMovement : MonoBehaviour {
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;
	public float minimumX = -60F;
	public float maximumX = 60F;
	public float minimumY = -80F;
	public float maximumY = 80F;
	float rotationY = 0F;
	public GameObject fpsCamera;
	public bool altPressed;
	public Quaternion lastRotation;
	GUITexture gt;
	bool wasLocked = false;
	
	public GameObject leftArm;
	public GameObject rightArm;

	// Use this for initialization
	void Start () {
		
		gt = GetComponent<GUITexture>();
	}
	void DidLockCursor()
	{
		Debug.Log("Locking cursor");

		// Disable the button
		gt.enabled = false;
	}
	
	void DidUnlockCursor()
	{
		Debug.Log("Unlocking cursor");

		// Show the button again
		gt.enabled = true;
	}
	void OnMouseDown()
	{
		// Lock the cursor
		Screen.lockCursor = true;
	}
	// Update is called once per frame
	void Update()
	{

		if (GameStatus.firstPerson == true)
		{
			//-91.8,-10.25,81.07
			
			if (fpsCamera.transform.rotation.x < -60)
			{
				fpsCamera.transform.Rotate(-60,fpsCamera.transform.rotation.y,fpsCamera.transform.rotation.z);
			} else if (fpsCamera.transform.rotation.x > 60)
			{
				fpsCamera.transform.Rotate(60,fpsCamera.transform.rotation.y,fpsCamera.transform.rotation.z);
			}
			if (axes == RotationAxes.MouseXAndY)
			{
				
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				

				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
				
				
				leftArm.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
				rightArm.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}
}
