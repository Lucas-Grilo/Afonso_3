using UnityEngine;

public class MoveEnemy_2 : MonoBehaviour
{
    public float velocidade = 2f; // Velocidade do movimento
    public float distanciaLimite = 3f; // Distância que o inimigo se moverá
    private Vector3 pontoInicial;
    private Vector3 pontoFinal;
    private bool indoParaCima = false;


    void Start()
{
    pontoInicial = transform.position;
    pontoFinal = pontoInicial + new Vector3(0, distanciaLimite, 0); // Mova para cima
}

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        // Mova o inimigo entre os pontos inicial e final
        if (indoParaCima)
        {
            transform.position = Vector3.MoveTowards(transform.position, pontoFinal, velocidade * Time.deltaTime);
            if (transform.position == pontoFinal)
            {
                indoParaCima = false; // Mude a direção
               transform.localScale = Vector3.one;
               

            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pontoInicial, velocidade * Time.deltaTime);
            if (transform.position == pontoInicial)

            {
                indoParaCima = true; // Mude a direção
                
                 //transform.localScale = new Vector3(-1, 1, 1);


            }
        }
    }

}