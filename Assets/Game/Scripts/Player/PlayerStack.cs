using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStack : MonoBehaviour
{
    public Animator anim;
    private string ACTION_ANIM = "Action";

    public GameObject standedBrickPrefabs;

    public Transform brickContainer;
    public Transform playerMesh;

    public bool IsWin { get; set; }

    private Stack<GameObject> brickStack;

    private int animState = 0;

    private float brickHeight = 0.3f;

    #region Instance
    private TreasureManager treasureManager;
    private CanvasManager canvasManager;
    #endregion

    #region Singleton
    public static PlayerStack Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion

    void Start()
    {
        treasureManager = TreasureManager.Ins;
        canvasManager = CanvasManager.Ins;

        brickStack = new Stack<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.EDIBLE_BLOCK_TAG))
        {
            PushToStack();
            SetAnimState(1);
            Invoke(nameof(SetIdleAnim), 0.1f);
        }

        if (other.CompareTag(Constant.INEDIBLE_BLOCK_TAG))
        {
            PopFromStack(other.transform.position + Vector3.down * brickHeight);
        }

        if (other.CompareTag(Constant.WIN_POSITION_TAG))
        {
            SetAnimState(2);
            IsWin = true;
            treasureManager.WaitToOpenTreasure(1f);
            Invoke(nameof(ActiveWinScreen), 2f);
        }
    }

    public void SetAnimState(int state)
    {
        animState = state;
        anim.SetInteger(ACTION_ANIM, animState);
    }

    public void SetIdleAnim()
    {
        SetAnimState(0);
    }


    private void PushToStack()
    {
        GameObject brick = Instantiate(standedBrickPrefabs, transform.position, Quaternion.identity, brickContainer);
        brickStack.Push(brick);

        brick.transform.localPosition = new Vector3(0, (brickStack.Count - 1) * brickHeight, 0);
        playerMesh.localPosition = new Vector3(0, brickStack.Count * brickHeight, 0);
    }

    private void PopFromStack(Vector3 popedPosition)
    {
        if(brickStack.Count == 0)
        {
            canvasManager.LoseScreen();
            Destroy(this.gameObject);
            return;
        }
        
        GameObject brick = brickStack.Pop();
        brick.transform.parent = null;
        brick.transform.position = popedPosition + Vector3.up * 0.1f;

        playerMesh.localPosition = new Vector3(0, brickStack.Count * brickHeight, 0);
    }

    private void ActiveWinScreen()
    {
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            canvasManager.WinScreen();
        }
        else canvasManager.NextScreen();
    }
}
