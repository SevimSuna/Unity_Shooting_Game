using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rg;
    public ParticleSystem lose;
    public Transform kursunPos;
    public GameObject kursun;
    public Text result, enemyCount;

    private Touch touch;
    private Quaternion rotationY;

    private float playerSmoothSpeed = 0.1f;
    private float rotationSpeed = 0.1f;
    private float distance;
    private bool canShoot = false;

    void Start()
    {
        result.gameObject.SetActive(false);
        rg = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Yakýn düþmaný bulmak.
        distance = FindEnemy();
        //Vuruþ mesafesi
        if (distance < 10f && (!canShoot))
        {
            StartCoroutine(Shoot());
        }
        //Ekrana dokunarak hareket etme.
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //X ve Z kordinatlarýnda hareket
                Vector3 kuvvet = new Vector3(touch.deltaPosition.x * playerSmoothSpeed, 0f, touch.deltaPosition.y * playerSmoothSpeed);
                rg.AddForce(kuvvet);
                //Belli mesafe üzeri dönüþ hareketi el ile yapýlýyor.
                if (distance >= 10f)
                {  
                    rotationY = Quaternion.Euler(
                        0f,
                        +touch.deltaPosition.x * rotationSpeed,
                        0f);
                    transform.rotation = rotationY * transform.rotation;
                }
            }

        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //Düþman temasý kontrolü
        if(collision.collider.gameObject.tag.Equals("Enemy"))
        {
            result.text = "Kaybettiniz!";
            result.gameObject.SetActive(true);
            Explode();
        }
    }
    IEnumerator Shoot()
    {
        //Vuruþ animasyonu ve kurþun
        GameObject go = Instantiate(kursun, kursunPos.position, kursunPos.rotation) as GameObject;
        canShoot = true;
        go.GetComponent<Rigidbody>().velocity = kursunPos.transform.forward * 20f;
        Destroy(go.gameObject, 1);
        //Patlama oluþturmamak için Frame üzerinden atýþ saðladým.
        yield return new WaitForFixedUpdate();
        canShoot = false;
    }
    float FindEnemy()
    {
        //En yakýn düþman bulunmasý
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closest = null;
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy"); // Bütün düþmanlarýn bulunmasý
        enemyCount.text = "Düþman sayýsý: " + allEnemy.Length;
        foreach(GameObject current in allEnemy)
        {
            float distanceToEnemy = (current.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closest = current;
            }
        }
        Debug.DrawRay(this.transform.position, closest.transform.position,Color.red);
        if(distanceToClosestEnemy < 10f)
        {
            transform.LookAt(closest.transform.position); //Belirli mesafede düþmana dönüüþ otomatik
        }
        //En yakýn düþman
        return distanceToClosestEnemy;
    }
    void Explode()
    {
        //Oyun sonu animasyon
        lose.gameObject.transform.position = transform.position;
        Instantiate(lose);
        lose.Play();
        transform.gameObject.SetActive(false);
    }
}
