using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    GameObject player;
    GameObject mainCamera;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -15);
    }
}
