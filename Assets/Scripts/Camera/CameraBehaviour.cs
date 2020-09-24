using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Vector3 normalPosition;
    [SerializeField] bool follow = false;
    [SerializeField] Transform playerToFollow;
    [SerializeField] Vector3 followPos;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            followPos = playerToFollow.position;
            followPos.y = 20;
            followPos.z -= 5;
            transform.position = followPos;
        }
    }
    public void GetPlayer(GameObject p)
    {
        playerToFollow = p.transform;
        follow = true;
    }

    public void StopFollowing()
    {
        follow = false;
        transform.position = normalPosition;
    }
}
