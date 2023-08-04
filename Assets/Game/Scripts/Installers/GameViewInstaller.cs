using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller<GameViewInstaller>
{
    public override void InstallBindings()
    {
        ModelInstaller.Install(Container);
        //Container.Bind<PopUpView>().FromComponentOn(_popUPView.gameObject).AsCached();
    }
}