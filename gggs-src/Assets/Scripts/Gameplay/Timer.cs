﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

  private HUDManager hudManager;
  private GameFunctions gameFunctions;

  [SerializeField]
  private float defaultGameTime;
  [SerializeField]
  private float countDownTime;

  private float gameTime;
  private bool runTimer;
  private bool gameOverRun;

  private int _score;

  private void Awake() {
    hudManager = GetComponent<HUDManager>();
    gameFunctions = FindObjectOfType (typeof (GameFunctions)) as GameFunctions;

    DataManager.AllowControl = false;

    StartCoroutine(CountDownTimer(countDownTime));

    gameTime = defaultGameTime;
  }

  public void StartTimer() {
    Debug.Log("timer starting");
    runTimer = true;
    gameOverRun = false;
    DataManager.AllowControl = true;
  }

  private void Update() {
    TimerLoop();
  }

  private void TimerLoop() {

    if (gameTime > 0) {
      gameTime -= Time.deltaTime;
    } else if (!gameOverRun) {
      gameTime = 0;

      DataManager.AllowControl = false;
      DataManager.GameOver = true;

      StartCoroutine(GameOverDelay(3));

      gameOverRun = true;
    }

    hudManager.TimerChange(Mathf.Round(gameTime), defaultGameTime);
  }

  public IEnumerator GameOverDelay(float wait) {
    yield return new WaitForSeconds(wait);


    // @REFACTOR: this is just bad lol
    while (DataManager.ObjectIsStillMoving) {
      Debug.Log("starting wait for objects to stop moving");
      yield return new WaitForSeconds(1);
      // yield return null;
    }

    hudManager.GameOverDisplay();
    LockMouse.Lock(false);
  }

  private IEnumerator CountDownTimer(float countDownTime) {
    hudManager.OverlayText("");
    yield return new WaitForSeconds(1);
    float time = countDownTime;
    while (time > 0) {
      time -= Time.deltaTime;

      hudManager.OverlayText(Mathf.Round(time) + "");
      yield return null;
    }

    hudManager.HideOverlay();
    StartTimer();
  }

}
