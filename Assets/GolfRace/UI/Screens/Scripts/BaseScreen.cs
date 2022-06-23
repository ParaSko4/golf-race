using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfRace.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseScreen : MonoBehaviour
    {
        [SerializeField]
        private float fadeDuration;

        private CanvasGroup fade;

        private void Awake()
        {
            fade = GetComponent<CanvasGroup>();

            OnAwake();
        }

        protected virtual void OnAwake() { }

        public async Task Show()
        {
            fade.blocksRaycasts = false;

            await fade.DOFade(1f, fadeDuration).AsyncWaitForCompletion();

            fade.blocksRaycasts = true;
        }

        public async Task Hide()
        {
            fade.blocksRaycasts = true;

            await fade.DOFade(0f, fadeDuration).AsyncWaitForCompletion();

            fade.blocksRaycasts = false;
        }
    }
}