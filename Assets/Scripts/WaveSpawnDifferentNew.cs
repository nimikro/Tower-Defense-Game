using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveSpawnDifferentNew : MonoBehaviour
{
    // static variables do not need a reference to the instance of the class
    public static int EnemiesAlive;

    public WaveDifferentNew wave;
    public WaveDifferentNew.WaveGroup[] groups;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public Text waveCountdownText;

    public GameState gameState;

    private int waveIndex = 0;
    private int arrayIndex;

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

        if (waveIndex == wave.waveCount && PlayerStats.Lives != 0)
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
        EnemiesAlive = wave.enemyCount;

        // SPAWN A DIFFERENT ENEMY EACH TIME
        for (int j = 0; j < wave.groupCount; j++)
        {
            for (int i = 0; i < wave.enemyCount; i++)
            {
                arrayIndex = Random.Range(0, groups.Length);
                PopulateGroups(groups[arrayIndex]);
                SpawnEnemy(groups[arrayIndex].enemyPrefab);
                yield return new WaitForSeconds(1f / groups[arrayIndex].rate);
            }
        }
        
        PlayerStats.Rounds++;
        waveIndex++;

    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    public void PopulateGroups(WaveDifferentNew.WaveGroup group)
    {
        group.rate = Random.Range(2, 6);
    }
}
