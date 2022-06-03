using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviourPunCallbacks
{
	public override void OnConnectedToMaster()
	{
		SceneManager.LoadScene("menu.main", LoadSceneMode.Single);
	}

#pragma warning disable IDE0051
	private void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	private void Start()
	{
		SceneManager.LoadScene("menu.main", LoadSceneMode.Single);
		// PhotonNetwork.ConnectUsingSettings();
	}
#pragma warning restore IDE0051
}