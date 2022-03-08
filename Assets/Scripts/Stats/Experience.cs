using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // is Delegate
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }
        public float GetPoints()
        {
            return experiencePoints;
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}