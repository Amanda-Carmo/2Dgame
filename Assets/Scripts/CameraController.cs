using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float offset;
    public float offsetSmoothing;
    private Vector3 playerPosition;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    // Verifica se a posição do jogador está dentro dos limites permitidos do mapa.
    // Se não estiver nos limites de x, para de seguir em X, mas continua em Y e vice-versa

    if (player.transform.position.x > minX && player.transform.position.x < maxX && player.transform.position.y > minY && player.transform.position.y < maxY)
    {
        // Atualiza a posição da câmera suavemente.
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }

    // Limita a posição da câmera apenas no eixo X.
    else if (player.transform.position.x > minX && player.transform.position.x < maxX)
    {
        float clampedY = Mathf.Clamp(player.transform.position.y, minY, maxY);
        playerPosition = new Vector3(player.transform.position.x, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
    
    // Limita a posição da câmera apenas no eixo Y.
    else
    {
        float clampedX = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(player.transform.position.y, minY, maxY);
        playerPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }


    }
}
