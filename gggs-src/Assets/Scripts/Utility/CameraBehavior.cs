﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

  private Transform target;
  private Transform root;

  [SerializeField]
  private float followSpeed = 3f;
  [SerializeField]
  private float rotateSpeed = 3f;
  [SerializeField]
  private float distance = 3f;

  [SerializeField]
  private float minLookX;
  [SerializeField]
  private float maxLookX;

  private Vector3 velocity = Vector3.zero;
  private float distanceNoise = 0.25f;
  [SerializeField]
  private float distanceNoiseRate;

  private Controls controls;
  private BallMovement ballMovement;

  private void OnEnable() {
    controls = Controls.DefaultBindings();
  }

  private void Awake() {
    target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    ballMovement = target.GetComponent<BallMovement>();
    root = transform.root;
  }

  private void FixedUpdate() {
    if (!target) return;

    RootTransform();
    Rotate();
    Follow();
  }

  private void RootTransform() {
    root.position = target.position;
  }

  private void Follow() {
    float _distanceNoise = Mathf.PerlinNoise(distanceNoise += 0.01f, distanceNoise += 0.01f);
    _distanceNoise *= Time.deltaTime * distanceNoiseRate;
    float _distance = (distance * (ballMovement.currentSpeed / 110)) + (10 + _distanceNoise);
    Debug.Log(_distance);

    Vector3 pos = root.rotation * Vector3.forward + root.position;
    pos += root.transform.forward * -_distance;

    transform.position = pos;
  }

  private void Rotate() {
    float currentRotationAngleY = root.eulerAngles.y;
    float currentRotationAngleX = root.eulerAngles.x;
    float currentRotationAngleZ = root.eulerAngles.z;

    float zRotationModifier = 0;
    if (controls.Look.X > 0) {

    }

    float wantedRotationAngleY = root.eulerAngles.y
    + controls.Look.X * 100 * rotateSpeed * Time.deltaTime;
    float wantedRotationAngleX = root.eulerAngles.x
    + controls.Look.Y * 100 * -rotateSpeed * Time.deltaTime;
    float wantedRotationAngleZ = 0
    + controls.Look.X * 10 * -(rotateSpeed * 0.8f) * Time.deltaTime;

    if (wantedRotationAngleX > 180) {
      wantedRotationAngleX = wantedRotationAngleX - 360;
    }

    if (wantedRotationAngleX >= maxLookX) {
      wantedRotationAngleX = maxLookX;
    } else if (wantedRotationAngleX <= minLookX) {
      wantedRotationAngleX = minLookX;
    }

    wantedRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotateSpeed * Time.deltaTime);
    wantedRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotateSpeed * Time.deltaTime);
    wantedRotationAngleZ = Mathf.LerpAngle(currentRotationAngleZ, wantedRotationAngleZ, rotateSpeed * Time.deltaTime);

    var currentRotation = Quaternion.Euler(wantedRotationAngleX, wantedRotationAngleY, wantedRotationAngleZ);
    root.rotation = currentRotation;
  }

}