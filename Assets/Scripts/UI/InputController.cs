using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(InputField))]
public class InputController : MonoBehaviour
{
	public string Text { get => inputField.text; set => inputField.text = value; }

	public InputField inputField;
	public Image image;

	public void OnValueChangedInputField()
	{
		image.color = Color.white;
	}
}
