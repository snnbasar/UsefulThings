using DG.Tweening;
using ElephantSDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Army myArmy;
    //public int myId;
    public int myTeamId;


    private Rigidbody rb;
    private Animator anim;

    //float randomDir;
    Vector3 middleOffset;


    public Transform myGoPos;
    public bool goToEnemy;

    private GameObject deathParticle;
    //[SerializeField] private float gravityValue = -9.81f;
    //[SerializeField] private float gravityMultiplier = 5f;
    [SerializeField] private float fallingTolerance;

    public bool amIMiddleSoldier;
    public bool doingGiantFinal;
    public bool doingTriangleFinal;


    public int _power;
    public int power
    {
        get { return _power; }
        set
        {
            _power = value;

        }
    }
    public int size;
    public Vector3 sizeVector;
    public static float vectorSize = 0.1f;

    public float timeToSizeCooldown;
    float time;

    private MeshRenderer renderer;

    private bool amIPlayer;

    public void Initialize(Army army)
    {
        //if (GameManager.useRemoteSettings)
        //{
        //    fallingTolerance = RemoteConfig.GetInstance().GetFloat("fallingTolerance", -2f);
        //    vectorSize = RemoteConfig.GetInstance().GetFloat("vectorSize", 0.1f);
        //}


        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        //rb.drag = 0;
        myArmy = army;
        //myId = myArmy._spawnedUnits.IndexOf(this.gameObject);
        //randomDir = Extensions.GetRandomMinusOneOrOne();
        middleOffset = new Vector3(transform.localScale.x * 0.5f, 0, transform.localScale.z * 0.5f);
        myTeamId = army.armyId;
        amIPlayer = myArmy.amIPlayer;

        anim = GetComponentInChildren<Animator>();

        //this.gameObject.tag = "Soldier";

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        this.renderer = renderer;
        Material mat = renderer.sharedMaterial;
        mat = army.myMaterial;
        renderer.sharedMaterial = mat;

        if (!army.particleDeath)
            return;
        deathParticle = Instantiate(army.particleDeath, this.transform.position + Vector3.up * 0.5f, Quaternion.identity, this.transform);
        deathParticle.SetActive(false);

        sizeVector = new Vector3(vectorSize, vectorSize, vectorSize);

    }

    private void Update()
    {
        CheckClamp();
        CheckFalling();

        CheckMySize();
    }

    float timer = 0;
    bool doBlendshapeAnim;
    private void LateUpdate()
    {
        if (!doBlendshapeAnim)
            return;
        timer += Time.deltaTime;
        float x = timer * 100;
        if (x >= 100)
            x = 100;
        //renderer.SetBlendShapeWeight(0, x);
        
    }

    private void CheckMySize()
    {
        if (!amIMiddleSoldier)
            return;
        if (power > size)
        {
            time += Time.deltaTime;
            if (time < timeToSizeCooldown)
                return;
            time = 0;

            this.transform.localScale += sizeVector;
            this.transform.Translate(0, vectorSize, 0);
            size++;
        }
        else if (power < size)
        {
            time += Time.deltaTime;
            if (time < timeToSizeCooldown)
                return;
            time = 0;

            this.transform.localScale -= sizeVector;
            this.transform.Translate(0, -vectorSize, 0);
            size--;
        }
    }

    void FixedUpdate()
    {
        if (!myArmy)
            return;
        


        if(goToEnemy)
            GoingEnemyMove();
        else
            NormalMove();



    }

    private void NormalMove()
    {
        //Vector3 dir = (myArmy.transform.position + myArmy._points[myId]) - transform.position;
        Vector3 destPoint = myArmy._points[myArmy._spawnedUnits.IndexOf(this)];
        //Vector3 dir = (myArmy.transform.position + destPoint) - transform.position;
        Vector3 dir = destPoint - transform.position;
        dir -= middleOffset;
        dir += myArmy.Formation.GetNoise(destPoint);
        //dir += CharacterController.Instance.MoveVector();

        //dir += myArmy.Formation.GetNoise(destPoint) * (myArmy.Formation.heightScale * randomDir);
        //print(myArmy.Formation.GetNoise(destPoint));
        //dir += myArmy.Formation.GetNoise(dir);
        //Vector3 dir = myArmy.transform.position - transform.position;
        //dir = dir.normalized;
        //dir.y = rb.velocity.y;

        transform.rotation = Quaternion.Slerp(transform.rotation, myArmy.transform.rotation, Time.deltaTime * 10f);


        //rb.MovePosition(transform.position + dir * Time.deltaTime * myArmy._unitSpeed);
        //rb.AddForce(dir * Time.deltaTime * myArmy._unitSpeed, ForceMode.Acceleration);


        //Vector3 newPos = Vector3.MoveTowards(transform.position, myArmy._points[myArmy._spawnedUnits.IndexOf(this)], myArmy._unitSpeed * Time.deltaTime);
        //newPos.y = rb.position.y;
        //transform.position = newPos;


        //Vector3 newPos = Vector3.MoveTowards(transform.position, myArmy._points[myArmy._spawnedUnits.IndexOf(this)], myArmy._unitSpeed * Time.deltaTime);
        ////newPos.y += rb.position.y + gravityValue * gravityMultiplier * Time.deltaTime;
        //rb.MovePosition(newPos);

        transform.position = Vector3.MoveTowards(transform.position, destPoint, myArmy._unitSpeed * Time.deltaTime);

        GiveMeGravity();

        float moveDis = 0.5f;
        if (anim)
            anim.SetBool("running", amIPlayer ? PlayerController.Instance.IsMoving() : dir.sqrMagnitude > moveDis);
    }


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
        transform.position = Vector3.MoveTowards(transform.position, myGoPos.position, myArmy._unitSpeed * Time.deltaTime);

        GiveMeGravity();

        if (anim)
            anim.SetBool("running", dis != 0 || dis != 0);

    }

    public void CheckClamp()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, myArmy.XClampMin, myArmy.XClampMax), transform.position.y, transform.position.z);
    }

    private void CheckFalling()
    {
        if(transform.position.y <= fallingTolerance)
        {
            KillMe();
        }
    }

    private void GiveMeGravity()
    {
        if (doingTriangleFinal)
            return;
        GiveMeForce(Vector3.down * 1000); //Gravity
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Soldier"))
        //{
        //    if(collision.gameObject.TryGetComponent<Soldier>(out Soldier soldier))
        //    {
        //        if (myTeamId != soldier.myTeamId)
        //        {
        //            KillMe();
        //        }

        //    }
        //}

        //if (collision.gameObject.CompareTag("Obstacle"))
        //{
        //    KillMe();
        //}

    }

    private void OnCollisionStay(Collision collision)
    {
        //if (!doingGiantFinal)
        //    return;
        //if (collision.gameObject.TryGetComponent<Soldier>(out Soldier soldier))
        //{
        //    if (soldier.amIMiddleSoldier)
        //    {
        //        soldier.IncreaseMySize(1);
        //        KillMe();
        //    }
        //}


    }

    private void KillMe()
    {
        myArmy._spawnedUnits.Remove(this);
        myArmy.Formation.amount--;
        PlayDeathParticle();
        Destroy(this.gameObject);
    }

    private void PlayDeathParticle()
    {
        if (myArmy.particleDeath)
        {

            deathParticle.transform.SetParent(null);
            deathParticle.SetActive(true);
            Destroy(deathParticle, 1f);
        }
    }

    public void SetEnemy(Transform enemy)
    {
        myGoPos = enemy;
        goToEnemy = true;
    }


    public void GiveMeForce(Vector3 dir)
    {
        rb.AddForce(dir);
    }

    public void IncreaseMySize(int a)
    {
        power += a;
    }

    public void DoGiantInitialize(Soldier middleSoldier)
    {
        doingGiantFinal = true;
        //if (rb.useGravity == false)
        //    rb.useGravity = true;
        if (middleSoldier == this)
            return;
        SetEnemy(middleSoldier.transform);
    }

    public void DoTriangleInitialize()
    {
        doingTriangleFinal = true;
        rb.useGravity = false;
    }

    [Button]
    public void ChangeMyBlendShapeMuscle()
    {

        doBlendshapeAnim = true;
    }
}
