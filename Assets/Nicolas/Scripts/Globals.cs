// AQUI FICARÃO AS CONFIGURAÇÕES GLOBAIS DO JOGO, DISPONÍVEIS EM QUALQUER CENA, E QUE PODERÃO SER SALVAS COM UM SISTEMA DE SAVE

using UnityEngine;

public class Globals : MonoBehaviour
{
    public static bool firstScene = true;   // Variável para verificar se é a primeira cena que está sendo carregada (útil para o fade da transição)
    public static string currentScene = "", sceneToBeLoaded = "";  //Variáveis para armazenar a cena atual e a próxima cena a ser carregada
    public static Vector2 lastPlayerPosition; //Variável para armazenar a última posição do jogador
    public static string lastCameraBounds = ""; //Variável para armazenar o nome dos limites da câmera

    public static void ResetGlobalVariables()
    {
        lastPlayerPosition = Vector2.zero;  //Reseta a variável para armazenar a última posição do jogador
        lastCameraBounds = "";  //Reseta a variável para armazenar o nome dos limites da câmera
    }
}