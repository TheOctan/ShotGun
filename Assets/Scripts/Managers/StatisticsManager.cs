using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
		SetLineValue(LinePosionn.ShootCount, SessionManager.sessionData.ShotCount.ToString());
		SetLineValue(LinePosionn.ReloadCoutn, SessionManager.sessionData.ReloadCount.ToString());
		SetLineValue(LinePosionn.TraveledDistance, ((int)(SessionManager.sessionData.TraveledDistance)).ToString() + "m");
		SetLineValue(LinePosionn.Duration, FormatTimeFromSeconds(SessionManager.sessionData.Duration, ':'));
		SetLineValue(LinePosionn.Date, SessionManager.sessionData.Date.ToString("dd/MM/yyyy"));
		SetLineValue(LinePosionn.ShootDamage, SessionManager.sessionData.ShotDamage.ToString());
		SetLineValue(LinePosionn.HitDamage, SessionManager.sessionData.HitDamage.ToString());
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
