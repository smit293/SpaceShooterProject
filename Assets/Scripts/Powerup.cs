using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 4;

    [SerializeField]
    private int powerupID;
    private GameObject _player;
    private Vector3 _playerPos;


    void OnEnable()
    {
        _player = GameObject.Find("Player");
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            BeingPulled();
        }
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        Die();
    }

    private void BeingPulled()
    {
        _playerPos = _player.transform.position;
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _playerPos, step);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {

            Destroy(this.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {


                switch (powerupID)
                {
                    case 0:

                        player.TripleShotActive();
                        break;

                    case 1:

                        player.SpeedPowerupActive();
                        break;

                    case 2:
                        player.ShieldIsActive();
                        break;
                    case 3:
                        player.AmmoReload();
                        break;

                    case 4:
                        player.HazardPickup();
                        break;

                    case 5:
                        player.LifeRefill();
                        break;

                    case 6:
                        player.MissilePower();
                        break;

                    default:
                        Debug.Log("Default value");
                        break;
                }

            }

            Destroy(this.gameObject);

        }

    }


    private void Die()
    {
        if (transform.position.y <= -5.75f)
        {
            Destroy(this.gameObject);
        }

    }

}
