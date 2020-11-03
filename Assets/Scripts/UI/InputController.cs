using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(InputField))]
public class InputController : MonoBehaviour
{
	public bool IsValid { get; private set; }
	public string text { get => inputField.text; }

	private InputField inputField;
	private Image image;

	private void Awake()
	{
		IsValid = true;
		inputField = GetComponent<InputField>();
		image = GetComponent<Image>();
	}

	public void OnValueChangedInputField(string text)
	{
		image.color = Color.white;
	}

	public void OnEndInputField(string text)
	{
		HandleValidation(text);
	}

	public void SetValue(string text)
	{
		HandleValidation(text);

		if (IsValid)
		{
			inputField.text = text;
		}
	}

	public void ResetValue()
	{
		inputField.text = string.Empty;
		IsValid = true;
	}

	private void HandleValidation(string text)
	{
		if (ValidateValue(text))
		{
			IsValid = true;
		}
		else
		{
			image.color = Color.red;
			IsValid = false;
		}
	}

	private bool ValidateValue(string text)
	{
		return text.Length > 3 || text == string.Empty;
	}
}
