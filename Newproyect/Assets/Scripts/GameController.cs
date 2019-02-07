using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle, Playing, Ended, Ready};
public class GameController : MonoBehaviour

{
    [Range (0f, 0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;

    
    public GameState gameState = GameState.Idle;
    
    public GameObject player;
    public GameObject enemyGenerator;

    public float scaleTime = 6f;
    public float scaleInc = .25f; 

    private AudioSource musicPlayer;




    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update() { 
        bool userAction = Input.GetKeyDown("up")  || Input.GetMouseButtonDown(0);
        //Empieza el juego

        if (gameState == GameState.Idle && (userAction)){
            gameState = GameState.Playing;
                uiIdle.SetActive(false);
                player.SendMessage("UpdateState","PlayerRun");
                enemyGenerator.SendMessage("StartGenerator");
                musicPlayer.Play();
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
    void Parallax() {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed, 0f, 1f, 1f);
    }
    public void RestartGame(){
        SceneManager.LoadScene("Principal");
        ResetTimeScale();
    }
    public void GameTimeScale(){
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incremetado: " + Time.timeScale.ToString());
    }
    public void ResetTimeScale(float newTimeScale = 1f){
        CancelInvoke("GameTimeScale");
        Time.timeScale = newTimeScale;
        Debug.Log("Ritmo de juego restablecido");
    }
}
