using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(InputField))]
public class InputController : MonoBehaviour
{
	public bool IsValid { get; private set; } = true;

    private InputField inputField;
	private Image image;

	private void Awake()
	{
		inputField = GetComponent<InputField>();
		image = GetComponent<Image>();
	}

	public void OnValueChangedInputField(string text)
	{
		image.color = Color.white;
	}

	public void OnEndInputField(string text)
	{
		if (text.Length < 3 && text != string.Empty)
		{
			image.color = Color.red;
			IsValid = false;
		}
		else
		{
			IsValid = true;
		}
	}
}
