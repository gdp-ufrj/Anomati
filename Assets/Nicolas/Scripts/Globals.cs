// AQUI FICARÃO AS CONFIGURAÇÕES GLOBAIS DO JOGO, DISPONÍVEIS EM QUALQUER CENA, E QUE PODERÃO SER SALVAS COM UM SISTEMA DE SAVE

using UnityEngine;

public class Globals : MonoBehaviour
{
    public static bool firstScene = true;   // Variável para verificar se é a primeira cena que está sendo carregada (útil para o fade da transição)
    public static string currentScene = "", sceneToBeLoaded = "";  //Variáveis para armazenar a cena atual e a próxima cena a ser carregada
    public static Vector2 lastPlayerPosition; //Variável para armazenar a última posição do jogador
    public static string lastCameraBounds = ""; //Variável para armazenar o nome dos limites da câmera

    public enum MapNames    //Enum para os nomes dos mapas
    {
        Atelie2000, CasaPai2000, Montanha2000, CasaHugo2000, Centro2000, Atelie2025, CasaPai2025, Montanha2025, CasaHugo2025, Centro2025
    }


    //Variáveis triggers:
    public static bool triggerDadRun = true, endDadRun = false, vestiuRoupaAvo = false;  //Trigger para iniciar e finalizar a perseguição com o pai
    public static bool finishAto1 = false, finishAto2 = false, finishAto3 = false, finishDialogoElizaAteliePresent = false, dialogoCasaPai2025 = false, dialogoPai2025 = false;


    public static void ResetGlobalVariables()
    {
        lastPlayerPosition = Vector2.zero;  //Reseta a variável para armazenar a última posição do jogador
        lastCameraBounds = "";  //Reseta a variável para armazenar o nome dos limites da câmera
        endDadRun = false;  //Reseta o trigger de finalização da perseguição
        finishAto1 = false;  //Reseta o trigger de finalização do ato 1
        finishAto2 = false;  //Reseta o trigger de finalização do ato 2
        finishAto3 = false;  //Reseta o trigger de finalização do ato 3
        finishDialogoElizaAteliePresent = false;  //Reseta o trigger de finalização do diálogo com Eliza no Ateliê no presente
        dialogoCasaPai2025 = false;  //Reseta o trigger do diálogo na casa do pai em 2025
        dialogoPai2025 = false;
        vestiuRoupaAvo = false;  //Reseta a variável que indica se o jogador vestiu a roupa do avô
    }


    public static string GetSceneName(MapNames map)   //Método de extensão para MapNames que retorna o nome da cena correspondente
    {
        switch (map)
        {
            case MapNames.Atelie2000: return "Atelie2000";
            case MapNames.CasaPai2000: return "CasaPai2000";
            case MapNames.Montanha2000: return "Montanha2000";
            case MapNames.CasaHugo2000: return "CasaHugo2000";
            case MapNames.Centro2000: return "Centro2000";
            case MapNames.Atelie2025: return "Atelie2025";
            case MapNames.CasaPai2025: return "CasaPai2025";
            case MapNames.Montanha2025: return "Montanha2025";
            case MapNames.CasaHugo2025: return "CasaHugo2025";
            case MapNames.Centro2025: return "Centro2025";
            default: return "";
        }
    }

}