using System;

namespace GolfRace.Gameplay.Wallet
{
    public class WalletModel
    {
        public event Action<int> CoinsChange;

        public int Coins { get; private set; }

        public WalletModel() { }

        public WalletModel(int coins)
        {
            Coins = coins;
        }

        public void SetCoins(int coins)
        {
            ChangeCoins(coins);
        }

        public void AddCoins(int coins)
        {
            ChangeCoins(Coins + coins);
        }

        public void RemoveCoins(int coins)
        {
            ChangeCoins(Coins - coins);
        }

        public void ResetCoins()
        {
            ChangeCoins(0);
        }

        private void ChangeCoins(int coins)
        {
            Coins = coins;

            CoinsChange?.Invoke(Coins);
        }
    }
}
