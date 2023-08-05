using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class SlotManager : ISlotManager
{
    [Inject]
    private IGameManager _gameManager;
    [Inject]
    private MainThreadHandler _mainThreadHandler;
    private SlotView _view;
    private bool _isSpinning;

    public void SetView(SlotView slotView)
    {
        _view = slotView;
        Init();
    }

    private void Init()
    {
        _view.SetOnEndSpin(OnEndSpin);
    }



    public void StartSpin()
    {
        var predefinedPosition = new List<int>();

        var betValue = _gameManager.GetBetValue();
        _gameManager.SpendMoney(betValue, delegate {
            _mainThreadHandler.AddMainThreadAction(delegate {
                if (_isSpinning)
                {
                    _view.EndSpin();
                }
                else
                {
                    _gameManager.Clear();
                    for (int i = 0; i < 6; i++)
                    {
                        //for now randomized position
                        predefinedPosition.Add(UnityEngine.Random.Range(0,6));
                    }
                    _view.StartSpin(predefinedPosition);
                    _isSpinning = true;
                }
               
            });
        });
        
    }


    public void OnEndSpin(List<SymbolBehavior> reelSymdbols)
    {
        ComputeLineWins(reelSymdbols);
    }

    private void ComputeLineWins(List<SymbolBehavior> reelSymdbols)
    {
        var payOutLines = _gameManager.GetPayoutLineData().PayoutLineModels;
    
        //no start symbol yet
        var startSymbol = -1;
        var winningLines = new List<List<int>>();
        var payOutReward = 0;
        var crossoverCombinations = new List<List<int>>();

        for (int i = 0; i < payOutLines.Count; i++)
        {
            var payoutLevel = 1;
            var lineArray = new List<int>();
            var payOutCoordinates = payOutLines[i].GetCoordinatesArray();
            startSymbol = -1;
            for (int j = 0; j < payOutCoordinates.Length; j++)
            {
                var currentLineTarget = payOutCoordinates[j] - 1;
                
                if (startSymbol == -1)
                {
                    startSymbol = reelSymdbols[currentLineTarget].GetSelectedSymbol();
                    lineArray.Add(currentLineTarget);
                }
                else
                {
                    if (startSymbol == reelSymdbols[currentLineTarget].GetSelectedSymbol())
                    {
                        payoutLevel++;
                        lineArray.Add(currentLineTarget);
                    }
                    else
                    {
                        break;
                    }
                    
                }
            }

            if (crossoverCombinations.Contains(lineArray))
            {
                payoutLevel = 1;
            }


            var payOutValue = GetPayout(payoutLevel,startSymbol);
            if(payOutValue > 0)
            {
                payOutReward += payOutValue;
                winningLines.Add(lineArray);

                var combination = new List<int>();
                for (int j = 0; j < lineArray.Count; j++)
                {
                    combination.Add(lineArray[j]);
                    crossoverCombinations.Add(combination);
                }
            }

        }

        if (winningLines.Count > 0)
        {
            _gameManager.ShowReward(payOutReward);
            _view.AnimateLines(winningLines);
            
        }
        _isSpinning = false;
    }


    private int GetPayout(int payoutLevel, int startSymbol)
    {
        var payOutSymbol = _gameManager.GetSymbolPayoutData().SymbolPayoutModels;
        var payOutValue = 0;
        switch (payoutLevel)
        {
            case 3:
                payOutValue = payOutSymbol[startSymbol].ThreePayout;
                break;
            case 4:
                payOutValue = payOutSymbol[startSymbol].FourPayout;
                break;
            case 5:
                payOutValue = payOutSymbol[startSymbol].FivePayout;
                break;
        }
        return payOutValue;
    }
}