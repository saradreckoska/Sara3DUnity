using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Keeps track of the player */

[DefaultExecutionOrder(-100)]
public class PlayerManager : MonoBehaviour {

	#region Singleton

	public static PlayerManager instance;

	void Awake ()
	{
		instance = this;

		// Auto-find player by tag if not assigned in Inspector
		if (player == null)
		{
			player = GameObject.FindWithTag("Player");
			if (player != null)
				Debug.Log("PlayerManager: auto-found player via tag 'Player' -> " + player.name);
			else
				Debug.LogWarning("PlayerManager: 'player' not assigned and no GameObject with tag 'Player' found. Assign the player in the Inspector.");
		}
	}

	#endregion

	public GameObject player;

	public void KillPlayer ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}