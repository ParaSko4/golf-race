namespace GolfRace.Gameplay.Wallet
{
    public class WalletManager
    {
        private WalletModel userWallet;
        private WalletModel timeWallet;

        public WalletModel UserWallet { get => userWallet; }
        public WalletModel TimeWallet { get => timeWallet; }

        public WalletManager()
        {
            userWallet = new WalletModel();
            timeWallet = new WalletModel();
        }

        public void CommitTimeWallet()
        {
            userWallet.SetCoins(timeWallet.Coins);
        }

        public void ResetTimeWallet()
        {
            timeWallet.ResetCoins();
        }
    }
}
