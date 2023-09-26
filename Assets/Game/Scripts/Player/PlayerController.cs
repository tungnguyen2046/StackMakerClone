using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get => canMove; }

    private Vector3 targetPos;
    private float speed = 15f;
    private bool canMove;
    private Transform thisTf;

    #region Singleton
    public static PlayerController Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion

    private void Start()
    {
        thisTf = transform;
        targetPos = thisTf.position;
    }

    private void OnEnable() 
    {
        InputHandler.OnSwipe += FindTarget;
    }

    private void OnDisable() 
    {
        InputHandler.OnSwipe -= FindTarget;
    }

    private void Update()
    {
        if (canMove)
        {
            if ((thisTf.position - targetPos).sqrMagnitude > 0.0001f)
            {
                thisTf.position = Vector3.MoveTowards(thisTf.position, targetPos, speed * Time.deltaTime);
            }
            else
            {
                canMove = false;
            }
        }
    }

    private void FindTarget(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                Raycasting(thisTf.position, Vector3.forward);
                break;

            case Direction.Back:
                Raycasting(thisTf.position, Vector3.back);
                break;

            case Direction.Left:
                Raycasting(thisTf.position, Vector3.left);
                break;

            case Direction.Right:
                Raycasting(thisTf.position, Vector3.right);
                break;
        }

        canMove = true;
    }

    private void Raycasting(Vector3 startRay, Vector3 dirUnit)
    {
        RaycastHit hit;
        if (Physics.Raycast(startRay, dirUnit, out hit, 1f))
        {
            // Debug.Log("Hit " + hit.transform.name);

            if (hit.transform.CompareTag(Constant.EDIBLE_BLOCK_TAG) || 
                hit.transform.CompareTag(Constant.INEDIBLE_BLOCK_TAG) ||
                hit.transform.CompareTag(Constant.WALKABLE_BLOCK_TAG) ||
                hit.transform.CompareTag(Constant.WIN_POSITION_TAG)
                )
            {
                thisTf.LookAt(thisTf.position + dirUnit);
                targetPos = hit.transform.position;
                startRay += dirUnit;

                Raycasting(startRay, dirUnit);
            }
            else
            {
                // Debug.Log("Hit Unidentify object" + hit.transform.name);
            }
        }
        else
        {
            // Debug.Log("No hit" );
        }
    }
}
