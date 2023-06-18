using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rotate to face agent when close enough and periodically shoot bullets
// https://forum.unity.com/threads/trying-to-make-my-turret-target-the-closest-enemy-and-then-rotate-to-face-it-results-in-error.1367223/
// https://www.youtube.com/watch?v=dCtt6ri5lag
public class Turret : MonoBehaviour
{
    public float targetingRange;
    public float fireRate;
    public float shootForce;
    public float aimSpeed;
    private float nextTimeToFire = 0f;
    private Transform target;
    [SerializeField] private Transform bulletShootSpawnLocation;
    [SerializeField] private Transform turretRotationObject;
    private bool targetDetected;
    private SphereCollider targetRadius;

    void Start()
    {
        targetRadius = GetComponent<SphereCollider>();
    }

    
    void Update()
    {
        targetRadius.radius = targetingRange;
        if (targetDetected)
        {
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time +1 / fireRate;
                ShootProjectile();
            }
        }
    }
    public void ShootProjectile()
    {
        GameObject bullet = BulletPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero; //This resets the movement of the bullet
            bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bullet.transform.position = bulletShootSpawnLocation.position;
            bullet.transform.rotation = bulletShootSpawnLocation.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().AddForce(bulletShootSpawnLocation.up * shootForce, ForceMode.Impulse); //impulse to get all the energy at once
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
    void OnTriggerStay(Collider other) //aims and triggers shooting bool check in update
    {
        target = FindClosestTarget().transform;
        Vector3 targetDirection = target.position - transform.position;
        float singleStep = aimSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(turretRotationObject.transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        turretRotationObject.transform.rotation = Quaternion.LookRotation(newDirection);
        
        if (target != null)
        {
            if (targetDetected == false)
            {
                targetDetected = true;
            }
        }
        else
        {
            if (targetDetected)
            {
                targetDetected = false;
            }
        }
    }
    public GameObject FindClosestTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest;
        closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
