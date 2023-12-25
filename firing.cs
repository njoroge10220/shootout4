using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class firing : MonoBehaviour
{
    public Camera cam;
    public Button[] fireBtn;
    public bool isShoot;

    public AudioSource shootingSound;
    public ParticleSystem muzzle;

    private Vector3 tarPoint, bulletDir;
    public GameObject shoot;
    private GameObject gernade;

    private GameObject enemyOther;
    public int enemiesToKill;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fireBtn.Length; i++)
        { fireBtn[i].GetComponent<Button>().onClick.AddListener(TaskOnClick); }

        isShoot = false;

        enemiesToKill = shoot.GetComponent<bulletMovements>().enemiesToKill;
    }

    void TaskOnClick() 
    { Shooting(); }
    public void Shooting()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit) ||
            Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(-Vector3.forward), out hit))
        {
            tarPoint = hit.point;

            Vector3 bulletDir = tarPoint - cam.transform.position;
            Vector3 bulletDir1 = bulletDir + new Vector3(1, 1, 0);

            isShoot = true;

            if (gernade == isActiveAndEnabled)
                shoot.GetComponent<bulletMovements>().ShootBullet(shoot.GetComponent<gunScripts>().weapons[2]);
            else{
                shoot.GetComponent<bulletMovements>().ShootBullet(this.gameObject);
                muzzle.Play();
                shootingSound.Play();
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyOther = collision.gameObject;

            enemyOther.GetComponent<FollowPlayer>().Damage();

            if (enemyOther.GetComponent<FollowPlayer>().enemyHealthBar.value <= 0)
            {
                enemyOther.GetComponent<Animator>().SetBool("die", true);

                enemyOther.GetComponent<spawner>().enemyCounter();

                enemyOther.GetComponent<spawner>().enemy = enemyOther;

                enemyOther.GetComponent<spawner>().StartCoroutine(enemyOther.GetComponent<spawner>().spawnEnemy(enemyOther));

                if (shoot.GetComponent<bulletMovements>().playerMinSlider.value != 100 && 
                    shoot.GetComponent<bulletMovements>().playerMaxSlider.value != 100)
                {
                    shoot.GetComponent<bulletMovements>().playerMinSlider.value += 10;
                    shoot.GetComponent<bulletMovements>().playerMaxSlider.value += 10;
                }
            } else { return; }
        }
    }
}
