using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public string sceneToLoad;
    public string exitName;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        PlayerPrefs.SetString("LastExitName", exitName);
        SceneManager.LoadScene(sceneToLoad);

    }

}
 