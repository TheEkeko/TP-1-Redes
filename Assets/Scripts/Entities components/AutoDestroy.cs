using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AD", 3);
    }

    void AD()
    {
        Destroy(this.gameObject);
    }
}
