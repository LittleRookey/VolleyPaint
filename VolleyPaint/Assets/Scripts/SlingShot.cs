using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blobcreate.Universal;
using Blobcreate.ProjectileToolkit;

[CreateAssetMenu(menuName = "Litkey/Ability/Projectile/SlingShot")]
public class SlingShot : Ability
{
	[Header("Specific Settings")]
	//public Transform launchPoint;
	//public Rigidbody bulletPrefab;
	public LayerMask groundMask;
	public float torqueForce = 0f;
	public float smallA = -0.1f;
	public float bigA = -0.01f;
	public float lerpSpeed = 5f;
	private TrajectoryPredictor trajectory;
	public bool drawLine;
	public bool faceDirection;
	float currentA;
	float currentTorque;

	private GameObject ball;
	private Rigidbody rb;

	//private TrajectoryPredictor trajectorySpawned;

	// Player shoots some kind of projectile or ray
	// if it hits the ball, stop the ball for a while, player gains access to shoot the ball in the direction player is looking at
	// and shoot the ball
    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
		ball = GameObject.FindGameObjectWithTag("Ball");

		if (ball != null) 
		{
			rb = ball.GetComponent<Rigidbody>();
			ball.GetComponent<ProjectileBehaviour>().faceDirection = faceDirection;
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
		}
		currentA = smallA;
		OnFireButtonDown();

		if (drawLine)
        {
			if (ball != null)
            {
				trajectory = ball.GetComponentInChildren<TrajectoryPredictor>();
				trajectory.enabled = false;
            }
        }
	}

    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
		OnFireButton();
		DrawPredictionLine();
	}

	// fire the projectile
    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
		OnFireButtonUp();
		trajectory.enabled = false;
	}

	private void DrawPredictionLine()
    {
		if (drawLine)
        {
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
						out var hit, 300f, groundMask);
			RenderLaunch(ball.transform.position, hit.point);
			trajectory.enabled = true;
        }
	}
	public void RenderLaunch(Vector3 origin, Vector3 target)
	{
		var v = Projectile.VelocityByA(origin, target, currentA);
		trajectory.Render(origin, v, target, 16);
	}

	public void Fire(Vector3 target)
	{
		//var b = Instantiate(bulletPrefab, launchPoint.position, launchPoint.rotation);
		if (ball == null)
        {
			Debug.LogWarning("Ball is null");
			return;
        }
		ball.GetComponent<ProjectileBehaviour>().Launch(target);

		// Magic happens!
		var f = Projectile.VelocityByA(ball.transform.position, target, currentA);
		rb.AddForce(f, ForceMode.VelocityChange);

        // Add some torque, not necessary, but interesting.
        var t = Vector3.Lerp(torqueForce * Random.onUnitSphere,
            torqueForce * (target - ball.transform.position).normalized, currentTorque);
        rb.AddTorque(t, ForceMode.VelocityChange);
    }


	void OnFireButtonDown()
	{
		currentA = smallA;
		currentTorque = 0f;
	}

	void OnFireButton()
	{
		currentA = Mathf.Lerp(currentA, bigA, lerpSpeed * Time.deltaTime);
		currentTorque = Mathf.Lerp(currentTorque, 1f, lerpSpeed * Time.deltaTime);
	}

	void OnFireButtonUp()
	{
		rb.isKinematic = false;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 300f, groundMask);
		//Fire(hit.point);
		ball.GetComponent<BallBehaviour>().FireServerRPC(hit.point, currentA);
		currentA = smallA;
		currentTorque = 0f;
	}
}
