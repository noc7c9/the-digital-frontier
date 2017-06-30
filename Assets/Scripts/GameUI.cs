﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Defines all UI Behaviour
 */
public class GameUI : MonoBehaviour {

    public Image fadeScreen;
    public GameObject gameOverUI;

    public RectTransform newWaveBanner;
    public Text newWaveNumber;
    public Text newWaveEnemyCount;

    public float fadeTime;

    Spawner spawner;

    void Awake() {
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;

        FindObjectOfType<PlayerController>().OnDeath += OnGameOver;
    }

    void OnNewWave(int waveNumber) {
        Spawner.Wave wave = spawner.waves[waveNumber - 1];

        if (wave.infinite) {
            newWaveNumber.text = "- Last Wave -";
            newWaveEnemyCount.text = "Enemies: Infinite";
        } else {
            newWaveNumber.text = "- Wave: " + NumberToWord(waveNumber) + " -";
            newWaveEnemyCount.text = "Enemies: " + wave.enemyCount;
        }

        DisplayNewWaveBanner();
    }

    IEnumerator currentNewWaveCoroutine;
    void DisplayNewWaveBanner() {
        if (currentNewWaveCoroutine != null) {
            StopCoroutine(currentNewWaveCoroutine);
        }
        StartCoroutine(currentNewWaveCoroutine = AnimateBanner());
    }

    IEnumerator AnimateBanner() {
        float visibleY = -200;
        float hiddenY = -450;
        float delay = 2f;
        float speed = 1.5f;
        float percent = 0;

        // display banner
        newWaveBanner.gameObject.SetActive(true);
        while (percent <= 1) {
            percent += Time.deltaTime * speed;

            newWaveBanner.anchoredPosition = Vector2.up
                * Mathf.Lerp(hiddenY, visibleY, percent);
            yield return null;
        }

        // wait a moment
        yield return new WaitForSeconds(delay);

        // hide banner
        while (percent >= 0) {
            percent -= Time.deltaTime * speed;

            newWaveBanner.anchoredPosition = Vector2.up
                * Mathf.Lerp(hiddenY, visibleY, percent);
            yield return null;
        }
        newWaveBanner.gameObject.SetActive(false);
    }

    string NumberToWord(int num) {
        string[] words = {"One", "Two", "Three", "Four", "Five"};
        return words[num-1];
    }

    void OnGameOver() {
        StartCoroutine(Fade(Color.clear, Color.black, fadeTime));
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time) {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime * speed;
            fadeScreen.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI input
    public void StartNewGame() {
        SceneManager.LoadScene("Main");
    }

}
