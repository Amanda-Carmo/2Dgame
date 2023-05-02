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

    // Lista com as duas posições possíveis de entrada do mapa com tamanho 2
    [Header("Map Positions")]
    [SerializeField] public float[] mapX = new float[2];
    [SerializeField] public float[] mapY = new float[2];

    // Criar vetor para armazenar a posição de entrada do mapa
    private Vector2 position;


    // Start is called before the first frame update
    void Start()
    {
        // Pegar posição do player
        this.player = GameObject.FindWithTag("Player").transform;


        // Checar se  a tag do objeto é "Cerca" e sortear a posição de entrada do mapa
        if(gameObject.tag == "Cerca")
        {                    
            // Sortear entre 0 e 1 para escolher a posição de entrada do mapa
            int random = Random.Range(0, 2);
            Debug.Log(random);

            position = new Vector2(mapX[random], mapY[random]);
        }

        else{
            position = new Vector2(playerX, playerY);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){

        // Checar se o player está colidindo com a Cerca
        if(collision.gameObject.CompareTag("Player")){
            // Mudar posição do player para a posição de entrada do outro mapa
            this.player.position = position;
        }
        
        // // Mudar posição do player para a posição de entrada do outro mapa
        // this.player.position = position;
    }
}
 