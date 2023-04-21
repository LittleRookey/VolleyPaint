using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DarkTonic.MasterAudio;

public class PlayerMovement : NetworkBehaviour
{
	public float mouseSensitivityX = 1.0f;
	public float mouseSensitivityY = 1.0f;

	public float walkSpeed = 10.0f;
	public float jumpForce = 15f;

	public int extraJumps = 1;
	private int jumpsLeft = 0;

	public bool canMove;

	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;

	Transform cameraT;
	float verticalLookRotation;

	Rigidbody rigidbodyR;

	bool grounded;
	public LayerMask groundedMask;
	bool cursorVisible;

	Animator anim;

	// Use this for initialization
	void Start()
	{
		cameraT = Camera.main.transform;
		rigidbodyR = GetComponent<Rigidbody>();
		LockMouse();
		anim = transform.Find("unitychan").GetComponent<Animator>();
	}
	public override void OnNetworkSpawn()
	{
		if (!IsOwner)
		{
			transform.Find("Main Camera").gameObject.GetComponent<Camera>().enabled = false;
			transform.Find("Main Camera").gameObject.GetComponent<AudioListener>().enabled = false;
			this.enabled = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// rotation
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

		// movement
		if (canMove)
        {
			Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
			Vector3 targetMoveAmount = moveDir * walkSpeed;
			moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

			// Toggle on/off running sound
			if (moveAmount.magnitude >= 0.2f && grounded)
            {
				MasterAudio.PlaySound3DAtVector3("Running", transform.position);
			}
			else
            {
				MasterAudio.StopAllOfSound("Running");
			}

        }

		// jump
		if (Input.GetButtonDown("Jump"))
		{
			if (grounded)
			{
				rigidbodyR.AddForce(transform.up * jumpForce);
			}
			else if (jumpsLeft > 0)
            {
				--jumpsLeft;
				rigidbodyR.velocity = new Vector3(rigidbodyR.velocity.x, 0f, rigidbodyR.velocity.z); // zero the y velocity before starting midair jump
				rigidbodyR.AddForce(transform.up * jumpForce);
			}
		}

		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
		{
			if (grounded == false) // Play landing sound upon immediately touching ground
            {
				MasterAudio.PlaySound3DAtVector3("Landing", transform.position);
			}
			grounded = true;
			jumpsLeft = extraJumps;
		}
		else
		{
			grounded = false;
		}

        /* Lock/unlock mouse on click */
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!cursorVisible)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }
        }

		UpdateAnimationStates();
    }

	void FixedUpdate()
	{
		if (canMove)
			rigidbodyR.MovePosition(rigidbodyR.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

	void UnlockMouse()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		cursorVisible = true;
	}

	void LockMouse()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cursorVisible = false;
	}

	void UpdateAnimationStates()
    {
		if (moveAmount.magnitude > 0.2f)
        {
			anim.SetBool("Running", true);
        }
		else
        {
			anim.SetBool("Running", false);
        }

		if (grounded)
        {
			anim.SetBool("Jumping", false);
        }
		else
        {
			anim.SetBool("Jumping", true);
        }
    }
}
