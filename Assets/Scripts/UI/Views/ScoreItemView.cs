using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItemView : MonoBehaviour
{
	[SerializeField] private Image background;
	[SerializeField] private Text positionText;
	[SerializeField] private Text nameText;
	[SerializeField] private Text scoreText;

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
