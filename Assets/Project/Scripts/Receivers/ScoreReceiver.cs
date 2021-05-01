using Assets.Scripts.Data;
using SaveSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OctanGames.Receivers
{
	[CreateAssetMenu(menuName = "Receiver/Score")]
	public class ScoreReceiver : BaseScoreReceiver
	{
		public const string SaveKey = "Score";

		public override IEnumerator GetScore(int maxCount, Action<IEnumerable<ScoreData>> callback)
		{
			yield return new WaitForSecondsRealtime(1f);

			if (SaveSystem.HasKey(SaveKey))
			{
				callback(SaveSystem.Load<List<ScoreData>>(SaveKey).OrderByDescending(e => e.Score).Take(maxCount));
			}
			else
			{
				callback(new List<ScoreData>());
			}
		}

		public override IEnumerator GetScore(int maxCount, Action<IEnumerable<ScoreData>> callback, ScoreData score)
		{
			yield return new WaitForSecondsRealtime(1f);

			if (SaveSystem.HasKey(SaveKey))
			{
				var loadedScore = SaveSystem.Load<List<ScoreData>>(SaveKey);
				loadedScore.Add(score);

				callback(loadedScore.OrderByDescending(e => e.Score).Take(maxCount));
				SaveSystem.Save(SaveKey, loadedScore);
			}
			else
			{
				var saveScore = new List<ScoreData> { score };
				callback(saveScore);
				SaveSystem.Save(SaveKey, saveScore);
			}

		}
	}
}