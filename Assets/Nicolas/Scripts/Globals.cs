// AQUI FICARÃO AS CONFIGURAÇÕES GLOBAIS DO JOGO, DISPONÍVEIS EM QUALQUER CENA, E QUE PODERÃO SER SALVAS COM UM SISTEMA DE SAVE

using UnityEngine;

public class Globals : MonoBehaviour {
    public static bool firstScene = true;   // Variável para verificar se é a primeira cena que está sendo carregada (útil para o fade da transição)
    public static string currentScene = "Present", lastScene = "Present";
}

