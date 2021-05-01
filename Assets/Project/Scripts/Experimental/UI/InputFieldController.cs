using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OctanGames.Experimental.UI
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(InputField))]
	public class InputFieldController : MonoBehaviour
	{
		public string Text { get => inputField.text; set => inputField.text = value; }

		public InputField inputField;
		public Image image;

		public void OnValueChangedInputField()
		{
			image.color = Color.white;
		}
	}
}