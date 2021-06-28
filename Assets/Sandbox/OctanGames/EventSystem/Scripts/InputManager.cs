using OctanGames.ScriptableEvents.Events.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OctanGames
{
	public class InputManager : MonoBehaviour
    {
        public BaseGameEvent<bool> InputEvent;

        private bool trigger = true;

    	private void Update()
    	{
			if (Keyboard.current[Key.Space].wasPressedThisFrame)
			{
                trigger = !trigger;
                InputEvent.Raise(trigger);
			}
    	}
    }
}
