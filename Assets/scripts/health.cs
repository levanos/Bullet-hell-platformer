using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public Image bar;
    public float fill;
    public charmovement playa;

    private float maxHp;

    void Start()
    {
        maxHp = playa.hp;
        fill = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        fill = playa.hp/maxHp;
        bar.fillAmount = fill;
    }
}
