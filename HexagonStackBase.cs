using ElephantSDK;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class HexagonStackBase : MonoBehaviour
{
    public FormationBase _formation;
    public FormationBase Formation
    {
        get
        {
            if (_formation == null)
            {
                _formation = GetComponent<FormationBase>();
            }

            return _formation;
        }
        set => _formation = value;
    }

    public List<Arrow> stack = new List<Arrow>();

    public List<Vector3> _points = new List<Vector3>();

    public float animSpeed;
    public float rotateSpeed;


    public virtual IEnumerator Start()
    {
        if (GameManager.useRemoteSettings)
        {
            animSpeed = RemoteConfig.GetInstance().GetFloat("animSpeed", 100f);
            rotateSpeed = RemoteConfig.GetInstance().GetFloat("rotateSpeed", 750f);
        }

        _points = Formation.EvaluatePoints().ToList();
        yield return null;
    }


    public virtual void FixedUpdate()
    {
        ArrangePositions();
    }

    [Button]
    public void ArrangePositions()
    {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > stack.Count)
        {
            var remainingPoints = _points.Skip(stack.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < stack.Count)
        {
            Kill(stack.Count - _points.Count);
        }

        //for (int i = 0; i < stack.Count; i++)
        //{
        //    //stack[i].transform.position = _points[i];
        //    stack[i].transform.position = Vector3.MoveTowards(stack[i].transform.position, _points[i], animSpeed * Time.deltaTime);
        //    stack[i].transform.rotation = Quaternion.RotateTowards(stack[i].transform.rotation, this.transform.rotation, rotateSpeed * Time.deltaTime);
        //}


    }


    public void AddMeToStackAndEveluate(Arrow arrow)
    {
        Formation.amount++;
        Formation.EvaluatePoints();
        AddMeToStack(arrow);
    }
    public virtual void AddMeToStack(Arrow arrow)
    {
        arrow.Initialize();
        stack.Add(arrow);
        //arrow.transform.SetParent(this.transform);
        arrow.myStackManager = this;
        arrow.amIInStack = true;
        arrow.amIStable = false;
        //ChangeArrowMat(arrow);
        arrow.ChangeMaterial(1);
        arrow.SetActiveTrail(false);
        arrow.transform.SetParent(null);
        MMVibrationManager.Haptic(HapticTypes.Selection);


        //ArrangePositions();
    }


    public void RemoveMeFromStackAndEveluate(Arrow arrow)
    {
        Formation.amount--;
        Formation.EvaluatePoints();
        RemoveMeFromStack(arrow);
    }
    public virtual void RemoveMeFromStack(Arrow arrow)
    {
        stack.Remove(arrow);
        //arrow.transform.SetParent(null);

        arrow.amIInStack = false;
        arrow.myStackManager = null;
        MMVibrationManager.Haptic(HapticTypes.Selection);


        //ArrangePositions();
    }

    private void Spawn(IEnumerable<Vector3> points)
    {
        foreach (var pos in points)
        {
            //var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
            Vector3 randomPos = new Vector3(0, UnityEngine.Random.Range(-1f, 2f), UnityEngine.Random.Range(-1f, 2f));
            randomPos *= 0.2f;
            //var unit = Instantiate(GameData.Instance._arrowPrefab, transform.position + randomPos, this.transform.rotation);
            var unit = GameManager.Instance.arrowPool.Pull(transform.position + randomPos, this.transform.rotation);
            var arrow = unit.GetComponent<Arrow>();
            arrow.Initialize();
            AddMeToStack(arrow);
            if (GameManager.Instance.useAccelerationOnSpawn)
                arrow.amIOnAcceleration = true;
            //stack.Add(arrow);
            //arrow.Initialize();

        }

        #region OldCode
        //for (int i = points.Count<Vector3>() - 1; i >= 0; i--)
        //{
        //    //var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
        //    Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-1f, 2f), 0, UnityEngine.Random.Range(-1f, 2f));
        //    randomPos *= 0.2f;
        //    var unit = Instantiate(_unitPrefab, transform.position + points.ElementAt(i), Quaternion.identity, _parent);
        //    _spawnedUnits.Add(unit);
        //    unit.GetComponent<Soldier>().Initialize(this);
        //} 
        #endregion
    }

    public Arrow SpawnArrow()
    {
        Vector3 randomPos = new Vector3(0, UnityEngine.Random.Range(-1f, 2f), UnityEngine.Random.Range(-1f, 2f));
        randomPos *= 0.2f;
        //var unit = Instantiate(GameData.Instance._arrowPrefab, transform.position + randomPos, this.transform.rotation);
        var unit = GameManager.Instance.arrowPool.Pull(transform.position + randomPos, transform.rotation);
        var arrow = unit.GetComponent<Arrow>();
        arrow.Initialize();
        AddMeToStackAndEveluate(arrow);
        return arrow;
    }

    private void Kill(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var arrow = stack.Last();
            //stack.Remove(unit);
            RemoveMeFromStack(arrow);
            //Destroy(arrow.gameObject);
            arrow.gameObject.SetActive(false);
        }
    }



    public void SetArrowCount(int newCount)
    {
        Formation.amount = newCount;
        Formation.EvaluatePoints();

    }

    public bool CheckIsStackEmpty()
    {
        return stack.Count <= 0;
    }

}
