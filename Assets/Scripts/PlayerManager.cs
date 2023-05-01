using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerManager found!");
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }  

        DontDestroyOnLoad(gameObject); 
        
    }
}
