using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private readonly float _speed = 10;
    private bool _isEnemyLaser = false;
    private bool _reverseFiring;
    [SerializeField]
    private GameObject _bossHit;





    void Update()
    {
        if (_isEnemyLaser == false || _reverseFiring == true)
        {
            MoveUp();
        }
        else if (_reverseFiring == false)
        {
            MoveDown();
        }


    }
    public void EnemyReverseFiring()
    {
        _reverseFiring = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            FindObjectOfType<Player>().GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Boss"))
        {

            Instantiate(_bossHit, transform.position, Quaternion.identity);
            other.GetComponent<Boss>().Damaged();
            Destroy(this.gameObject);
        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    void MoveDown()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveUp()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
