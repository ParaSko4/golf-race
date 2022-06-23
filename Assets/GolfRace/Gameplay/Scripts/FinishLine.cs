using GolfRace.Gameplay.Car;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GolfRace.Gameplay
{
    public class FinishLine : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> winningEffects;

        private bool alreadyWinning;

        private void Start()
        {
            foreach (var effect in winningEffects)
            {
                effect.Stop();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CarController carController;

            if (other.transform.parent.TryGetComponent(out carController) && alreadyWinning == false)
            {
                alreadyWinning = true;

                foreach (var effect in winningEffects)
                {
                    effect.Play();
                    carController.Stop();
                }
            }
        }


    }
}