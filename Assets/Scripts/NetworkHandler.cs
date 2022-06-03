using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkHandler : MonoBehaviourPunCallbacks
{
	public void CheckConnectionPhoton()
	{
		if (!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.ConnectUsingSettings();
		}
		// if (!PhotonNetwork.IsConnected)
		// {
		// 	this.eventEmitter.EmitEvent("onConnectFail", this, "");
		// 	Debug.Log($@"Not Connected");
		// 	return;
		// }

		// if (!PhotonNetwork.ConnectUsingSettings())
		// {
		// 	this.eventEmitter.EmitEvent("onConnectFail", this, "");
		// 	Debug.Log($@"Not Connected");
		// 	return;
		// }

		// if (!PhotonNetwork.InLobby)
		// {
		// 	PhotonNetwork.JoinLobby();
		// }

		// if (!PhotonNetwork.InLobby)
		// {
		// 	this.eventEmitter.EmitEvent("onConnectFail", this, "");
		// 	Debug.Log($@"Not in Loby");
		// 	return;
		// }

		// this.eventEmitter.EmitEvent("onConnectSuccess", this, "");
	}

	public override void OnConnectedToMaster()
	{
		this.eventEmitter.EmitEvent("onConnectSuccess", this, "");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		this.eventEmitter.EmitEvent("onConnectFail", this, "");
	}
	// public Boolean CheckConnectionPhoton()
	// {
	// 	return PhotonNetwork.IsConnectedAndReady;
	// }

	[SerializeField]
	protected EventEmitter eventEmitter;
}