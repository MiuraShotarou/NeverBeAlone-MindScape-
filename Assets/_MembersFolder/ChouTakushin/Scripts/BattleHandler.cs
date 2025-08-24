using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class BattleHandler : MonoBehaviour
{
    public List<GameObject> EnemyUnits = default;
    public List<GameObject> PlayerUnits = default;
    private List<GameObject> _battleUnitList = new List<GameObject>();
    private Queue<GameObject> _battleUnitQueue = new Queue<GameObject>();
    private int _battleUnitCount = 0;
    private bool _playerOneMoreFlg = false;


    private bool _enemyOneMore = false;
    private bool _isHandlerBusy = false;

    private BattleState _battleState;
    [SerializeField] private GameObject _roulettePanel = default;
    [SerializeField] private GameObject _startText = default;
    [SerializeField] private GameObject _targetSelectText = default;
    [SerializeField] private GameObject _commandPanel = default;
    [SerializeField] private GameObject _playableHandler = default;
    [SerializeField] private GameObject _turnTable = default;
    [SerializeField] PlayableDirector _director = default;

    private GameObject _selectedUnit = null; 
    private bool _isBattleOver = false;
    private RouletteStatus _rouletteResult = RouletteStatus.Stop;
    
    public BattleState BattleState
    {
        get { return _battleState; }
        set { _battleState = value; }
    }
    public bool PlayerOneMoreFlg
    {
        get => _playerOneMoreFlg;
        set => _playerOneMoreFlg = value;
    }

    public bool EnemyOneMore
    {
        get => _enemyOneMore;
        set => _enemyOneMore = value;
    }

    /// <summary>
    /// QTE���茋��
    /// </summary>
    public enum RouletteStatus
    {
        Stop, Rolling, Miss, Good, Excellent
    }

    public enum BattleStatus
    {
        Wait, Busy
    }
    
    void Start()
    {
        _battleUnitCount = EnemyUnits.Count + PlayerUnits.Count;
        _battleState = BattleState.Init;
    }

    void Update()
    {
        // if (_isHandlerBusy) return;

        switch (_battleState)
        {
            case BattleState.Init:
                _battleState = BattleState.Busy;
                // �����ݒ�
                InitBattleData();
                break;
            case BattleState.Intro:
                _battleState = BattleState.Busy;
                // �퓬�J�nAnimation�Đ�
                PlayBattleIntro();
                break;
            case BattleState.Roulette:
                _battleState = BattleState.WaitForCommand;
                // QTE���s
                PlayRoulette();
                break;
            case BattleState.RouletteEnd:
                _battleState = BattleState.Busy;
                // �s��������
                SortBattleUnits();
                // �^�[���e�[�u��UI�\��
                InitTurnTable();
                break;
            case BattleState.TurnStart:
                _battleState = BattleState.WaitForCommand;
                // �s��Unit�擾
                GameObject unit = _battleUnitQueue.Peek();
                BattleUnitTakeAction(unit);
                break;
            case BattleState.TurnEnd:
                _battleState = BattleState.Busy;
                // �^�[���I������
                EndTurn();
                break;
            case BattleState.BattleEnd:
                break;
            case BattleState.Busy:
            case BattleState.WaitForCommand:
            default:
                break;
        }
    }

    /*
    IEnumerator BattleLoop()
    {
        // �����f�[�^�p��
        InitBattleData();

        // �퓬�������o
        PlayBattleIntro();

        // QTE���s
        PlayRoulette();

        // �s��������
        SortBattleUnits();

        // QTE��̉��o
        // UI��TurnTable���X�V
        UpdateTurnTable(_battleUnits);

        // �퓬�L�����N�^�[��index
        int battleUnitIndex = 0;
        while (!_isBattleOver)
        { 
            GameObject unitGo = _battleUnits[battleUnitIndex];
            BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
            if(unit is BattleUnitPlayer)
            {
                // �v���C���[�s���̏ꍇ�A���j���[��\�����āA������󂯕t����
                BattleUnitActionBase unitAction = null;
                ShowCommandMenu((p) => { unitAction = p; });
                //PlayerChooseCommand();
                yield return new WaitUntil(() => unitAction != null);
                Debug.Log("Player Action Selected");
                //PlayerDoUnitAction();
            }
            else
            {
                // �v���C���[�ȊO���s������ꍇ�A������Ԃł���ꍇ����AI�ݒ�ɏ]���čs������
                if (!unit._isDead)
                {
                    
                }
            }
            if (battleUnitIndex >= _battleUnitCount - 1)
            {
                battleUnitIndex = 0;
            }
            else
            {
                // One More�����𖞂����Ȃ��ꍇ�A���̃��j�b�g���s������
                if (!_playerOneMore)
                {
                    battleUnitIndex++;
                }
            }
        }
        //yield return null;
    }
    */

    void InitBattleData()
    {
        // TODO ����������
        _battleState = BattleState.Intro;
    }
    /// <summary>
    /// �퓬�������o���Đ�����
    /// </summary>
    /// <returns></returns>
    void PlayBattleIntro()
    {
        _startText.SetActive(true);
        _director.Play();
    }

    /// <summary>
    /// �퓬�������o�I�����ɌĂяo�����
    /// </summary>
    public void OnBattleIntroFinished()
    {
        _startText.SetActive(false);
        _battleState = BattleState.Roulette;
    }
    
    /// <summary>
    /// �yDEBUG�z QTE�J�n
    /// </summary>
    public void PlayRoulette()
    {
        _roulettePanel.SetActive(true);
        _rouletteResult = RouletteStatus.Rolling;
    }
    public void SetRouletteResult(string rst)
    {
        switch (rst)
        {
            case "Miss":
                _rouletteResult = RouletteStatus.Miss;
                break;
            case "Good":
                _rouletteResult = RouletteStatus.Good;
                break;
            case "Excellent":
                _rouletteResult = RouletteStatus.Excellent;
                break;
            default:
                break;
        }
        _roulettePanel.SetActive(false);
        _battleState = BattleState.RouletteEnd;
    }

    void SortBattleUnits()
    {
        // QTE���肪Miss�ȊO�̏ꍇ
        if(_rouletteResult != RouletteStatus.Miss)
        {
            // �G�l�~�[���Ƀ^�[���e�[�u���ɓ���āA�\�[�g����
            _battleUnitList.AddRange(EnemyUnits);
            _battleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            // �v���C���[��퓬�ɍ�������
            _battleUnitList.InsertRange(0, PlayerUnits);
            _battleUnitQueue = new Queue<GameObject>(_battleUnitList);
        }
        else
        {
            // QTE���肪Miss�̏ꍇ�A
            // �G�l�~�[�ƃv���C���[�𓯂����X�g�ɓ���āA�\�[�g����
            _battleUnitList.AddRange(EnemyUnits);
            _battleUnitList.InsertRange(0, PlayerUnits);
            _battleUnitList.Sort((a, b) =>
            {
                return b.GetComponent<BattleUnitBase>()._dex - a.GetComponent<BattleUnitBase>()._dex;
            });
            _battleUnitQueue = new Queue<GameObject>(_battleUnitList);
        }
        //
        // foreach (GameObject unit in _battleUnitList)
        // {
        //     Debug.Log("Unit Name: " + unit.GetComponent<BattleUnitBase>()._unitName);
        // }
    }
    /// <summary>
    /// ���j���[�\�����A�v���C���[
    /// </summary>
    /// <param name="setUnitAction"></param>
    void PlayerChooseCommand(Action<BattleUnitActionBase> setUnitAction)
    {
        BattleUnitActionBase rst = new BattleUnitActionBase();
        setUnitAction.Invoke(rst);
    }
    void ShowCommandMenu()
    {
        _commandPanel.SetActive(true);
    }
    
    /// <summary>
    /// TODO TurnTable�̍X�VAnimation�Đ�
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayTurnTableUpdate()
    {
        return null;
    }

    public void InitTurnTable()
    {
        _turnTable.SetActive(true);
        foreach (var unitGo in _battleUnitList)
        {
            BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
            unit._icon.gameObject.SetActive(true);
            unit._icon.transform.SetParent(_turnTable.transform);
            unit._icon.transform.SetAsFirstSibling();
        }
        _battleState = BattleState.TurnStart;
    }
    public void UpdateTurnTable(GameObject unitGo)
    {
        unitGo.GetComponent<BattleUnitBase>()._icon.transform.SetAsLastSibling();
    }

    void BattleUnitTakeAction(GameObject unitGo)
    {
        BattleUnitBase unit = unitGo.GetComponent<BattleUnitBase>();
        if (unit is BattleUnitPlayer)
        {
            ShowCommandMenu();
        }
        else
        {
            EnemyTakeAction(unitGo);
        }
    }

    public void OnPlayerChooseAction()
    {
        DeactiveCommandPanel();
        ActiveTargetSelectText();
        _battleState = BattleState.WaitForTargetSelect;
    }

    /// <summary>
    /// �^�[���I������
    /// </summary>
    public void EndTurn()
    {
        // �s������̐擪unit���s���z��̍Ō���Ɉړ�
        GameObject unitToDequeue = _battleUnitQueue.Dequeue();
        _battleUnitQueue.Enqueue(unitToDequeue);

        // �^�[���e�[�u���X�V
        UpdateTurnTable(unitToDequeue);

        _battleState = BattleState.TurnStart;
    }
    
    public void DeactiveCommandPanel()
    {
        _commandPanel.SetActive(false);
    }
    public void ActiveTargetSelectText()
    {
        _targetSelectText.SetActive(true);
    }
    public void DeactiveTargetSelectText()
    {
        _targetSelectText.SetActive(false);
    }
    public void EnemyTakeAction(GameObject unitGo)
    {
        // �e�X�g�p
        unitGo.GetComponent<BattleUnitEnemyBase>().DoAttack1(PlayerUnits[0]);
    }
    
    public void TestInitializer()
    {
    }
}
