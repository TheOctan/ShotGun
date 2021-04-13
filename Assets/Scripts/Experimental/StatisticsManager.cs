using OctanGames.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace OctanGames.Experimental
{
	public class StatisticsManager : MonoBehaviour
	{
		public RectTransform[] statistics;

		public enum LinePosionn
		{
			ShootCount = 0,
			ReloadCoutn,
			TraveledDistance,
			Duration,
			Date,
			ShootDamage,
			HitDamage
		}

		private void OnEnable()
		{
			SetLineValue(LinePosionn.ShootCount, SessionManager.SessionData.ShotCount.ToString());
			SetLineValue(LinePosionn.ReloadCoutn, SessionManager.SessionData.ReloadCount.ToString());
			SetLineValue(LinePosionn.TraveledDistance, ((int)SessionManager.SessionData.TraveledDistance).ToString() + "m");
			SetLineValue(LinePosionn.Duration, FormatTimeFromSeconds(SessionManager.SessionData.Duration, ':'));
			SetLineValue(LinePosionn.Date, SessionManager.SessionData.Date.ToString("dd/MM/yyyy"));
			SetLineValue(LinePosionn.ShootDamage, SessionManager.SessionData.ShotDamage.ToString());
			SetLineValue(LinePosionn.HitDamage, SessionManager.SessionData.HitDamage.ToString());
		}

		private void SetLineValue(LinePosionn position, string value)
		{
			var line = statistics[(int)position];
			line.GetChild(1).GetComponent<Text>().text = value;
		}

		private string FormatTimeFromSeconds(float seconds, char splitter)
		{
			var time = TimeSpan.FromSeconds(seconds);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(time.Minutes.ToString("D2"));
			stringBuilder.Append(splitter);
			stringBuilder.Append(time.Seconds.ToString("D2"));

			return stringBuilder.ToString();
		}
	}
}