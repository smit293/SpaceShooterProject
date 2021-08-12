using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTwo : MonoBehaviour
{
    private Transform _phaseOne;
    private Transform _phaseTwo;
    [SerializeField]
    private AudioClip _laserCharge;
    [SerializeField]
    private AudioClip _laserFiring;
    private AudioSource _audioSource;
    private GameObject _targetPos;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserCharge;
        _audioSource.Play();
        _targetPos = GameObject.FindGameObjectWithTag("TargetPos");
        _phaseOne = gameObject.transform.GetChild(0);
        _phaseTwo = gameObject.transform.GetChild(1);
        _phaseOne.gameObject.SetActive(true);
        _phaseTwo.gameObject.SetActive(false);
        StartCoroutine(PhaseSwitchControl());
    }
    private void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position,_targetPos.transform.position,50 * Time.deltaTime);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<Player>().GetComponent<Player>().Damage();
        }
    }
    IEnumerator PhaseSwitchControl() // For toggle to the laser beam after the charge up is finished
    {
        
        yield return new WaitForSeconds(2.0f);
        _audioSource.clip = _laserFiring;
        _audioSource.Play();
        _phaseOne.gameObject.SetActive(false);
        _phaseTwo.gameObject.SetActive(true);
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(this.gameObject);
    }
   
}
