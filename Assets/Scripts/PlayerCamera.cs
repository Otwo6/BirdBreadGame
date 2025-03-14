using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCam : NetworkBehaviour
{
	public float sensX;
	public float sensY;

	public Transform orientation;

	public Camera cam;

	private float xRotation = 0f;
	private float yRotation = 0f;

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (!IsOwner) {
			cam.enabled = false;
		}
	}

	private void Update() {

		if (!IsOwner) return;

		// Ensure the camera is enabled for the owner
		if (!cam.enabled) {
			cam.enabled = true;
		}

		// Mouse Input
		float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
		float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

		yRotation += mouseX;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
		
	}
}