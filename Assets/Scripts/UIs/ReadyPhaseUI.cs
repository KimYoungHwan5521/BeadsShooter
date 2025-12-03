using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPhaseUI : MonoBehaviour
{
    [SerializeField] GameObject startNextStage;

    [Header("Select Options")]
    [SerializeField] GameObject rewardOptionsBoxOuter;
    [SerializeField] GameObject rewardOptionsBox;
    [SerializeField] GameObject hideOrShow;
    [SerializeField] RewardOption[] rewardOptions;
    int selectedOption = -1;

    [Header("Place Material")]
    [SerializeField] GameObject currentMaterial;
    [SerializeField] GameObject[] placedMaterialsObject;
    int[,] placedMaterials = new int[4,4];
    int selectedGrid = -1;
    [SerializeField] GameObject placeButton;
    [SerializeField] BlueprintDrawer[] blueprints;

    readonly RewardFormat[] randomReward = new RewardFormat[] { new(RewardType.AttackDamage, 0.2f) };

    public void SetReadyPhase()
    {
        startNextStage.SetActive(false);
        selectedOption = -1;
        selectedGrid = -1;
        rewardOptionsBoxOuter.SetActive(true);
        hideOrShow.SetActive(true);
        currentMaterial.SetActive(false);

        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                placedMaterialsObject[4*i+j].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[placedMaterials[i, j]];
            }
        }
        placeButton.SetActive(false);

        List<Blueprint> blueprintsData = GameManager.Instance.StageManager.bar.blueprints;
        for (int i = 0; i<blueprints.Length; i++)
        {
            if (i < blueprintsData.Count)
            {
                blueprints[i].gameObject.SetActive(true);
                blueprints[i].SetBlueprint(blueprintsData[i]);
            }
            else
            {
                blueprints[i].gameObject.SetActive(false);
            }
        }
        SetOptions();
    }
    
    public void HideOrShowRewardOptions()
    {
        rewardOptionsBox.SetActive(!rewardOptionsBox.activeSelf);
    }

    void SetOptions()
    {
        foreach(RewardOption rewardOption in rewardOptions)
        {
            rewardOption.SetOption("", Random.Range(1, 6), randomReward[Random.Range(0, randomReward.Length)]);
        }
    }

    public void FocusOption(int index)
    {
        selectedOption = index;
    }

    public void SelectOption()
    {
        if (selectedOption == -1) return;
        rewardOptionsBoxOuter.SetActive(false);
        hideOrShow.SetActive(true);
        currentMaterial.SetActive(true);
        currentMaterial.GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[rewardOptions[selectedOption].materialType];
        placeButton.SetActive(true);
    }

    public void FocusGrid(int index)
    {
        selectedGrid = index;
    }

    public void Place()
    {
        if(selectedGrid == -1 || placedMaterials[selectedGrid / 4, selectedGrid % 4] != 0) return;
        placedMaterials[selectedGrid / 4, selectedGrid % 4] = rewardOptions[selectedOption].materialType;
        placedMaterialsObject[selectedGrid].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[rewardOptions[selectedOption].materialType];
        currentMaterial.SetActive(false);
        placeButton.SetActive(false);
        startNextStage.SetActive(true);
    }
}
