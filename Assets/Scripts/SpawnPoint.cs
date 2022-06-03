using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public void Spawn()
	{
		this.Spawn_Impl();
	}

	public void Spawn(EventParameter parameter)
	{
		this.Spawn_Impl(parameter);
	}

	[PunRPC]
	public void SpawnNetwork(Int32 id, Player player)
	{
		var instance = Instantiate(this.pfb_Spawnable, this.transform.position, this.transform.rotation);
		var instances = instance.GetComponents<MonoBehaviour>();

		var spawnableCount = 0;
		foreach (var ins in instances)
		{
			if (ins is ISpawnable spawnable)
			{
				spawnable.SetID(this.id);
				spawnableCount += 1;
			}
		}

		var views = instance.GetComponentsInChildren<PhotonView>();
		views[0].ViewID = id;

		if (spawnableCount < 1)
		{
			Destroy(instance.gameObject);
			return;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Spawn_Impl()
	{
		var instance = PhotonNetwork.Instantiate(this.pfb_Spawnable.name, this.transform.position, Quaternion.identity, 0);
		// var instance = Instantiate(this.pfb_Spawnable, this.transform.position, this.transform.rotation);
		var instances = instance.GetComponents<MonoBehaviour>();

		var spawnableCount = 0;
		foreach (var ins in instances)
		{
			if (ins is ISpawnable spawnable)
			{
				spawnable.SetID(this.id);
				spawnableCount += 1;
			}
		}

		if (spawnableCount < 1)
		{
			Destroy(instance.gameObject);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Spawn_Impl(EventParameter parameter)
	{
		if (String.CompareOrdinal(parameter.Argument, this.id) != 0)
		{
			return;
		}

		this.Spawn();
	}

#pragma warning disable IDE0051
	private void Start()
	{
		if (this.spawnOnStart)
		{
			this.Spawn();
		}
	}
#pragma warning restore IDE0051

	[SerializeField]
	protected PhotonView photonView;

	[SerializeField]
	protected Boolean spawnOnStart;

	[SerializeField]
	protected GameObject pfb_Spawnable;

	[SerializeField]
	protected String id;
}