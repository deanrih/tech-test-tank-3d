using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSpawnHandler : MonoBehaviour
{
	public IEnumerator DelayedSpawnCoroutine(String id, Single delay)
	{
		return DelayedSpawnCoroutine_Impl(id, delay);
	}

	public void OnPlayerDestroyed(EventParameter parameter)
	{
		this.OnPlayerDestroyed_Impl(parameter);
	}

	public void OnPlayerSpawn(EventParameter parameter)
	{
		this.OnPlayerSpawn_Impl(parameter);
	}

	public void RegisterPlayerID(String id)
	{
		this.RegisterPlayerID_Impl(id);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator DelayedSpawnCoroutine_Impl(String id, Single delay)
	{
		yield return new WaitForSeconds(delay);
		this.onPlayerShouldSpawn.Raise(this, id);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnPlayerDestroyed_Impl(EventParameter parameter)
	{
		if (!this.playerIDs.Contains(parameter.Argument))
		{
			return;
		}

		this.StartCoroutine(this.DelayedSpawnCoroutine(parameter.Argument, this.spawnDelay));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnPlayerSpawn_Impl(EventParameter parameter)
	{
		this.RegisterPlayerID(parameter.Argument);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RegisterPlayerID_Impl(String id)
	{
		if (this.playerIDs.Contains(id))
		{
			return;
		}

		this.playerIDs.Add(id);
	}

	protected List<String> playerIDs = new();

	[SerializeField]
	protected EventSource onPlayerShouldSpawn;

	[SerializeField]
	protected Single spawnDelay;
}