using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;
using System.Linq;

public class ReelBehavior : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private SymbolBehavior[] _symbolBehaviors;
    [SerializeField]
    private SimpleScrollSnap _simpleScrollSnap;
    private bool _startedSpin;
    private int _reelNumber = 0;
    private Action _onEndSpin;
    private int _centerId;
    private int _targetPosition;

    public void SetReelNumber(int number)
    {
        _reelNumber = number;
        for (int i = 0; i < _symbolBehaviors.Length; i++)
        {
            var symbol = UnityEngine.Random.Range(0, 6);
            _symbolBehaviors[i].SetSymbol(symbol);
        }
    }

    public void OnCenter(int centerId)
    {
        _centerId = centerId;

        
    }

    public int[] GetReelSymbols()
    {
        var centerSymbol = _symbolBehaviors[_targetPosition].GetSelectedSymbol();
        var previousSymbol = _symbolBehaviors[GetPreviousSymbolFromCenter(_targetPosition)].GetSelectedSymbol();
        var nextSymbol = _symbolBehaviors[GetNextSymbolFromCenter(_targetPosition)].GetSelectedSymbol();
        return new int[] { previousSymbol, centerSymbol, nextSymbol };
    }

    public SymbolBehavior[] GetReelItems()
    {
        var centerReelItem = _symbolBehaviors[_targetPosition];
        var previousReelItem = _symbolBehaviors[GetPreviousSymbolFromCenter(_targetPosition)];
        var nextReelItem = _symbolBehaviors[GetNextSymbolFromCenter(_targetPosition)];
        return new SymbolBehavior[] { previousReelItem, centerReelItem, nextReelItem };
    }

    private int GetPreviousSymbolFromCenter(int centerID)
    {
        var previousId = centerID + 1;
        if (previousId >= _symbolBehaviors.Length)
        {
            previousId = 0;
        }
      
        return previousId;
    }
    private int GetNextSymbolFromCenter(int centerID)
    {
        var previousId = centerID - 1;
        if (previousId < 0)
        {
            previousId = _symbolBehaviors.Length - 1;
        }
        return previousId;
    }


    public void Spin(int targetPosition)
    {
        _targetPosition = targetPosition;
      
        StartCoroutine("StopReel");
        _startedSpin = true;
        var reelItems = GetReelItems();
        for (int i = 0; i < _symbolBehaviors.Length; i++)
        {
            if (reelItems.Contains(_symbolBehaviors[i])==false)
            {
                var symbol = UnityEngine.Random.Range(0, 6);
                _symbolBehaviors[i].SetSymbol(symbol);
            }
        }
    }

    private IEnumerator StopReel()
    {
        yield return new WaitForSeconds(3 + (0.45f * _reelNumber));
        _startedSpin = false;
        _simpleScrollSnap.Velocity = new Vector2(0, 0);
        _simpleScrollSnap.GoToPanel(_targetPosition); 
        
        _onEndSpin?.Invoke();
    }

    private void FixedUpdate()
    {
        if (_startedSpin==false)
        {
            return;
        }
        _simpleScrollSnap.Velocity = new Vector2(0, -5000f);

        /*        if (_simpleScrollSnap.Velocity.y >= -5000)
                {
                    _simpleScrollSnap.Velocity -= new Vector2(0, 10f);
                }*/

    }

    public void SetOnEndSpin(Action onEndSpin)
    {
        _onEndSpin = onEndSpin;
    }

    internal void EndSpin()
    {
       
        _startedSpin = false;
        _simpleScrollSnap.Velocity = new Vector2(0, 0);
        _onEndSpin?.Invoke();
    }
}