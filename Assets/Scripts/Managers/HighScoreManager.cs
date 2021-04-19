using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using System.Collections.Generic;
using OctanGames.Experimental.Managers;
using System;

namespace OctanGames.Managers
{
	public class HighScoreManager : TimeoutBehaviour
	{
		public GameObject loadIndicator;
		public RectTransform content;
		public ScoreItemView scoreElementPrefab;

		[Header("Receiver")]
		public int scoreCount = 10;
		public string defaultNickname = "Player 1";
		[SerializeField] private BaseScoreReceiver scoreReceiver = null;

		public static List<ScoreData> ScoreData { get; private set; } = new List<ScoreData>();

		private bool IsMarkPlayerScore = false;

		public void UpdateScore()
		{
			loadIndicator.SetActive(true);
			ClearScore();

			IEnumerator scoreRoutine;
			if (ScoreKeeperManager.score != 0)
			{
				scoreRoutine = scoreReceiver.GetScore(scoreCount, OnReceiveModels, new ScoreData() { Name = defaultNickname, Score = ScoreKeeperManager.score });
			}
			else
			{
				scoreRoutine = scoreReceiver.GetScore(scoreCount, OnReceiveModels);
			}

			InvokeTimeoutAction(scoreRoutine, scoreReceiver.ConnectionTimeout);
		}

		protected override void OnTimeout()
		{
			base.OnTimeout();
			loadIndicator.SetActive(false);
		}

		private void OnReceiveModels(IEnumerable<ScoreData> models)
		{
			if (timeoutCoroutine != null)
				StopCoroutine(timeoutCoroutine);

			foreach (var model in models)
			{
				ScoreItemView instance = Instantiate(scoreElementPrefab, content);
				InitializeScoreItemView(instance, model);
			}

			loadIndicator.SetActive(false);
		}
		private void ClearScore()
		{
			foreach (Transform child in content)
			{
				Destroy(child.gameObject);
			}
			content.DetachChildren();
		}
		private void InitializeScoreItemView(ScoreItemView itemView, ScoreData data)
		{
			itemView.Position = (itemView.transform.GetSiblingIndex() + 1).ToString();
			itemView.Name = data.Name;
			itemView.Score = data.Score.ToString("D6");

			var score = SessionManager.SessionData.Score;

			if (!IsMarkPlayerScore && data.Score == score)
			{
				IsMarkPlayerScore = true;
				itemView.BackgroundColor = new Color(0.2f, 0.5f, 0.2f, 0.5f);
			}
			else
			{
				itemView.Transparency = content.childCount % 2 == 0 ? 0f : 0.5f;
			}
		}
	}
}