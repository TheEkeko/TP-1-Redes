using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    [SerializeField] int playerClass;

    static ClassManager instance;

    public int PlayerClass { get => playerClass; set => playerClass = value; }


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public static ClassManager Instance
    {
        get
        {
            /*if (ClassManager.instance == null)
            {
                ClassManager.instance = new ClassManager();
            }*/
            return ClassManager.instance;
        }
    }
}