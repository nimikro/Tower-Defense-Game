using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f;
    public int damage = 50;

    public GameObject ImpactEffect;

    // get the target position from turret
    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // get distance from target and set bullet speed
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // if the target is closer than the distance the bullet is going to travel in a frame, we already hit the target
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // else move the bullet/missile (normalize is like Time.deltaTime for distance)
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

        void HitTarget()
        {
            GameObject effectIns = (GameObject)Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 5f);

            if(explosionRadius > 0f)
            {
                Explode();

            }
            else
            {
                Damage(target);        
            }
            // destroy bullet
            Destroy(gameObject);
        }

    // function the causes peripheral damage
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }
    
    // single enemy damage
    void Damage(Transform enemy)
    {
        // deal damage to enemy equal to damage
        Enemy newenemy = enemy.GetComponent<Enemy>();
        // if object is actually of enemy tag -> deal damage
        if (newenemy != null)
        {
            newenemy.TakeDamage(damage);
        }
    }

    // see explosion radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
