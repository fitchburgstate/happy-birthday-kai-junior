﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

  private Transform player;
  private Rigidbody rb;

  private Transform cam;
  [SerializeField]
  private float speed;

  private Controls controls;

  private Vector3 dir;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    GameObject camObj = GameObject.Find("SmoothCameraFollow");
    cam = camObj.transform;
  }


	private void Start() {
    player = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();

	}
	
	private void Update() {
		dir = controls.Move;
	}

  private void FixedUpdate() {
    if (dir != Vector3.zero && DataManager.AllowControl) {
      rb.AddForce(dir.x * speed * cam.transform.right);
      rb.AddForce(dir.y * speed * cam.transform.forward);

      Debug.Log(cam.transform.forward);

      // forward needs a flat thing! child an object under the camera that is always 0,0,0 for rotation
    }
  }
}