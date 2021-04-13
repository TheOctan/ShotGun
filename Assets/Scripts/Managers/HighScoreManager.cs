using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using System.Collections.Generic;
using Assets.Scripts.Receivers;
using OctanGames.Experimental.Managers;

namespace OctanGames.Managers
{
	public class HighScoreManager : TimeoutBehaviour
	{
		public GameObject loadIndicator;
		public RectTransform content;
		public ScoreItemView scoreElementPrefab;

		[Header("Receiver")]
		public int scoreCount = 10;
		[SerializeField] private BaseScoreReceiver scoreReceiver = null;

		public static List<ScoreData> ScoreData { get; private set; } = new List<ScoreData>();

		private bool IsMarkPlayerScore = false;

		public void UpdateScore()
		{
			loadIndicator.SetActive(true);

			ClearScore();

			if (Experimental.ConfigurationManager.LoginData.IsLogined)
			{
				var login = Experimental.ConfigurationManager.LoginData.Nickname;
				var session = SessionManager.SessionData;

				InvokeTimeoutAction(scoreReceiver.GetScore(login, session, scoreCount, OnReceiveModels), scoreReceiver.ConnectionTimeout);
			}
			else
			{
				InvokeTimeoutAction(scoreReceiver.GetScore(scoreCount, OnReceiveModels), scoreReceiver.ConnectionTimeout);
			}
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
				var instance = Instantiate(scoreElementPrefab, content);
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

			var nickname = Experimental.ConfigurationManager.LoginData.Nickname;
			var score = SessionManager.SessionData.Score;

			if (Experimental.ConfigurationManager.LoginData.IsLogined && !IsMarkPlayerScore && data.Name == nickname && data.Score == score)
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