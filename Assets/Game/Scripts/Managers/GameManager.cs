
public class GameManager : IGameManager
{
    private SymbolPayoutData _symbolPayoutData;
    private PayoutLineData _payoutLineData;

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
}