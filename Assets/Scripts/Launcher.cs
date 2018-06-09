using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    public GameObject item;

    //力
    private int power = 5;
    //回転
    private int rotation = -40;
    //回転力
    private float torquePower = -2;

    public void Fire(SpriteRenderer sr)
    {
        //左向きの場合
        if (sr.flipX)
        {
            rotation = 40;
            torquePower = 2;
        }
        else
        //右向きの場合
        {
            rotation = -40;
            torquePower = -2;
        }

        Transform newItem = Object.Instantiate(item, transform.position , Quaternion.identity, transform).transform;
        LaunchUtils.LaunchItem(ref newItem, power, transform.up, rotation, torquePower);


    }

    void Update()
    {

    }
}