using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace IsaacFagg.UI.Main
{
    public class TrackBuilding : MonoBehaviour
    {
        //States
        public enum BuildingState { Building, Completed};
        private BuildingState state = BuildingState.Building;
        public BuildingState State
        {
            get
            {
                return state;
            }
            set
            {
                ChangeState(value);
            }
        }

        //State GameObjects
        public GameObject buildGO;
        public GameObject completeGO;

        public Slider progressBar;
        public Text progressText;
        private int progress = 0;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                UpdateProgress(value);
            }
        }

        private void Start()
        {
            //Get Saved data
            State = BuildingState.Completed;
        }

        #region State Change

        private void ChangeState(BuildingState newState)
        {
            if (newState == BuildingState.Building)
            {
                ShowBuilding();
            }
            else
            {
                ShowCompleted();
            }
            state = newState;
        }

        private void ShowBuilding()
        {
            buildGO.SetActive(true);
            completeGO.SetActive(false);
        }

        private void ShowCompleted()
        {
            buildGO.SetActive(false);
            completeGO.SetActive(true);
        }
        #endregion


        #region Building

        private void UpdateProgress(int newValue)
        {
            progress = Mathf.Clamp(newValue, 0, 100);

            if (progress == 100)
            {
                ChangeState(BuildingState.Completed);
                progress = 0;
            }
            else
            {
                progressBar.value = progress;
                progressText.text = progress.ToString();
            }
        }

        private void FinishTrackBuilding()
        {
            ChangeState(BuildingState.Completed);
        }

        #endregion

        #region Completed

        public void StartNewBuild()
        {
            ChangeState(BuildingState.Building);
            Progress = 0;
        }


        #endregion




    }

}
