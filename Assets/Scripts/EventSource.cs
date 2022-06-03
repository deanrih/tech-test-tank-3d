using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IEventListener
{
	void EmitEvent(UnityEngine.Object sender, String arg);
}

[CreateAssetMenu(fileName = "Event_", menuName = "Event/Event Source", order = 1)]
public class EventSource : ScriptableObject
{
	public void AttachListener(IEventListener listener)
	{
		this.AttachListener_Impl(listener);
	}

	public void DetachListener(IEventListener listener)
	{
		this.DetachListener_Impl(listener);
	}

	public void Raise(String arg)
	{
		this.Raise_Impl(arg);
	}

	public void Raise(UnityEngine.Object sender, String arg)
	{
		this.Raise_Impl(sender, arg);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AttachListener_Impl(IEventListener listener)
	{
		if (this.listeners.Contains(listener))
		{
			return;
		}

		this.listeners.Add(listener);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void DetachListener_Impl(IEventListener listener)
	{
		if (!this.listeners.Contains(listener))
		{
			return;
		}

		this.listeners.Remove(listener);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Raise_Impl(string arg)
	{
		this.Raise_Impl(this, arg);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Raise_Impl(UnityEngine.Object sender, String arg)
	{
		Debug.Log($@"Event raised: {this.name}, Arg: {arg}");
		this.listeners.ForEach(x => x.EmitEvent(sender, arg));
	}

	protected readonly List<IEventListener> listeners = new();
}