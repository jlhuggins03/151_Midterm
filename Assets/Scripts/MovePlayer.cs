using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class MovePlayer : MonoBehaviour {

	public float speed;
	public GameObject monster;
	private Transform monsterTrans;
	private Transform playerTrans;
	public float distance;


	private Rigidbody rb;

	//************* Need to setup this server dictionary...
	Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
	//*************

	// Use this for initialization
	void Start () 
	{
		Application.runInBackground = true; //allows unity to update when not in focus

		//************* Instantiate the OSC Handler...
	    OSCHandler.Instance.Init ();
		OSCHandler.Instance.SendMessageToClient ("pd", "/unity/start", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/playmusic", 1);
		//*************

		playerTrans = GetComponent<Transform>();
		monsterTrans = monster.GetComponent<Transform>();
        rb = GetComponent<Rigidbody> ();
	}
	

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		distance = Vector3.Distance(playerTrans.position, monsterTrans.position);

        //Debug.Log(rb.position.x);

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

		rb.AddForce (movement*speed);

		//************* Routine for receiving the OSC...
		OSCHandler.Instance.UpdateLogs();
		Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
		servers = OSCHandler.Instance.Servers;

		foreach (KeyValuePair<string, ServerLog> item in servers) {
			// If we have received at least one packet,
			// show the last received from the log in the Debug console
			if (item.Value.log.Count > 0) {
				int lastPacketIndex = item.Value.packets.Count - 1;
			}
		}
		//*************


		OSCHandler.Instance.SendMessageToClient("pd", "/unity/distance", (1 - distance / 41f));
	}
}
