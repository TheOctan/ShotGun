﻿using OctanGames.Entities.Player;
using OctanGames.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OctanGames.UI
{
	public class GameUI : MonoBehaviour
	{
		public static bool GameIsPaused = false;
		public bool HierarhyMenu { get; set; } = false;

		[Header("UI groups")]
		public Image fadePlane;
		public GameObject allUI;
		public GameObject gameOverUI;
		public GameObject pauseMenuUI;

		[Header("UI elements")]
		public RectTransform newWaveBanner;
		public Text newWaveTitle;
		public Text newWaveEnemyCount;
		public Text scoreUI;
		public Text gameOverScoreUI;
		public RectTransform healthBar;

		[SerializeField]
		private GameOverEvent GameOverEvent;

		private Spawner spawner;
		private PlayerHealth player;

		private bool isGameOver = false;

		private void Awake()
		{
			spawner = FindObjectOfType<Spawner>();
		}
		private void Start()
		{
			allUI.SetActive(true);
			player = FindObjectOfType<PlayerHealth>();
		}
		private void Update()
		{
			scoreUI.text = ScoreKeeperManager.score.ToString("D6");
			float healthPercent = 0;
			if (player != null)
			{
				healthPercent = player.Health / player.startingHealth;
			}
			healthBar.localScale = new Vector3(healthPercent, 1, 1);

			if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
			{
				if (!HierarhyMenu)
				{
					if (GameIsPaused)
					{
						Resume();
					}
					else
					{
						Pause();
					}
				}
			}
		}

		public void OnNewWave(int waveNumber)
		{
			string[] numbers = { "One", "Two", "Three", "Four", "Five" };
			newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
			string enemyCountString = spawner.waves[waveNumber - 1].infinit ? "Infinit" : spawner.waves[waveNumber - 1].enemyCount.ToString();
			newWaveEnemyCount.text = "Enemies: " + enemyCountString;

			StopCoroutine("AnimateNewWaveBanner");
			StartCoroutine("AnimateNewWaveBanner");
		}
		public void OnGameOver()
		{
			Cursor.visible = true;
			StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, 0.75f), 1));
			gameOverScoreUI.text = scoreUI.text;

			scoreUI.gameObject.SetActive(false);
			healthBar.transform.parent.gameObject.SetActive(false);
			gameOverUI.SetActive(true);

			GameOverEvent.Invoke();

			isGameOver = true;
		}
		public void OnRestart()
		{
			SceneManager.LoadScene("Game");
		}
		public void OnReturnMainMenu()
		{
			GameIsPaused = false;
			Time.timeScale = 1f;
			SceneManager.LoadScene("Menu");
		}
		public void OnExit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
		}

		private void Resume()
		{
			Cursor.visible = false;
			pauseMenuUI.SetActive(false);
			Time.timeScale = 1f;
			GameIsPaused = false;
		}
		private void Pause()
		{
			Cursor.visible = true;
			pauseMenuUI.SetActive(true);
			Time.timeScale = 0f;
			GameIsPaused = true;
		}

		private IEnumerator AnimateNewWaveBanner()
		{
			float delayTime = 1f;
			float speed = 3f;
			float animationPercent = 0;
			int dir = 1;

			float endDelayTime = Time.time + 1 / speed + delayTime;

			while (animationPercent >= 0)
			{
				animationPercent += Time.deltaTime * speed * dir;

				if (animationPercent >= 1)
				{
					animationPercent = 1;
					if (Time.time > endDelayTime)
					{
						dir = -1;
					}
				}

				newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-175, 45, animationPercent);
				yield return null;
			}
		}
		private IEnumerator Fade(Color from, Color to, float time)
		{
			float speed = 1 / time;
			float percent = 0;

			while (percent < 1)
			{
				percent += Time.deltaTime * speed;
				fadePlane.color = Color.Lerp(from, to, percent);
				yield return null;
			}
		}
	}

	[System.Serializable]
	public class GameOverEvent : UnityEvent
	{

	}
}