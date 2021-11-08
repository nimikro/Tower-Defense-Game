using UnityEngine;

// unity will save and load these values and display them in the inspector using System.Serializable
// THE BELOW CODE IS USED IN WAVESPAWNERDIFFERENT TO CREATE A LIST OF WAVES, EACH ONE WITH A DIFFERENT NUMBER OF ENEMIES AND TYPES
// ----> IT SPAWNS DIFFERENT TYPES OF ENEMIES ALL IN THE SAME WAVE
[System.Serializable]
public class WaveDifferent
{
    [System.Serializable]
    public class WaveGroup
    {
        // this is the inner class. It can only be accessed inside the Wave class (since it is private)
        public GameObject enemyPrefab;
        [HideInInspector]
        public int count;
        [HideInInspector]
        public float rate;
    }
    public int count;
    public float rate;
}
