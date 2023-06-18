using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;

    void Awake()
    {
        BulletLifeTime();
    }
    public void BulletLifeTime()
    {
        StartCoroutine("BulletDeathTimer");
    }

    IEnumerator BulletDeathTimer()//In case of bullet never colliding into object we want to destroy it
    {
        yield return new WaitForSeconds(bulletLifeTime);
        //yield return new WaitForEndOfFrame(); //this might be necessary if the bullet life ends at the same time as a collision occurs
        if (gameObject == isActiveAndEnabled)
        {
            this.gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; //This resets the movement of the bullet
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6 && other.gameObject.TryGetComponent(out IHealth hit))
        {
            hit.TakeDamage(bulletDamage);
        }
        if (this.gameObject == isActiveAndEnabled)
        {
            this.gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
