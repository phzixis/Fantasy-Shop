using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementScript : MonoBehaviour
{

    const float moveSpeed = 10f;
    const float xOffset = 1;
    const float yOffset = -1;
    bool currentlyMoving = false;

    List<string> moveList = new List<string>();
    Vector2 targetTile;
    Vector2 targetLocation;
    string prevAnim = "none";
    bool paused = false;

    PathfindingManager pathfinder;
    SpriteRenderer spriteRenderer;
    Animator anim;
    NPCScript npcScript;
    ManagerManager managerManager;

    void Awake() { 
        managerManager = GameObject.FindObjectOfType<ManagerManager>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pathfinder = managerManager.pathfindingManager;
        npcScript = GetComponent<NPCScript>();
        RoundPosition();
    }

    void Update() {
        if(!paused) {
            spriteRenderer.sortingOrder = -(int)transform.position.y;
            if (currentlyMoving) {
                transform.position = Vector2.MoveTowards(transform.position, targetTile, moveSpeed * Time.deltaTime);
                if(Vector2.Distance(transform.position, targetTile) < 0.01f) {
                    StopWalking();
                }
            }
            if (!currentlyMoving && moveList.Count > 0) {
                currentlyMoving = true;
                
                if (prevAnim != moveList[0]) {
                    anim.SetFloat("offset", anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1);
                    prevAnim = moveList[0];
                }

                switch(moveList[0]) {
                    case "up":
                        MoveUp();
                        break;
                    case "down":
                        MoveDown();
                        break;
                    case "left":
                        MoveLeft();
                        break;
                    case "right":
                        MoveRight();
                        break;
                    default:
                        break;
                }
                moveList.RemoveAt(0);
            }
        }
    }
 
    void RoundPosition() {
        transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y));
    }

    public void PathFindToLocation(Vector3 location) {
        moveList = pathfinder.FindPath(transform.position + new Vector3(xOffset,yOffset), location);
    }

    public void Leave() {
        Vector3 exit1 = new Vector3(7.5f,-21.5f,0);
        Vector3 exit2 = new Vector3(-46f,-21.5f,0);

        Vector3 exitToTake = Random.Range(0,2) == 0 ? exit1 : exit2;
        PathFindToLocation(exitToTake);
    }

    void MoveDown() {
        targetTile = transform.position + Vector3.down;
        anim.SetInteger("anim_state", 1);
    }

    void MoveUp() {
        targetTile = transform.position + Vector3.up;
        anim.SetInteger("anim_state", 2);
    }

    void MoveLeft() {
        targetTile = transform.position + Vector3.left;
        anim.SetInteger("anim_state", 3);
    }

    void MoveRight() {
        targetTile = transform.position + Vector3.right;
        anim.SetInteger("anim_state", 4);
    }

    void StopWalking() {
        if (moveList.Count == 0) {
            npcScript.DoNextIntention();
        }
        RoundPosition();
        currentlyMoving = false;
        anim.SetInteger("anim_state", 0);
    }

    public void Pause() {
        paused = true;
        anim.enabled = false;
        anim.SetInteger("anim_state", 0);
    }
    public void Resume() {
        paused = false;
        anim.enabled = true;
    }
}