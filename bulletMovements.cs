using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class bulletMovements : MonoBehaviour
{
    private Camera cam;
    public Camera cam1;

    public Transform bulletSpawner;

    public Button[] fireBtn;

    public firing fire;
    public gunScripts gunScripts;

    public AudioSource hitSound, reloadSound, dyingSound;

    public GameObject[] playerSpawnPoints, bullet;
    private int spawnPoint;
    public Button reloadBtn;

    public GameObject player;
    private float moveSpeed;

    public bool enemiesKilling;

    public Animator animate;
    public Slider playerMinSlider, playerMaxSlider;
    public TextMeshProUGUI healthLeft, bulletsLeft, bulletsPresent, enemiesKilled;

    private int playerHealth;
    public int bulletsFired, healthPresent, enemiesToKill;

    private GameObject ammo;

    public unityDBConnectivity gameDB;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        moveSpeed = 150f;

        bulletsFired = gunScripts.bulletsIn;

        playerHealth = 100;
        playerMaxSlider.value = playerHealth;
        playerMinSlider.value = playerHealth;
        gameDB.kills = 0;
        enemies();

        healthPlayer();

        gameDB.healthofPlayer = healthPresent;

        animate = gameObject.GetComponent<Animator>();
        gunScripts = gameObject.GetComponent<gunScripts>();

        animate.SetBool("reLoad", false);

        cam = cam1;

        bulletsLeft.text = bulletsFired.ToString();
        healthLeft.text = healthPresent.ToString();
        enemiesKilled.text = enemiesToKill.ToString();
  
        ammo = GameObject.FindGameObjectWithTag("ammo");

        reloadBtn.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        if (gunScripts.weaponOn == 1 && bulletsFired == 30) return;
        else if (gunScripts.weaponOn == 0 && bulletsFired == 15) return;
        else if (gunScripts.weaponOn == 3 && bulletsFired == 50) return;
        else if (gunScripts.weaponOn == 2) return;
        else reLoading();
    }
     public void enemies()
    {
        //level with enemies To Kill
        if (gameDB.level == 0)
            enemiesToKill = 40;
        else if (gameDB.level == 1)
            enemiesToKill = 50;
        else if (gameDB.level == 2)
            enemiesToKill = 60;
        else if (gameDB.level == 3)
            enemiesToKill = 67;
        else if (gameDB.level == 4)
            enemiesToKill = 75;
        else if (gameDB.level == 5)
            enemiesToKill = 83;
        else if (gameDB.level == 6)
            enemiesToKill = 90;
        else if (gameDB.level == 7)
            enemiesToKill = 100;
        else if (gameDB.level == 8)
            enemiesToKill = 125;
        else if (gameDB.level == 9)
            enemiesToKill = 150;
    }
    void healthPlayer()
    {
        //levels with the helth of the player
        if (gameDB.level == 0)
        { healthPresent = 4; gameDB.energyOfPlayer -= 4; }
        else if (gameDB.level == 1)
        { healthPresent = 4; gameDB.energyOfPlayer -= 4; }
        else if (gameDB.level == 2)
        { healthPresent = 4; gameDB.energyOfPlayer -= 4; }
        else if (gameDB.level == 3)
        { healthPresent = 4;  gameDB.energyOfPlayer -= 4; }
        else { healthPresent = 5; gameDB.energyOfPlayer -= 5; }
    }

    public void ShootBullet(GameObject bulleting)
    {
        if (gunScripts.weaponOn == 1)//gun
        {
            while (fire.isShoot)
            {
                bulleting = Instantiate(bullet[0], bulletSpawner.position, Quaternion.identity);
                bulletsFired -= 1;
                fire.isShoot = false;
            }
            bulleting.GetComponent<Rigidbody>().AddForce(cam.transform.forward * moveSpeed, ForceMode.Impulse);
        }else if (gunScripts.weaponOn == 0)//pistol
        {
            while (fire.isShoot)
            {
                bulleting = Instantiate(bullet[1], bulletSpawner.position, Quaternion.identity);
                bulletsFired -= 1;
                fire.isShoot = false;
            }
            bulleting.GetComponent<Rigidbody>().AddForce(cam.transform.forward * moveSpeed, ForceMode.Impulse);
        }else if (gunScripts.weaponOn == 3)//gun3
        {
            while (fire.isShoot)
            {
                bulleting = Instantiate(bullet[2], bulletSpawner.position, Quaternion.identity);
                bulletsFired -= 1;
                fire.isShoot = false;
            }
            bulleting.GetComponent<Rigidbody>().AddForce(cam.transform.forward * moveSpeed, ForceMode.Impulse);
        }else if(gunScripts.weaponOn == 2){//gernade3
            while (fire.isShoot)
            {
                animate.SetBool("throwG", true);

                bulleting = Instantiate(gunScripts.weapons[2], cam.transform.position, Quaternion.identity);

                gunScripts.weapons[2].GetComponent<grenade>().explode = true;
                gunScripts.weapons[2].GetComponent<grenade>().fireGrenade(gunScripts.weapons[2]);

                gunScripts.weapons[2].GetComponent<grenade>().smoke.Play();

                bulletsFired -= 1;
                fire.isShoot = false;
            }
            animate.SetBool("throwG", false);
            bulleting.GetComponent<Rigidbody>().AddForce(cam.transform.forward * moveSpeed, ForceMode.Impulse);
        }
        else { return; }
    }

    void Update()
    {
        GameEnding();

        gameDB.healthofPlayer = healthPresent;

        enemiesKilled.text = enemiesToKill.ToString();

        if (bulletsFired > 0)
        {
            bulletsLeft.text = bulletsFired.ToString();
            healthLeft.text = healthPresent.ToString();
            bulletsPresent.text = gunScripts.bulletsIn.ToString();
        }else
            reLoading();

        //invoking the spawning of more gold and ammo
        InvokeRepeating("spawnAmmo", 1, Random.Range(3.0f, 6.0f));

        //destroy over stayed spawnedammo
        Destroy(ammo, 150);

        if (gameDB.ammoPresent <= 0)
            gameDB.ammoPresent = 0;
    }
    public void GameEnding()
    {
        if (healthPresent == 0)
        {
            if (playerHealth == 0)
            {
                //GameOver you lost!
                gameDB.youWon = false;
                animate.SetBool("die", true);

                playerMaxSlider.value = playerHealth;
                playerMinSlider.value = playerHealth;

                timeTogameOver();
                SceneManager.LoadScene("gameOverScene");
            }
        }else if (enemiesToKill <= 0)
        {
            //Next Level you won!
            gameDB.youWon = true;

            timeTogameOver();
            SceneManager.LoadScene("gameOverScene");
        }else{ return; }
    }

    IEnumerator timeTogameOver()
    {
        yield return new WaitForSeconds(1.0f);
    }

    //enemy Bullet impact
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemyBullet")
        {
            playerHealth -= 10;
            playerMaxSlider.value = playerHealth;
            playerMinSlider.value = playerHealth;
            hitSound.Play();

            if (playerHealth == 0)
            {
                animate.SetBool("die", true);
                dyingSound.Play();
                StartCoroutine(playerRespawn());
            }
            
        }else if(collision.gameObject.tag == "ammo"){
            gameDB.ammoPresent += 50;
            Destroy(collision.gameObject);
        }else if (collision.gameObject.tag == "cash"){
            gameDB.cashEarned += 50;
            Destroy(collision.gameObject);
        }else { return; }
    }
    //player respawning function
    IEnumerator playerRespawn()
    {
        yield return new WaitForSeconds(2.0f);

        spawnPoint = (spawnPoint + (Random.Range(1, 5))) % playerSpawnPoints.Length;

        gameObject.transform.position = playerSpawnPoints[spawnPoint].transform.position;

        spawnPoint = (spawnPoint + (Random.Range(1, 5))) % playerSpawnPoints.Length;

        healthPresent -= 1;
        animate.SetBool("die", false);

        gameObject.transform.position = playerSpawnPoints[spawnPoint].transform.position;

        bulletsFired = 0;

        playerHealth = 100;
        playerMaxSlider.value = playerHealth;
        playerMinSlider.value = playerHealth;
    }
    //reloading function
    public void reLoading()
    {
        animate.SetBool("reLoad", true);
        reloadSound.Play();

        fireBtn[0].enabled = false;
        fireBtn[1].enabled = false;

        StartCoroutine(reloader());
    }
    IEnumerator reloader()
    {
        yield return new WaitForSeconds(2.5f);
        animate.SetBool("reLoad", false);

        fireBtn[0].enabled = true;
        fireBtn[1].enabled = true;

        bulletsFired = gunScripts.bulletsIn;   
    }
    //spawning gold and ammo
    IEnumerator spawnAmmo()
    {
        yield return new WaitForSeconds(3.0f);

        spawnPoint = (spawnPoint + (Random.Range(1, 5))) % playerSpawnPoints.Length;

        Instantiate(ammo, playerSpawnPoints[spawnPoint].transform.position, Quaternion.identity);
    }
}
