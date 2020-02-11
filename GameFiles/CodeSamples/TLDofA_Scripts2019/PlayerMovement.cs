using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float moveSpeed;
	public float runSpeed;
	public float walkSpeed;
	public float jumpSpeed;
	public float angle;
	public Vector3 mouse_pos;
	public GameObject fpsGun;
	public float sprint = 100;
	public Vector3 object_pos;
	public GameObject gun;
	public GameObject smgGun;
	public GameObject gunController;
	public GameObject player;
	public Camera cam;
	public bool playerMoving;
	public bool running;
	public Rigidbody playerRB;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0F;
	public SimpleHealthBar sprintBar;
	public Texture2D cursorTexture;
	public TextMeshProUGUI runText;
	
	public GameObject leftArm;
	public GameObject rightArm;
	public bool justSprinted;
	public bool gunInHand;
	
	public Animator animator;
//	Use this for initialization
	void Start ()
	{
		GameStatus.firstPerson = false;
		gunController.GetComponent<GunController>();
		fpsGun.GetComponent<FPSGunController>();
		animator = GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody>();
		Vector2 asads = new Vector2(10.5f,10.5f);
		Cursor.SetCursor(cursorTexture,asads,CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		float xMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		float zMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
		transform.Translate(xMovement, 0, zMovement,Space.World);
		Vector3 xzMovement = new Vector3(xMovement,0,zMovement);
		var movingSpeed = xzMovement.magnitude;
		
		//Stair Movement
		
		Debug.DrawRay(transform.position,transform.forward,Color.blue,0.5f);
		RaycastHit hit;
		Vector3 speed = GetComponent<Rigidbody>().velocity;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 0.4f) && movingSpeed != 0)
		{
			if (hit.collider.gameObject.CompareTag("Stairs"))
			{
				transform.transform.position = new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z);
			}
		}
		
		
		if (Input.GetKey(KeyCode.LeftShift) && sprint > 0 && movingSpeed > 0 && !justSprinted)
		{
			running = true;
			moveSpeed = runSpeed;
			animator.speed = 3;
			if (sprint <= 1)
			{
				justSprinted = true;
			}
		}
		
		else if (Input.GetKeyUp(KeyCode.LeftShift) || movingSpeed <= 0 || sprint <= 0 || justSprinted)
		{
			running = false;
			animator.speed = 1;
			moveSpeed = walkSpeed;
			if (sprint > 5)
			{
				justSprinted = false;
			}
		}


		if (moveSpeed == runSpeed && !GameStatus.sprintConst)
		{
			runText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower,0);
			sprint -= Time.deltaTime * 15;
			sprintBar.UpdateBar( sprint, 100 );
			runText.text = "Running: " + Mathf.RoundToInt(sprint) + " / 100";
		} else if (moveSpeed == walkSpeed&& sprint <= 100)
		{
			runText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower,0);
			sprint += Time.deltaTime* 8;
			sprintBar.UpdateBar( sprint, 100 );
			runText.text = "Running: " + Mathf.RoundToInt(sprint) + " / 100";
		}  else if (!GameStatus.sprintConst)
		{
			runText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower,0);
		}
		if (GameStatus.sprintConst)
		{
			if (sprint <= 100)
			{
				sprint += Time.deltaTime* 8;
				sprintBar.UpdateBar( sprint, 100 );	
			}
			runText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower,1);
			runText.text = "Unlimited Sprint for: "+ Mathf.Round(GameStatus.sprintCalc) + "/" + GameStatus.sprintCalculatorMax + " seconds";
		}

		if (sprint <= 0)
		{
			running = false;
			animator.speed = 1;
			moveSpeed = walkSpeed;
		}
		
		if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			animator.SetBool("Walk", true);
		}
		else
		{
			animator.SetBool("Walk", false);
		}
	/*
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (GameStatus.firstPerson == false)
			{
				
				//GameStatus.firstPerson = true;
				//fpsguncontroller pyörittää updatessa
			}
			else
			{
				
				GameStatus.firstPerson = false;
				gunController.SendMessage("UpdateHand");
			}
		}
*/
		if (GameStatus.firstPerson == false)
		{
			gun.SetActive(true);
			mouse_pos = Input.mousePosition;

			mouse_pos.z = 20; //The distance between the camera and object
			object_pos = cam.WorldToScreenPoint(player.transform.position);
			mouse_pos.x = mouse_pos.x - object_pos.x;
			mouse_pos.y = mouse_pos.y - object_pos.y;
			angle = -Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			if (!GameStatus.paused)
			{
				transform.rotation = Quaternion.Euler(new Vector3(0, angle + 90, 0));
			}

		}
		else
		{
			gun.SetActive(false);
			/*if (!Input.GetKey(KeyCode.LeftAlt))
			{*/
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
				//}
			}
		}
		animator.SetBool("Jump",false);
		RaycastHit ground;
		if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position,-transform.up,out ground,0.2f))
		{
			animator.SetBool("Jump",true);
			GetComponent<Rigidbody>().AddForce(player.transform.up * jumpSpeed,ForceMode.Impulse);
		}
		
		animator.SetBool("Reload",false);
	}

	
}
