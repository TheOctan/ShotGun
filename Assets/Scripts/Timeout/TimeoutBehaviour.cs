using UnityEngine;
using System.Collections;

public abstract class TimeoutBehaviour : MonoBehaviour
{
	protected Coroutine actionCoroutine;
	protected Coroutine timeoutCoroutine;

	public void InvokeTimeoutAction(IEnumerator action, int timeout)
	{
		actionCoroutine = StartCoroutine(action);
		if (timeout > 0)
		{
			timeoutCoroutine = StartCoroutine(CheckTimeOut(timeout));
		}
	}
	private IEnumerator CheckTimeOut(int time)
	{
		yield return new WaitForSecondsRealtime(time);
		StopCoroutine(actionCoroutine);
		OnTimeout();
		yield return new WaitForSecondsRealtime(1);
		OnPostTimeout();
	}

	protected virtual void OnTimeout()
	{

	}

	protected virtual void OnPostTimeout()
	{

	}
}
