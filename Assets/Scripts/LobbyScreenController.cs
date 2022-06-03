using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreenController : MonoBehaviourPunCallbacks
{
	public void Cancel()
	{
		PhotonNetwork.LeaveRoom();
		// if (PhotonNetwork.InRoom)
		// {
		// 	return;
		// }

		// PhotonNetwork.room
	}

	public void StartMatch()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
		}

		PhotonNetwork.LoadLevel("game");
	}

	public void OnMatchmakingStart()
	{
		var options = new RoomOptions
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = this.requiredPlayerCount
		};

		PhotonNetwork.JoinRandomOrCreateRoom(null, requiredPlayerCount, MatchmakingMode.FillRoom, null, null, null, options);
	}

	// public void OnConnectionCheckStart()
	// {
	// 	this.OnConnectionCheckStart_Impl();
	// }

	// public void OnConnectionCheckFinished()
	// {
	// 	this.OnConnectionCheckFinished_Impl();
	// }

	// public void OnConnectionCheckFinishedSuccess()
	// {
	// 	this.OnConnectionCheckFinishedSuccess_Impl();
	// }

	// public void OnConnectionCheckFinishedFail()
	// {
	// 	this.OnConnectionCheckFinishedFail_Impl();
	// }

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == this.requiredPlayerCount)
		{
			this.StartMatch();
		}
		this.statusText.text = "Waiting for opponent...";
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == this.requiredPlayerCount && PhotonNetwork.IsMasterClient)
		{
			this.StartMatch();
		}
	}

	public override void OnLeftRoom()
	{
	}

	public override void OnCreatedRoom()
	{
		this.statusText.text = "Waiting for opponent...";
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// private void OnConnectionCheckStart_Impl()
	// {
	// 	this.statusText.text = "Checking connection...";
	// 	this.usernameInput.interactable = false;
	// 	this.playButton.interactable = false;
	// }

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// private void OnConnectionCheckFinished_Impl()
	// {
	// 	this.statusText.text = "";
	// 	this.usernameInput.interactable = true;
	// 	this.playButton.interactable = true;
	// }

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// private void OnConnectionCheckFinishedSuccess_Impl()
	// {
	// 	this.OnConnectionCheckFinished();

	// 	this.statusText.text = "Connected to server.";
	// }

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// private void OnConnectionCheckFinishedFail_Impl()
	// {
	// 	this.OnConnectionCheckFinished();

	// 	this.statusText.text = "Failed to connect to server.";
	// }

	[SerializeField]
	protected TMP_Text statusText;

	[SerializeField]
	protected Button cancelButton;

	[SerializeField]
	protected Byte requiredPlayerCount;
}