using ElephantSDK;
using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Army : MonoBehaviour {
    public FormationBase _formation;

    public FormationBase Formation {
        get {
            if (_formation == null)
            {
                _formation = GetComponent<FormationBase>();
                _formation.amIPlayer = amIPlayer;
            }

            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private GameObject _unitPrefab;
    public float _unitSpeed = 2;

    public List<Soldier> _spawnedUnits = new List<Soldier>();
    public List<Vector3> _points = new List<Vector3>();
    private Transform _parent;

    public int armyId;

    public float XClampMin;
    public float XClampMax;


    public Material myMaterial;

    public bool amIPlayer;

    [Header("FX")]
    public GameObject particleDeath;

    [Header("UI")]
    public TextMeshProUGUI amountText;
    public GameObject armyBorderImage;

    private void Awake() {
        _parent = new GameObject("Unit Parent").transform;
        _parent.SetParent(this.transform);
    }

    private void Start()
    {
        

        //PlayerController.Instance.armyParent = _parent;


        //Formation.OnAmountChange += SetAmountText;
        //To Invoke OnAmountChange
        int startAmount = Formation.amount;
        Formation.amount = startAmount;

        Color newColor = myMaterial.color;
        newColor.a = 80f / 100f;
        //amountText.transform.parent.GetComponent<Image>().color = newColor;

    }
    private void FixedUpdate() {
        SetFormation();
    }

    private void SetFormation() {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > _spawnedUnits.Count) {
            var remainingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < _spawnedUnits.Count) {
            Kill(_spawnedUnits.Count - _points.Count);
        }


        #region OldCode
        for (var i = 0; i < _spawnedUnits.Count; i++)
        {
            _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
            //Vector3 dir = (transform.position + _points[i]) - _spawnedUnits[i].transform.position;

            //_spawnedUnits[i].GetComponent<Rigidbody>().MovePosition(_spawnedUnits[i].transform.position + PlayerController.Instance.MoveVector() * Time.deltaTime * PlayerController.Instance.playerSpeed);
        }
        #endregion

    }


    private void Spawn(IEnumerable<Vector3> points) {
        foreach (var pos in points)
        {
            //var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
            Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-1f, 2f), 0, UnityEngine.Random.Range(-1f, 2f));
            randomPos *= 0.2f;
            var unit = Instantiate(_unitPrefab, transform.position + randomPos, this.transform.rotation, this.transform);
            var soldier = unit.GetComponent<Soldier>();
            _spawnedUnits.Add(soldier);
            soldier.Initialize(this);
            if (amIPlayer)
                MMVibrationManager.Haptic(HapticTypes.Selection);
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

    private void Kill(int num) {
        for (var i = 0; i < num; i++) {
            var unit = _spawnedUnits.Last();
            _spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
            if(amIPlayer)
                MMVibrationManager.Haptic(HapticTypes.Selection);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<Army>(out Army army))
        //{
        //    if(armyId == 0)
        //        StartCoroutine(DoBattle(army));
        //}
    }

    private IEnumerator DoBattle(Army army)
    {
        PlayerController.Instance.SetCanMove(false);

        for (int i = 0; i < _spawnedUnits.Count; i++)
        {
            if (i > army._spawnedUnits.Count - 1)
                break;
            var _soldier = _spawnedUnits[i];

            var rndmSoldier = army._spawnedUnits[i];

            if (rndmSoldier)
            {
                _soldier.SetEnemy(rndmSoldier.transform);
                rndmSoldier.SetEnemy(_soldier.transform);
            }
            
            yield return null;

        }

        bool doBattle = true;
        while (doBattle)
        {
            if(_spawnedUnits.Count > 0 && army._spawnedUnits.Count > 0)
            {
                for (int i = 0; i < _spawnedUnits.Count; i++)
                {

                    if (i > army._spawnedUnits.Count - 1)
                        break;

                    var _soldier = _spawnedUnits[i];

                    var rndmSoldier = army._spawnedUnits[i];

                    if (_soldier.myGoPos == null)
                    {
                        if (rndmSoldier)
                        {
                            _soldier.SetEnemy(rndmSoldier.transform);
                        }

                    }

                    if (rndmSoldier)
                    {

                        if (rndmSoldier.myGoPos == null)
                        {
                            rndmSoldier.SetEnemy(_soldier.transform);
                        }

                    }
                    yield return null;
                }

                //yield return new WaitForFixedUpdate();
            }
            if (_spawnedUnits.Count <= 0)
            {
                doBattle = false;
                LevelManager.Instance.GameOver();

                //for (int i = 0; i < army._spawnedUnits.Count; i++)
                //{
                //    army._spawnedUnits[i].goToEnemy = false;
                //    yield return null;

                //}
            }
            if (army._spawnedUnits.Count <= 0)
            {
                doBattle = false;
                PlayerController.Instance.SetCanMove(true);

                for (int i = 0; i < _spawnedUnits.Count; i++)
                {
                    _spawnedUnits[i].goToEnemy = false;
                    yield return null;

                }
            }
            yield return null;
        }

    }


    public void SetAmountText(int amount)
    {
        if (amount <= 0)
        {
            amountText.transform.parent.gameObject.SetActive(false);
            if (!amIPlayer)
                armyBorderImage.SetActive(false);
        }
        else
        {
            if(!amountText.transform.parent.gameObject.activeSelf /*&& !GameManager.Instance.doingFinal*/)
                amountText.transform.parent.gameObject.SetActive(true);
            amountText.text = amount.ToString();
            if(!amIPlayer)
                armyBorderImage.SetActive(true);

        }
    }

    public void SetActiveAmountText(bool sts)
    {
        amountText.transform.parent.gameObject.SetActive(sts);

    }

}