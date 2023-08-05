
using DG.Tweening;
using System;
using UnityEngine;

public class GameManager : IGameManager
{
    private GameView _gameView;
    private SymbolPayoutData _symbolPayoutData;
    private PayoutLineData _payoutLineData;
    private int _currentBet = 1;
    private int _betValue = 100;
    public void SetView(GameView gameView)
    {
        _gameView = gameView;
        _gameView.ShowMoneyText(GetMoney());
    }

    public void SetRemoteConfigData(SymbolPayoutData symbolPayoutData, PayoutLineData payoutLineData)
    {
        _symbolPayoutData = symbolPayoutData;
        _payoutLineData = payoutLineData;
    }

    public SymbolPayoutData GetSymbolPayoutData()
    {
        return _symbolPayoutData;
    }

    public PayoutLineData GetPayoutLineData()
    {
        return _payoutLineData;
    }

    public void Clear()
    {
        _gameView.Clear();
    }

    public void ShowReward(int winReward)
    {
        var currentReward = winReward;
        currentReward = currentReward * (_betValue / 10) * _currentBet;
        _gameView.ShowReward(currentReward);
        AddMoney(currentReward);
    }

    public void AddMoney(int amount,bool showResult=false)
    {
        var currentMoney = GetMoney();
        PlayerPrefs.SetInt("Anino_TotalMoney", currentMoney + amount);
        if (showResult)
        {
            _gameView.ShowMoneyText(currentMoney);
        }
      
    }

    public void SpendMoney(int amount, Action onSuccessSpend)
    {
        var currentMoney = GetMoney();
        if (currentMoney >= amount)
        {
            onSuccessSpend?.Invoke();
            currentMoney -= amount;
            PlayerPrefs.SetInt("Anino_TotalMoney", currentMoney);
            _gameView.ReduceMoney(currentMoney);
        }
    }

    public int GetMoney()
    {
        return PlayerPrefs.GetInt("Anino_TotalMoney", 10000);

    }
    
    public int GetBetValue()
    {
        return _currentBet * _betValue;
    }

    public void IncreaseDecreaseBet(bool isIncrease)
    {
        //magic number 100 for bet default
    
        _currentBet += isIncrease?1:-1;
        if(_currentBet > 20)
        {
            _currentBet = 1;
        }

        if (_currentBet <= 0)
        {
            _currentBet = 20;
        }
        _gameView.ShowBetValue(_currentBet * _betValue);
    }
}