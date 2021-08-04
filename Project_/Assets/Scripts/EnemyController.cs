using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    public ParticleSystem shoot;

    private int enemyHealth = 3;
    private float distance;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //D��man ile player aras�ndaki mesafe.
        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance < 10f)
        {
            //D���man hareketi
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Kur�un kontrol�
        shoot.gameObject.transform.position = transform.position;
        collision.gameObject.tag.Equals("Bullet");
        if(enemyHealth > 0)
        {
            enemyHealth--;
        }
        else
        {
            Destroy(this.gameObject);
            Instantiate(shoot);
            shoot.Play();
        }
    }
}
