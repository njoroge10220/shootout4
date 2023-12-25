using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class grenade : MonoBehaviour
{
    public ParticleSystem smoke, explosion;
    public AudioSource firingSound, explosionSound;

    private Camera cam;
    private GameObject enemy;

    public bool explode;

    private int explosionRadius;

    // Start is called before the first frame update
    void Start()
    {
        explosionRadius = 7;
        explode = false;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //getting the enemies for killing
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, transform.position);

        if(distance <= explosionRadius && explode == true){

            enemy.GetComponent<FollowPlayer>().Damage();

            if(enemy.GetComponent<FollowPlayer>().enemyHealthBar.value <= 0){
                enemy.GetComponent<Animator>().SetBool("die", true);

                enemy.GetComponent<spawner>().enemyCounter();

                enemy.GetComponent<spawner>().enemy = enemy;

                enemy.GetComponent<spawner>().StartCoroutine(enemy.GetComponent<spawner>().spawnEnemy(enemy));
            }else { return; }
        }explode = false;
    }

    public void fireGrenade(GameObject grenading)
    {
        grenading = Instantiate(this.gameObject, gameObject.transform.position, Quaternion.identity);
        firingSound.Play();
        grenading.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 10f, ForceMode.Impulse);
        smoke.Play();

        explode = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (explode == true)
        {
            explosionSound.Play();

            explosion.Play();
            explosion.Play();
            explosion.Play();
            explosion.Play(); 

            if (collision.gameObject.tag != "ground") { Destroy(collision.gameObject); } else { return; }

            explosion.Play();

            Destroy(gameObject);
        }
    }
}
