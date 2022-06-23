using DG.Tweening;
using GolfRace.Gameplay.Wallet;
using GolfRace.UI.VFX;
using TMPro;
using UnityEngine;
using Zenject;

namespace GolfRace.UI.Screens.Status.CoinCounter
{
    public class CoinPresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI coinPresenterText;
        [Space, SerializeField]
        private Transform coinImageTransform;
        [SerializeField]
        private int coinPunchLevel;
        [SerializeField]
        private float coinImagePunchScale;
        [SerializeField]
        private float coinImagePunchDuration;
        [Space, SerializeField]
        private float coinCounterDuration;

        private WalletManager walletManager;
        private Tween coinCounterTween;
        private int cointPunchCounter;
        private int currentCoinsOnScreen;

        [Inject]
        public void Consturct(WalletManager walletManager)
        {
            this.walletManager = walletManager;
        }

        private void OnEnable()
        {
            walletManager.TimeWallet.CoinsChange += OnCoinsChange;
        }

        private void OnDisable()
        {
            walletManager.TimeWallet.CoinsChange -= OnCoinsChange;
        }

        private void OnCoinsChange(int coins)
        {
            coinCounterTween?.Kill();

            coinCounterTween = DOVirtual.Int(currentCoinsOnScreen, coins, coinCounterDuration, (value) =>
            {
                currentCoinsOnScreen = value;
                coinPresenterText.text = currentCoinsOnScreen.ToString();

                if (coins > coinPunchLevel * cointPunchCounter)
                {
                    cointPunchCounter++;
                    PunchEffect.Punch(coinImageTransform, coinImagePunchScale, coinImagePunchDuration);
                }
            });
        }
    }
}