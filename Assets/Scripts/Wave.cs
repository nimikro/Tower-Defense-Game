using UnityEngine;

// unity will save and load these values and display them in the inspector using System.Serializable
// THE BELOW CODE IS USED IN WAVESPAWNER TO CREATE A LIST OF WAVES, EACH ONE BEING ABLE TO SPECIFY THE TYPE OF ENEMY TO SPAWN, THE COUNT AND THE RATE
[System.Serializable]
public class Wave
{
    public GameObject enemy;
    public int count;
    public float rate;
}
