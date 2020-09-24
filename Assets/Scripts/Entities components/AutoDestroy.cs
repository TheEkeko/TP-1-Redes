using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        if (time == 0)
            Invoke("AD", 3);
        else
            Invoke("AD", time);
    }

    void AD()
    {
        Destroy(this.gameObject);
    }
}
