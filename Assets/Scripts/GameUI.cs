using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	public Image fadePlane;
	public GameObject gameOverUI;

	public RectTransform newWaveBanner;
	public Text newWaveTitle;
	public Text newWaveEnemyCount;

	private Spawner spawner;

	void Start()
	{
		FindObjectOfType<Player>().OnDeath += OnGameOver;
	}

	void Awake()
	{
		spawner = FindObjectOfType<Spawner>();
		spawner.OnNewWave += OnNewWave;
	}

	void OnNewWave(int waveNumber)
	{
		string[] numbers = { "One", "Two", "Three", "Four", "Five" };
		newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
		string enemyCountString = spawner.waves[waveNumber - 1].infinit ? "Infinit" : spawner.waves[waveNumber - 1].enemyCount.ToString();
		newWaveEnemyCount.text = "Enemies: " + enemyCountString;

		StopCoroutine("AnimateNewWaveBanner");
		StartCoroutine("AnimateNewWaveBanner");
	}

	void OnGameOver()
	{
		StartCoroutine(Fade(Color.clear, Color.black, 1));
		gameOverUI.SetActive(true);
	}

	IEnumerator AnimateNewWaveBanner()
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

			newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-170, 45, animationPercent);
			yield return null;
		}
	}

	IEnumerator Fade(Color from, Color to, float time)
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

	// UI Input
	public void StartNewGame()
	{
		SceneManager.LoadScene("Game");
	}
}
