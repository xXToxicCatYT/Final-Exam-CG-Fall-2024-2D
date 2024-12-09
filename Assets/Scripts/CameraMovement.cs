using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float horizontalMoveRange;

    MarioController mario;

    // Start is called before the first frame update
    void Start()
    {
        mario = player.GetComponent<MarioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - player.transform.position.x > horizontalMoveRange && transform.position.x > -3)
        {
            transform.position = new Vector3(player.transform.position.x + horizontalMoveRange, transform.position.y, transform.position.z);
        }
        if (player.transform.position.x - transform.position.x > horizontalMoveRange)
        {
            transform.position = new Vector3(player.transform.position.x - horizontalMoveRange, transform.position.y, transform.position.z);
        }
    }
}
