using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    private Transform _target;
    public Rigidbody2D rb;
    public float angleChangingSpeed;
    public float movementSpeed = 10f;


    private void Start()
    {
        StartCoroutine(TimeToDie());
    }
    private void FixedUpdate()
    {
        if (FindClosestEnemy() != null)
        {
            _target = FindClosestEnemy().transform;
            Vector2 direction = (Vector2)_target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -angleChangingSpeed * rotateAmount;
            rb.velocity = transform.up * movementSpeed;
        }
        if (FindClosestEnemy() == null)
        {
            transform.Translate(movementSpeed * Time.deltaTime * Vector3.up);
        }


    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in enemies)
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
    IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }

}
        


