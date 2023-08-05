using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SlotView : MonoBehaviour
{
    [SerializeField]
    private Button _spinButton;
    [Inject]
    private ISlotManager _slotManager;
    [SerializeField]
    private ReelBehavior[] _reels;
    [SerializeField]
    private LineRenderer _lineRenderer;
    private Action<List<SymbolBehavior>> _onEndSpin;
    private int _reelEndCounter;
    private List<List<int>> _winningLines;
    private List<SymbolBehavior> _currentReelList = new List<SymbolBehavior>();
    private bool _isCooldown;
    private List<SymbolBehavior> _reelSymbols = new List<SymbolBehavior>();
    private void Start()
    {
        _spinButton.onClick.AddListener(OnClickSpin);
        _slotManager.SetView(this);

        Setup();
    }

    private IEnumerator StartSpinCooldown()
    {
        _isCooldown = true;
        yield return new WaitForSeconds(1f);
        _isCooldown = false;
    }

    private void Setup()
    {
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].SetReelNumber(i);
            _reels[i].SetOnEndSpin(OnEndSpin);
        }
    }

    private void OnClickSpin()
    {
        StartCoroutine("StartSpinCooldown");
        _slotManager.StartSpin();
    }

    public void StartSpin(List<int> position)
    {
        ClearAnimation();
        _reelEndCounter = 0;
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].Spin(position[i]);
        }
    }

    private void OnEndSpin()
    {
        _reelEndCounter++;
        _currentReelList = new List<SymbolBehavior>();
        if (_reelEndCounter >= _reels.Length)
        {
            //All reels ended
            var reelList = "";
            for (int i = 0; i < _reels.Length; i++)
            {
               
                var reelItemArray = _reels[i].GetReelItems();
                for (int j= 0; j < reelItemArray.Length; j++)
                {
                    _currentReelList.Add(reelItemArray[j]);
                    reelList += reelItemArray[j].transform.parent.name + reelItemArray[j].name + ",";

                }
               
            }
           
            StartCoroutine("OnEndSpinStart");
           
        }
    }

    private IEnumerator OnEndSpinStart()
    {
        yield return new WaitForSeconds(1f);
        _onEndSpin.Invoke(_currentReelList);
    }
    public void SetOnEndSpin(Action<List<SymbolBehavior>> onEndSpin)
    {
        _onEndSpin = onEndSpin;
    }

    public void AnimateLines(List<List<int>> winningLines)
    {
       
       
        _winningLines = winningLines;
       
        for (int i = 0; i < _winningLines.Count; i++)
        {
            var stringWin = "";
          
            for (int j = 0; j < _winningLines[i].Count; j++)
            {
                stringWin += _winningLines[i][j];
             
            }
           // Debug.Log("stringWin :" + stringWin);
        }
        ClearAnimation();
        StartCoroutine("StartLineAnimation");
    }

    private IEnumerator StartLineAnimation()
    {
        var animationString = "";
        for (int i = 0; i < _winningLines.Count; i++)
        {
            animationString = "";
            for (int j = 0;j < _winningLines[i].Count;j++)
            {
                animationString += _winningLines[i][j];
                _currentReelList[_winningLines[i][j]].Blur();
            }
            yield return new WaitForSeconds(1.25f);
        }
        ClearAnimation();
        StartCoroutine("StartLineAnimation");
        
    }
    private void ClearAnimation()
    {
        if (_winningLines ==null)
        {
            return;
        }
        if(_winningLines.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < _winningLines.Count; i++)
        {
            for (int j = 0; j < _winningLines[i].Count; j++)
            {
                _currentReelList[_winningLines[i][j]].ClearAnimation();
            }
        }
        StopCoroutine("StartLineAnimation");
    }

    public void EndSpin()
    {
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].EndSpin();
        }
    }
}
