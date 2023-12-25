using System.Collections;
using UnityEngine;

public class mapsCamera : MonoBehaviour
{
    public GameObject[] players;

    // Update is called once per frame
    [System.Obsolete]
    void LateUpdate()
    {
        if (players[0].active)
        {
            Vector3 mapPosition = players[0].transform.position;
            mapPosition.y = transform.position.y;
            transform.position = mapPosition;

            transform.rotation = Quaternion.Euler(90f, players[0].transform.eulerAngles.y, 0f);
        }else if (players[1].active){
            Vector3 mapPosition = players[1].transform.position;
            mapPosition.y = transform.position.y;
            transform.position = mapPosition;

            transform.rotation = Quaternion.Euler(90f, players[1].transform.eulerAngles.y, 0f);
        }

    }
}
