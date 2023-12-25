using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pausePlay : MonoBehaviour
{
    public Button playBtn, restartBtn, homeBtn;

    public unityDBConnectivity gameDB;

    public void playFunction()
    { Time.timeScale = 1; gameObject.SetActive(false); }
    public void restartFunction(string sceneLevel)
    {
        StartCoroutine(timeToRestart());

        if (gameDB.level == 0)
            sceneLevel = "level1";
        else if (gameDB.level == 1)
            sceneLevel = "level2";
        else if (gameDB.level == 2)
            sceneLevel = "level3";
        else if (gameDB.level == 3)
            sceneLevel = "level4";
        else if (gameDB.level == 4)
            sceneLevel = "level5";
        else if (gameDB.level == 5)
            sceneLevel = "level6";
        else if (gameDB.level == 6)
            sceneLevel = "level7";
        else if (gameDB.level == 7)
            sceneLevel = "level8";
        else if (gameDB.level == 8)
            sceneLevel = "level9";
        else
            sceneLevel = "level10";

        SceneManager.LoadScene(sceneLevel);
    }
    public void homeFunction()
    { SceneManager.LoadScene("choiceScene"); }
    IEnumerator timeToRestart()
    { yield return new WaitForSeconds(0.5f); }
}
