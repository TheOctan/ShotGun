using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using System.Collections.Generic;
using Assets.Scripts.Receivers;

public class HighScoreManager: MonoBehaviour
{
	public GameObject loadIndicator;
	public RectTransform content;
	public ScoreItemView scoreElementPrefab;

	[Header("Receiver")]
	[SerializeField]
	private BaseReceiver receiver = null;

	public static List<ScoreData> ScoreData { get; private set; } = new List<ScoreData>();

	[ContextMenu("Update Score")]
	public void UpdateScore()
	{
		int modelCount = 20;
		loadIndicator.SetActive(true);

		ClearScore();
		StartCoroutine(receiver.GetItems(modelCount, OnReceiveModels));
	}

	private void OnReceiveModels(IEnumerable<ScoreData> models)
	{
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
		itemView.Transparency = content.childCount % 2 == 0 ? 0f : 0.5f;
	}
}
