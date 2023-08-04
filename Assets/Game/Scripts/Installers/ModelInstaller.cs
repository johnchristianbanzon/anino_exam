using Zenject;

public class ModelInstaller : Installer<ModelInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ISlotManager>().To<SlotManager>().FromNew().AsSingle();
        
    }
}