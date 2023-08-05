
using DG.Tweening;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameView : MonoBehaviour
{
    [Inject]
    private MainThreadHandler _mainThreadHandler;
    [Inject]
    private IGameManager _gameManager;
    [SerializeField]
    private TextMeshProUGUI _winText;
    [SerializeField]
    private TextMeshProUGUI _moneyText;
    [SerializeField]
    private TextMeshProUGUI _betValueText;
    [SerializeField]
    private Button _decreaseBet;
    [SerializeField]
    private Button _onClickInfoPopUp;
    [SerializeField]
    private Button _onCloseInfoPopUp;
    [SerializeField]
    private Button _onClickAddMoney;
    [SerializeField]
    private Button _increaseBet;
    [SerializeField]
    private GameObject _infoPopUp;

    public void ShowMoneyText(int moneyText)
    {
        _moneyText.text = moneyText.ToString();
    }


    public void Clear()
    {
        _winText.text = "0";
    }

    public void ReduceMoney(int newMoney)
    {
        var currentMoney = int.Parse(_moneyText.text);
        DOTween.To(() => currentMoney, x => _moneyText.text = (x.ToString()),  newMoney, 0.8f);
    }

    public void ShowReward(int winReward)
    {
        var currentMoney = int.Parse(_moneyText.text);
        DOTween.To(() => 0, x => _winText.text = (x.ToString()), winReward, 0.8f);
        DOTween.To(() => currentMoney, x => _moneyText.text = (x.ToString()), currentMoney + winReward, 0.8f);
    }

    public Firebase.FirebaseApp app = null;
    private async void Start()
    {
        _gameManager.SetView(this);
        InitializeFirebaseAndStartGame();
        _decreaseBet.onClick.AddListener(OnlickDecreaseBet);
        _increaseBet.onClick.AddListener(OnlickIncreaseBet);
        _onClickAddMoney.onClick.AddListener(OnClickAddMoney);
        _onClickInfoPopUp.onClick.AddListener(OnClickInfoPopUp);
        _onCloseInfoPopUp.onClick.AddListener(OnClickOnCloseInfoPopUp);
    }

    private void OnClickOnCloseInfoPopUp()
    {
        _infoPopUp.gameObject.SetActive(false);
    }

    private void OnClickInfoPopUp()
    {
        _infoPopUp.gameObject.SetActive(true);
    }

    private void OnClickAddMoney()
    {
        _gameManager.AddMoney(10000,true);
    }

    private void OnlickIncreaseBet()
    {
        _gameManager.IncreaseDecreaseBet(true);
    }

    private void OnlickDecreaseBet()
    {
        _gameManager.IncreaseDecreaseBet(false);
    }

    private void InitializeFirebaseAndStartGame()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync()
        .ContinueWithOnMainThread(
           previousTask =>
           {
               var dependencyStatus = previousTask.Result;
               if (dependencyStatus == Firebase.DependencyStatus.Available)
               {
                   // Create and hold a reference to your FirebaseApp,
                   app = Firebase.FirebaseApp.DefaultInstance;
                   SetRemoteConfigDefaults();
                   
               }
               else
               {
                   UnityEngine.Debug.LogError(
                 $"Could not resolve all Firebase dependencies: {dependencyStatus}\n" +
                 "Firebase Unity SDK is not safe to use here");
               }
           });
    }

    private void SetRemoteConfigDefaults()
    {
        var defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add(
           "LineCoordinates",
           "Default");
        defaults.Add(
           "SymbolPayoutData",
           "Default");
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(
           previousTask =>
           {
               FetchRemoteConfig(InitializeCommonDataAndStartGame);
           }
        );
    }

    private void InitializeCommonDataAndStartGame()
    {
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        _mainThreadHandler.AddMainThreadAction(delegate {
            var symbolPayoutData = JsonUtility.FromJson<SymbolPayoutData>(remoteConfig.GetValue("SymbolPayoutData").StringValue);
            var lineCoordinatesData = JsonUtility.FromJson<PayoutLineData>(remoteConfig.GetValue("LineCoordinates").StringValue);
            _gameManager.SetRemoteConfigData(symbolPayoutData, lineCoordinatesData);
        });

    }

    public void FetchRemoteConfig(System.Action onFetchAndActivateSuccessful)
    {
        if (app == null)
        {
            Debug.LogError($"Do not use Firebase until it is properly initialized by calling {nameof(InitializeFirebaseAndStartGame)}.");
            return;
        }

        Debug.Log("Fetching data...");
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        remoteConfig.FetchAsync(System.TimeSpan.Zero).ContinueWithOnMainThread(
           previousTask =>
           {
               if (!previousTask.IsCompleted)
               {
                   Debug.LogError($"{nameof(remoteConfig.FetchAsync)} incomplete: Status '{previousTask.Status}'");
                   return;
               }
               ActivateRetrievedRemoteConfigValues(onFetchAndActivateSuccessful);
           });
    }

    private void ActivateRetrievedRemoteConfigValues(System.Action onFetchAndActivateSuccessful)
    {
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus == LastFetchStatus.Success)
        {
            remoteConfig.ActivateAsync().ContinueWithOnMainThread(
               previousTask =>
               {
                   Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                   onFetchAndActivateSuccessful();
               });
        }
    }

    public void ShowBetValue(int newValue)
    {
        DOTween.To(() => _gameManager.GetBetValue(), x => _betValueText.text = (x.ToString()), newValue, 0.8f);
    }
}
