using OctanGames.Entities.Player;
using OctanGames.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OctanGames.UI
{
	public class GameUI : MonoBehaviour
	{
		public static bool IsPaused = false;

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

		[Header("Events")]
		[SerializeField] private GameOverEvent gameOverEvent;
		[SerializeField] private TogglePauseEvent togglePauseEvent;

		private Spawner spawner;
		private PlayerHealth player;

		public void TogglePlayerPause(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				TogglePauseState();
			}
		}
		public void TogglePauseState()
		{
			IsPaused = !IsPaused;

			ToggleTimeScale();
			ToggleUI();
			togglePauseEvent.Invoke(IsPaused);
		}
		public void SelectButton(Selectable selectable)
		{
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);
			selectable.Select();
			selectable.OnSelect(null);
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

			gameOverEvent.Invoke();
		}
		public void OnRestart()
		{
			SceneManager.LoadScene("Game");
		}
		public void OnReturnMainMenu()
		{
			IsPaused = false;
			Time.timeScale = 1f;
			SceneManager.LoadScene("ModernMainMenu");
		}
		public void OnExit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
		}

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
			UpdateUI();
		}

		private void UpdateUI()
		{
			scoreUI.text = ScoreKeeperManager.score.ToString("D6");
			float healthPercent = 0;
			if (player != null)
			{
				healthPercent = player.Health / player.startingHealth;
			}
			healthBar.localScale = new Vector3(healthPercent, 1, 1);
		}
		private void ToggleTimeScale()
		{
			Time.timeScale = IsPaused ? 0f : 1f;
		}
		private void ToggleUI()
		{
			Cursor.visible = IsPaused;
			pauseMenuUI.SetActive(IsPaused);
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

	[System.Serializable]
	public class TogglePauseEvent : UnityEvent<bool>
	{
	}
}