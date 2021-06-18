using OctanGames.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OctanGames
{
    public class InputManager : MonoBehaviour
    {
        public GameEvent InputEvent;

    	private void Update()
    	{
			if (Keyboard.current[Key.Space].wasPressedThisFrame)
			{
                InputEvent.Raise();
			}
    	}
    }
}
