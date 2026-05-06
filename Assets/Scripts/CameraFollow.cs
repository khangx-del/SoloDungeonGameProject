using UnityEngine;

public class CameraFollow : MonoBehaviour
{
     private Transform player;

void Start()
    {
        FindPlayer();
    }
   void LateUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }
        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            -10f
        );
    }
     void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
        }
    }

}
