using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // static so that it doesn't require a reference to any particular object in the scene
    // so you can call static variables from whichever class you want -> PlayerStats.Rounds for example
    // as opposed to startMoney that we want to change using the editor
    // STATIC VARIABLES WILL CARRY BETWEEN SCENES <----- REMEMBER THAT
    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 20;

    public static int Rounds;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;
        Rounds = 0;
    }

}
