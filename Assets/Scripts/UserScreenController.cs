using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserScreenController : MonoBehaviourPunCallbacks
{
	public void Connect()
	{
		PhotonNetwork.NickName = this.usernameInput.text;
		PhotonNetwork.ConnectUsingSettings();

		this.statusText.text = "Connecting...";
		this.usernameInput.interactable = false;
		this.playButton.interactable = false;
	}

	public void OnConnectionCheckStart()
	{
		this.OnConnectionCheckStart_Impl();
	}

	public void OnConnectionCheckFinished()
	{
		this.OnConnectionCheckFinished_Impl();
	}

	public void OnConnectionCheckFinishedSuccess()
	{
		this.OnConnectionCheckFinishedSuccess_Impl();
	}

	public void OnConnectionCheckFinishedFail()
	{
		this.OnConnectionCheckFinishedFail_Impl();
	}

	public override void OnConnectedToMaster()
	{
		this.eventEmitter.EmitEvent("onConnectSuccess", this, "");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		this.statusText.text = $@"Disconnected: {cause}";
		this.usernameInput.interactable = true;
		this.playButton.interactable = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnConnectionCheckStart_Impl()
	{
		this.statusText.text = "Checking connection...";
		this.usernameInput.interactable = false;
		this.playButton.interactable = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnConnectionCheckFinished_Impl()
	{
		this.statusText.text = "";
		this.usernameInput.interactable = true;
		this.playButton.interactable = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnConnectionCheckFinishedSuccess_Impl()
	{
		this.OnConnectionCheckFinished();

		this.statusText.text = "Connected to server.";
		PhotonNetwork.NickName = this.usernameInput.text;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void OnConnectionCheckFinishedFail_Impl()
	{
		this.OnConnectionCheckFinished();

		this.statusText.text = "Failed to connect to server.";
	}

	[SerializeField]
	protected EventEmitter eventEmitter;

	[SerializeField]
	protected TMP_Text statusText;

	[SerializeField]
	protected TMP_InputField usernameInput;

	[SerializeField]
	protected Button playButton;
}