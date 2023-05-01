using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMap : MonoBehaviour
{
    public Transform player;

    [Header("Player Position")]    
    [SerializeField] public float playerX;
    [SerializeField] public float playerY;


    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        
        // Mudar posição do player para a posição de entrada do outro mapa
        this.player.position = new Vector2(playerX, playerY);
    }
}
 