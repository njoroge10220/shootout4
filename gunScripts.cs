using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class gunScripts : MonoBehaviour
{
    public GameObject[] weapons = new GameObject[4];

    private Animator anim;
   
    public int weaponOn, switchweapon, bulletsIn;

    public unityDBConnectivity gameDB;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        //choosing between the guns
        if(gameDB.weapon == 0)
        {
            switchweapon = 1;
            weaponOn = 1;
        }else if(gameDB.weapon == 1){
            switchweapon = 3;
            weaponOn = 3;
        }    
    
        anim.SetBool("idle", true);
        anim.SetBool("fire", false);
    }

    // Update is called once per frame
     void Update()
     {//Gun
        if (switchweapon == 1)
        {
            anim.SetBool("pistolling", false);
            anim.SetBool("grenade", false);
         

            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);

            StartCoroutine(timeToChange());

             bulletsIn = 30;
        }//Gun3
        else if(switchweapon == 3){
            anim.SetBool("pistolling", false);
            anim.SetBool("grenade", false);
           

            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            weapons[3].SetActive(true);

            StartCoroutine(timeToChange());

            bulletsIn = 50;
        }//pistol
        else if (switchweapon == 0){
            anim.SetBool("pistolling", true);
            anim.SetBool("grenade", false);
          

            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);

            StartCoroutine(timeToChange());

            bulletsIn = 15;
        }//grenade
        else if (switchweapon == 2){
            anim.SetBool("grenade", true);

            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
            weapons[3].SetActive(false);

            StartCoroutine(timeToChange());

            bulletsIn = 3;
        }else { return; }
     }

    //Grenade function
    public void Grenade()
    {
        gameObject.GetComponent<bulletMovements>().bulletsFired = 3;
        switchweapon = 2;
        weaponOn = 2;
    }
    //Gun function
    public void Gun()
    { //choosing between the guns
        if (gameDB.weapon == 0)
        {
            gameDB.ammoPresent -= 30;

            gameObject.GetComponent<bulletMovements>().bulletsFired = 30;
            switchweapon = 1;
            weaponOn = 1;
        }else if (gameDB.weapon == 1){
            gameDB.ammoPresent -= 50;

            gameObject.GetComponent<bulletMovements>().bulletsFired = 50;
            switchweapon = 3;
            weaponOn = 3;
        }else{return;}
    }

    //pistol function
    public void Pistol()
    {
        gameDB.ammoPresent -= 15;

        gameObject.GetComponent<bulletMovements>().bulletsFired = 15;
        switchweapon = 0;
        weaponOn = 0;
    }

    public IEnumerator timeToChange()
    { yield return new WaitForSeconds(2f); }
}
