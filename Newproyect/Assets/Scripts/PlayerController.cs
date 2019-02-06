using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("up")  || Input.GetMouseButtonDown(0)){
            UpdateState("PlayerJump");
        }
    }

    public void UpdateState(string state = null){
        if(state != null){
            animator.Play(state);
        }

    }
      void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Enemy"){
            Debug.Log("Me muero");
        }
    }
}
