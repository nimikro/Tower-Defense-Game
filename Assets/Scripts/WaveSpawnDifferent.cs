using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveSpawnDifferent : MonoBehaviour
{
    // static variables do not need a reference to the instance of the class
    public static int EnemiesAlive;

    public WaveDifferent wave;
    public WaveDifferent.WaveGroup[] groups;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public Text waveCountdownText;

    public GameState gameState;

    private int waveIndex = 0;

    private void Start()
    {
        EnemiesAlive = 0;
    }

    void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == wave.count && PlayerStats.Lives != 0)
        {
            gameState.WinLevel();
            this.enabled = false;
        }

        // Spawn a wave after after countdown reaches 0
        // Because SpawnWave is a Co-routine we need to call it using StartCoroutine
        // A co-routine uses the IEnumerator of System.Collections
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        // make sure countdown does not go in the negatives
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    // Spawn enemies equal to the current wave number
    // IEnumerator allows for yield return, which waits a certain number of seconds before continuing the for loop
    // so that the enemies don't spawn on top of each other
    IEnumerator SpawnWave()
    {
        // SPAWN WAVE.COUNT OF GROUP.COUNT OF THE SAME ENEMIES EACH TIME
        for (int j = 0; j < wave.count; j++)
        {
            foreach (WaveDifferent.WaveGroup group in groups)
            {
                PopulateGroups(group);
                for (int i = 0; i < group.count; i++)
                {
                    EnemiesAlive = group.count;
                    SpawnEnemy(group.enemyPrefab);
                    yield return new WaitForSeconds(1f / group.rate);
                }
            }
        }

        PlayerStats.Rounds++;
        waveIndex++;

    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    public void PopulateGroups(WaveDifferent.WaveGroup group)
    {
        group.count = Random.Range(1, 5);
        group.rate = Random.Range(1, 5);
    }
}
