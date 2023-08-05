using System;
using UnityEngine;

public interface IGameManager 
{
    public void SetRemoteConfigData(SymbolPayoutData symbolPayoutData, PayoutLineData payoutLineData);
    public SymbolPayoutData GetSymbolPayoutData();
    public PayoutLineData GetPayoutLineData();
    public void Clear();
    public void SetView(GameView gameView);
    public void ShowReward(int winReward);
    public void AddMoney(int amount, bool showResult = false);
    public void SpendMoney(int amount, Action onSuccessSpend);
    public int GetMoney();
    public int GetBetValue();
    public void IncreaseDecreaseBet(bool isIncrease);
}