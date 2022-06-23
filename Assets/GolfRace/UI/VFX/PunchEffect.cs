using DG.Tweening;
using UnityEngine;

namespace GolfRace.UI.VFX
{
    public class PunchEffect : MonoBehaviour
    {
        public static void Punch(Transform transform, float scale, float duration)
        {
            transform.DORewind();
            transform.DOPunchScale(new Vector3(scale, scale, scale), duration)
                .SetEase(Ease.OutExpo);
        }
    }
}