using GolfRace.Gameplay.Wallet;
using Zenject;

public class ManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WalletManager>().FromInstance(new WalletManager()).AsSingle();
    }
}