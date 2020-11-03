using UnityEngine;
using System.Collections;

public class OptionMenu: MonoBehaviour
{
	public InputController nicknameInputController;

	private void OnEnable()
	{
		var nickname = ConfigurationManager.configData.Nickname;
		if(nickname != null)
		{
			nicknameInputController.SetValue(nickname);
		}
	}
}
