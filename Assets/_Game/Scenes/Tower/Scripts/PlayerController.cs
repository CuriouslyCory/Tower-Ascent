using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Vector2 dragOrigin;

    private Rigidbody2D _rb;
    private Animator animator;

    [SerializeField]
    private GameState gameState;

    public PlayerCharacter playerCharacter;
    
    [SerializeField]
    public Camera mainCam;

    private enum ControllerStates {
        Idle,
        Dragging,
        Battle,
    }
    private ControllerStates controllerState;

    void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
        playerCharacter = gameObject.GetComponent<PlayerCharacter>();
        animator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
        controllerState = ControllerStates.Idle;
    }

    void OnMouseDown() 
    {
        Debug.Log("Mousedown");
        if(playerCharacter.playerState != PlayerCharacter.PlayerStates.Dead && controllerState == ControllerStates.Idle){
            controllerState = ControllerStates.Dragging;
            _rb.isKinematic = true;
            dragOrigin = playerCharacter.transform.position;
        }
    }

    void OnMouseUp() 
    {
        Debug.Log("MouseUp");
        _rb.isKinematic = false;
        
        if(playerCharacter.currentFloor != null && controllerState == ControllerStates.Dragging){
            NonPlayerCharacter enemy = playerCharacter.currentFloor.transform.GetChild(3).GetComponent<NonPlayerCharacter>();
            // snap player to fighting position
            playerCharacter.transform.position = playerCharacter.currentFloor.transform.position + playerCharacter.currentFloor.transform.TransformDirection(new Vector3(6,0));
            
            // snap cam to floor height
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, playerCharacter.transform.position.y, mainCam.transform.position.z);

            if(enemy.healthSystem.health > 0){
                StartBattle();
            }else{
                controllerState = ControllerStates.Idle;
            }
            
        }else if(playerCharacter.currentFloor == null && controllerState == ControllerStates.Dragging) {
            transform.position = dragOrigin;
            controllerState = ControllerStates.Idle;
        }

    }

    private void StartBattle()
    {
        // get enemy
        NonPlayerCharacter enemy = playerCharacter.currentFloor.transform.GetChild(3).GetComponent<NonPlayerCharacter>();

        // set states
        controllerState = ControllerStates.Battle;
        playerCharacter.playerState = PlayerCharacter.PlayerStates.Fighting;

        // start battle
        BattleSystem battleSystem = BattleSystem.Create(playerCharacter, enemy);
        battleSystem.Start();
        battleSystem.OnStateChanged += BattleSystem_OnStateChanged; 

    }

    private void BattleSystem_OnStateChanged(object sender, BattleStateEventArgs e)
    {
        switch(e.value){
            case BattleSystem.BattleStates.Complete:
                controllerState = ControllerStates.Idle;
                playerCharacter.playerState = PlayerCharacter.PlayerStates.Idle;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(controllerState == ControllerStates.Dragging){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }

    }

}