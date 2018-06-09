using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garagaraInfo : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        //地面または敵に当たると消える
        if (other.tag == "Road" || other.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}