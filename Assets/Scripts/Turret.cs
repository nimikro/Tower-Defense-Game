using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]

    public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public float slowAmount = 0.5f;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;


    public Transform firePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    //find a target in the scene -> not every frame because it is a slow method
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity; //if there are no enenies -> distance is set to infinity
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            // find closest enemy
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
    
        // if an enemy is found and he is within turret range, get the enemy's postion for the turret's target
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>(); // <-- targetEnemy is used for dealing damage with the Laser
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireCountdown -= Time.deltaTime;

        // if we don't have a target, don't do anything
        if (target == null)
        {
            // if there is no target line renderer must be disabled
            if(useLaser)
            {
                if(lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            
            return;
        }

        // get target direction and rotate towards it
        LockOnTarget();
        // if laser is enabled in editor attack using laser
        if(useLaser)
        {
            Laser();
        }
        // else fire turret bullets every fireCountdown
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
                
    }

    void LockOnTarget()
    {
        // get the direction for the turret and using quaternions get the rotation aspect and translate into euler angles for partToRotate
        // Lerp is used to smooth the rotation of the turret
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime); // <- deal damage with laser
        targetEnemy.Slow(slowAmount); // <- slow down enemy
        
        if(!lineRenderer.enabled)
        {
            // use PLAY and STOP for particle system, enable and disable for line renderer and lights
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        // line rendered works with an array of elements setting the first element at the start of the laser turret and the second at the target
        // -> check editor in position category
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        // make particles of impactEffect appear at the position where the lazer is hitting the target
        // dir.normalized returns a vector of (1,1,1) that gives us the radius of the enemy sphere which is 1
        // using that we offset the particle effect so that it hits the enemy right at the radius of the sphere
        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
                
    }

    void Shoot()
    {
        if (useLaser)
        {
            return;
        }

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
