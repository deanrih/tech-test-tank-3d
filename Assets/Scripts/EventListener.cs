using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct EventParameter
{
	[SerializeField]
	public UnityEngine.Object Sender;
	[SerializeField]
	public String Argument;
}

public class EventListener : MonoBehaviour, IEventListener
{
	public void EmitEvent(UnityEngine.Object sender, String arg)
	{
		this.OnEventRaised_Impl(sender, arg);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnEventRaised_Impl(UnityEngine.Object sender, String arg)
	{
		this.response.Invoke(new() { Sender = sender, Argument = arg });
	}

#pragma warning disable IDE0051
	private void OnEnable()
	{
		this.source.AttachListener(this);
	}

	private void OnDisable()
	{
		this.source.DetachListener(this);
	}
#pragma warning restore IDE0051

	[SerializeField]
	protected EventSource source;

	[SerializeField]
	protected UnityEvent<EventParameter> response;
}