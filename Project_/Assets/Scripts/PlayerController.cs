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
        //Yak�n d��man� bulmak.
        distance = FindEnemy();
        //Vuru� mesafesi
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
                //X ve Z kordinatlar�nda hareket
                Vector3 kuvvet = new Vector3(touch.deltaPosition.x * playerSmoothSpeed, 0f, touch.deltaPosition.y * playerSmoothSpeed);
                rg.AddForce(kuvvet);
                //Belli mesafe �zeri d�n�� hareketi el ile yap�l�yor.
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
        //D��man temas� kontrol�
        if(collision.collider.gameObject.tag.Equals("Enemy"))
        {
            result.text = "Kaybettiniz!";
            result.gameObject.SetActive(true);
            Explode();
        }
    }
    IEnumerator Shoot()
    {
        //Vuru� animasyonu ve kur�un
        GameObject go = Instantiate(kursun, kursunPos.position, kursunPos.rotation) as GameObject;
        canShoot = true;
        go.GetComponent<Rigidbody>().velocity = kursunPos.transform.forward * 20f;
        Destroy(go.gameObject, 1);
        //Patlama olu�turmamak i�in Frame �zerinden at�� sa�lad�m.
        yield return new WaitForFixedUpdate();
        canShoot = false;
    }
    float FindEnemy()
    {
        //En yak�n d��man bulunmas�
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closest = null;
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy"); // B�t�n d��manlar�n bulunmas�
        enemyCount.text = "D��man say�s�: " + allEnemy.Length;
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
            transform.LookAt(closest.transform.position); //Belirli mesafede d��mana d�n��� otomatik
        }
        //En yak�n d��man
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
