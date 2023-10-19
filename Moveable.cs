using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Moveable : MonoBehaviour
{
    public enum MoveState
    {
        Stable,
        PlayerMove,
        NormalMove,
        StackMove,
        GoEnemyMove
    }

    [Header("MOVEABLE")]
    public MoveState _moveState = MoveState.Stable;

    public Transform myGoPos;

    public float startSpeed = 14f;
    public float swipeSpeed = 14f;
    public float rotationSpeed = 750f;
    public static float stackSpeed = 8f;
    [HideInInspector] public float speed = 20f;


    public bool canMove;
    private float ver;
    private float hor;
    public bool manualAnim;
    public float flyingTolerance;

    public float XClampMin;
    public float XClampMax;
    public Camera fixedCamera;
    Vector3 input;
    Vector3 firstTouchPos;
    Vector3 firstPlayerPos;
    bool move;


    Rigidbody rb;
    Quaternion firstLookPos;
    Vector3 middleOffset;

    public bool amIInStack;
    public bool amIStable;
    //public HexagonStackBase myStackManager;


    public abstract void Moveable_Start();
    public abstract void Moveable_Update();
    public abstract void Moveable_FixedUpdate();

    public Rigidbody GetRigidbody() => rb;


    public virtual void OnMoveable_Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = startSpeed;

        if (firstLookPos == null)
            firstLookPos = transform.rotation;
    }


    public virtual void OnMoveable_Update()
    {
        if (_moveState == MoveState.PlayerMove)
            PlayerMoveInput();
    }

    public virtual void OnMoveable_FixedUpdate()
    {
        switch (_moveState)
        {
            case MoveState.Stable:
                break;
            case MoveState.PlayerMove:
                PlayerMove();
                break;
            case MoveState.NormalMove:
                NormalMove();
                break;
            case MoveState.StackMove:
                //StackMove();
                break;
            case MoveState.GoEnemyMove:
                GoingEnemyMove();
                break;
            default:
                break;
        }
    }

    private void PlayerMove()
    {
        if (!canMove)
            return;

        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, speed * 0.8f), Time.fixedDeltaTime * 10f);

        if (move)
        {
            Vector3 movementVector = fixedCamera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, 1)) - firstTouchPos;

            float finalXPos = firstPlayerPos.x - movementVector.x * swipeSpeed;
            //finalXPos = Mathf.Clamp(finalXPos, XClampMin, XClampMax);

            input = new Vector3(finalXPos, 0, 1);


            transform.position = Vector3.Lerp(transform.position, new Vector3(finalXPos, rb.position.y, rb.position.z), Time.fixedDeltaTime * 10f);

        }
    }

    private void PlayerMoveInput()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance._gameState == GameManager.GameState.NotStarted)
                    UIManager.Instance.PlayArea();

                firstTouchPos = fixedCamera.ScreenToWorldPoint(Input.mousePosition - new Vector3(0, 0, 1));
                firstPlayerPos = transform.position;

                move = true;

                MMVibrationManager.Haptic(HapticTypes.Selection);
            }
            else if (Input.GetMouseButton(0))
            {


            }
            else if (Input.GetMouseButtonUp(0))
            {
                move = false;

                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }

        }
    }

    public void ChangeMoveableState(MoveState state)
    {
        _moveState = state;
        switch (_moveState)
        {
            case MoveState.Stable:
                SetCanMove(false);
                break;
            case MoveState.PlayerMove:
                SetCanMove(true);
                break;
            case MoveState.NormalMove:
                SetCanMove(true);
                break;
            case MoveState.StackMove:
                SetCanMove(true);
                break;
            case MoveState.GoEnemyMove:
                SetCanMove(true);
                break;
            default:
                break;
        }
    }

    [Button]
    public void SetCanMove(bool sts)
    {
        canMove = sts;
        move = sts;
        StopVelocity();

    }

    private void StopVelocity()
    {
        hor = 0;
        ver = 0;
        if (!rb) return;
        rb.velocity = Vector3.zero;

    }

    private void NormalMove()
    {
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
        rb.velocity = new Vector3(transform.forward.x, rb.velocity.y, transform.forward.z) * Time.deltaTime * speed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, firstLookPos, rotationSpeed * Time.deltaTime);

    }

    //private void StackMove()
    //{
    //    if (!amIInStack)
    //        return;

    //    //Vector3 dir = (myArmy.transform.position + myArmy._points[myId]) - transform.position;
    //    int id = myStackManager.stack.IndexOf(this);
    //    if (id >= myStackManager._points.Count)
    //        return;
    //    Vector3 destPoint = myStackManager._points[id];
    //    destPoint *= myStackManager.Formation.Spread;
    //    //Vector3 dir = (myArmy.transform.position + destPoint) - transform.position;
    //    Vector3 dir = destPoint - transform.position;
    //    dir -= middleOffset;
    //    dir += myStackManager.Formation.GetNoise(destPoint);
    //    //dir += CharacterController.Instance.MoveVector();

    //    //dir += myArmy.Formation.GetNoise(destPoint) * (myArmy.Formation.heightScale * randomDir);
    //    //print(myArmy.Formation.GetNoise(destPoint));
    //    //dir += myArmy.Formation.GetNoise(dir);
    //    //Vector3 dir = myArmy.transform.position - transform.position;
    //    //dir = dir.normalized;
    //    //dir.y = rb.velocity.y;

    //    //transform.rotation = Quaternion.Slerp(transform.rotation, myStackManager.transform.rotation, Time.deltaTime * myStackManager.rotateSpeed);


    //    rb.MovePosition(transform.position + dir * Time.deltaTime * stackSpeed);
    //    //rb.AddForce(dir * Time.deltaTime * arrowSpeed, ForceMode.Acceleration);


    //    //Vector3 newPos = Vector3.MoveTowards(transform.position, myArmy._points[myArmy._spawnedUnits.IndexOf(this)], myArmy._unitSpeed * Time.deltaTime);
    //    //newPos.y = rb.position.y;
    //    //transform.position = newPos;


    //    //Vector3 newPos = Vector3.MoveTowards(transform.position, myArmy._points[myArmy._spawnedUnits.IndexOf(this)], myArmy._unitSpeed * Time.deltaTime);
    //    ////newPos.y += rb.position.y + gravityValue * gravityMultiplier * Time.deltaTime;
    //    //rb.MovePosition(newPos);

    //    //transform.position = Vector3.MoveTowards(transform.position, destPoint, arrowSpeed * Time.deltaTime);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, myStackManager.transform.rotation, myStackManager.rotateSpeed * Time.deltaTime);

    //    //GiveMeGravity();

    //}

    private void GoingEnemyMove()
    {
        /*
        Vector3 dir = (GameManager.Instance.ourBase.transform.position - transform.position).normalized;
        dir.y = 0;
        rb.velocity = new Vector3(dir.x * mySpeed, rb.velocity.y * mySpeed / 3, dir.z * mySpeed) * Time.deltaTime;
        */
        if (!myGoPos)
            return;
        float dis = Vector3.Distance(transform.position, myGoPos.position);

        Quaternion lookOnLook = Quaternion.LookRotation((myGoPos.position - transform.position).normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);

        //rb.velocity = new Vector3(transform.forward.x * myArmy._unitSpeed, rb.velocity.y * myArmy._unitSpeed / 3, transform.forward.z * myArmy._unitSpeed) * Time.deltaTime;
        //rb.MovePosition(transform.position + transform.forward * Time.deltaTime * myArmy._unitSpeed);
        transform.position = Vector3.MoveTowards(transform.position, myGoPos.position, speed * Time.deltaTime);

        //GiveMeGravity();

    }

    public void ClampX()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, XClampMin, XClampMax), transform.position.y, transform.position.z);
    }
}
