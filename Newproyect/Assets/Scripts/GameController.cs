using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle, Playing, Ended, Ready};
public class GameController : MonoBehaviour

{
    [Range (0f, 0.20f)]
    //VELOCIDAD DEL EFECTO PARALLAX
    public float parallaxSpeed = 0.02f;
    //VARIABLES DE LA INTERFAZ DE USUARIO
    #region [VARIABLES DE INTERFAZ DE USUARIO]
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public Text pointsText;
    #endregion

    
    public GameState gameState = GameState.Idle;
    public GameObject player;
    public GameObject enemyGenerator;

#region [VARIABLES DE GESTION DE LA DIFCULTAD CON EL TIEMPO]
    //VARIABLES DE AUMENTO DE DIFICULTAD CON EL TIEMPO
    public float scaleTime = 6f;
    public float scaleInc = .25f; 
#endregion
    private AudioSource musicPlayer;

    private int points = 0;



    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update() { 
        //Empieza el juego
        //Esta variable detecta cuando se pulsa el raton o la tecla UP 
        bool userAction = Input.GetKeyDown("up")  || Input.GetMouseButtonDown(0);
        
        //Comprobamos el estado del juego y que se haya pulsado el raton para comenzar
        if (gameState == GameState.Idle && (userAction)){
            //cambiamos el estado del juego a jugando 
            gameState = GameState.Playing;
            //desactivamos la iu del principio
                uiIdle.SetActive(false);
            //Activamos la interfaz de los puntos
                uiScore.SetActive(true);
            //Enviamos un mensaje al controlador del player para que se actualice el estado a corriendo y se active la animacion
                player.SendMessage("UpdateState","PlayerRun");
            //Enviamos un mensage al generador de enemigos para que comience a generarlos
                enemyGenerator.SendMessage("StartGenerator");
            //Enviamos un mensaje al generador del player para que comience la animacion del polvo al correr
                player.SendMessage("DustPlay");
            //Ponemos el play del audio
                musicPlayer.Play();
            //Hacemos una llamada repetida a la funcion para que se aumente progresivamente la dificultad en funcion de las variables creadas
                InvokeRepeating("GameTimeScale",scaleTime,scaleTime);
        }

        
            //Juego en marcha
        else if (gameState == GameState.Playing){
                Parallax();
        }
        //Juego preparado para reiniciar
        else if (gameState == GameState.Ready){
                //TODO
                if(userAction){
                    RestartGame();
                }
        }
        

    }
    //
    //Esta funcion cotrola el efecto Parallax , se mueve el suelo y el fondo para dar la impresion de ovimiento
    void Parallax() {
        //manejamos la velocidad del efecto mediante la variable finalSpeed
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        //manejamos las velocidadestanto del suelo como de el fondo
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed, 0f, 1f, 1f);
    }

    //Reinicia el juego
    public void RestartGame(){
        //Nos lleva a la escena principañ
        SceneManager.LoadScene("Principal");
        //Resetea la dificultad por tiempo
        ResetTimeScale();
    }
    //incrementa la dificultad
    public void GameTimeScale(){
        //aumenta la escala de tiempo del juego para aumentar la velocidad y asi su comlejidad
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incremetado: " + Time.timeScale.ToString());
    }
    //resetea  la escala de tiempo y asi la dificultad
    public void ResetTimeScale(float newTimeScale = 1f){
        //Cancela la invocacion a GameTimeScale
        CancelInvoke("GameTimeScale");
        //Resetea el valor de la escala al recibido en la funcion, 1 por defecto
        Time.timeScale = newTimeScale;
        Debug.Log("Ritmo de juego restablecido");
    }
    //funcion que incrementa los points
    public void IncreasePoints(){
        //suma un point
        points++;
        //lo pinta e el cuadro de texto
        pointsText.text = points.ToString();
    }
}
