using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct KeyEventPair
{
	[SerializeField]
	public String Key;

	[SerializeField]
	public EventSource EventSource;
}

public class EventEmitter : MonoBehaviour
{
	public void EmitEvent(String key, UnityEngine.Object sender, String arg)
	{
		this.EmitEvent_Impl(key, sender, arg);
	}

	public void SetValues()
	{
		this.SetValues_Impl();
	}

	public void SetRemappedEvents()
	{
		this.SetRemappedEvents_Impl();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EmitEvent_Impl(String key, UnityEngine.Object sender, String arg)
	{
		if (!this.remappedEvents.ContainsKey(key))
		{
			return;
		}

		this.remappedEvents[key].Raise(sender, arg);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetValues_Impl()
	{
		this.SetRemappedEvents();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetRemappedEvents_Impl()
	{
		this.remappedEvents.Clear();

		foreach (var kep in this.events)
		{
			this.remappedEvents.Add(kep.Key, kep.EventSource);
		}
	}

#pragma warning disable IDE0051
	private void Awake()
	{
		this.SetValues();
	}
#pragma warning restore IDE0051

	private Dictionary<String, EventSource> remappedEvents = new();

	[SerializeField]
	protected List<KeyEventPair> events = new();
}