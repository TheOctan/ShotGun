using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItemView : MonoBehaviour
{
	[SerializeField] private Image background = null;
	[SerializeField] private Text positionText = null;
	[SerializeField] private Text nameText = null;
	[SerializeField] private Text scoreText = null;

	public Color BackgroundColor { get => background.color; set => background.color = value; }
	public float Transparency
	{
		get => background.color.a;
		set
		{
			var color = background.color;
			color.a = value;
			background.color = color;
		}
	}

	public string Position { get => positionText.text; set => positionText.text = value; }
	public string Name { get => nameText.text; set => nameText.text = value; }
	public string Score { get => scoreText.text; set => scoreText.text = value; }
}
