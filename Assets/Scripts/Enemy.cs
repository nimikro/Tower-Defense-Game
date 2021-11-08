using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    // hide in inspector makes a variable not show up in the inspector, but can still be serialized and used by other objects
    [HideInInspector]
    public float speed;

    public float startHealth = 100f;
    private float health;

    public int worth = 50;

    private bool isDead = false;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    // reduce enemy health using Bullet.cs
    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health/startHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    // slow percentage
    // we cant use speed = speed * (1f - pct) because that would continuously reduce the speed by the (1-pct) every frame
    // so we need to use the starting speed, that remains constant, to update the new speed
    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    // destroy enemy
    void Die()
    {
        isDead = true;
        PlayerStats.Money += worth;
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        WaveSpawnDifferentNew.EnemiesAlive--;
        Destroy(gameObject);
        
    }

}
