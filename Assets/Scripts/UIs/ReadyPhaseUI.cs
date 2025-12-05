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
    const int gridSizeX = 4;
    const int gridSizeY = 4;
    int[,] placedMaterials = new int[gridSizeX, gridSizeY];
    int selectedGrid = -1;
    [SerializeField] GameObject placeButton;
    [SerializeField] BlueprintDrawer[] blueprints;

    [Header("Build")]
    [SerializeField] GameObject currentBuild;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] BlueprintDrawer currentBuildBlueprint;

    readonly RewardFormat[] randomReward = new RewardFormat[] { new(RewardType.AttackDamage, 0.2f) };

    public void SetReadyPhase()
    {
        startNextStage.SetActive(false);
        selectedOption = -1;
        selectedGrid = -1;
        rewardOptionsBoxOuter.SetActive(true);
        hideOrShow.SetActive(true);
        currentMaterial.SetActive(false);
        currentBuild.SetActive(false);

        for(int i=0; i< gridSizeY; i++)
        {
            for(int j=0; j< gridSizeX; j++)
            {
                placedMaterialsObject[gridSizeY * i+j].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[placedMaterials[i, j]];
                placedMaterialsObject[gridSizeY * i+j].GetComponent<Button>().interactable = placedMaterials[i, j] == 0;
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
        if(selectedGrid == -1 || placedMaterials[selectedGrid / gridSizeY, selectedGrid % gridSizeY] != 0) return;
        placedMaterials[selectedGrid / gridSizeY, selectedGrid % gridSizeY] = rewardOptions[selectedOption].materialType;
        placedMaterialsObject[selectedGrid].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[rewardOptions[selectedOption].materialType];
        currentMaterial.SetActive(false);
        placeButton.SetActive(false);
        startNextStage.SetActive(true);
        CheckBuildable();
    }

    class Buildable
    {
        public Blueprint blueprint;
        public List<Vector2Int> cordinates;

        public Buildable(Blueprint blueprint, List<Vector2Int> cordinates)
        {
            this.blueprint = blueprint;
            this.cordinates = cordinates;
        }
    }

    List<Buildable> buildables;
    int currentBuildableIndex;
    void CheckBuildable()
    {
        buildables = new();
        Buildable buildable;
        List<Vector2Int> cordinates;
        foreach(Blueprint blueprint in GameManager.Instance.StageManager.bar.blueprints)
        {
            for(int i=0; i<blueprint.blueprint.GetLength(0); i++)
            {
                if (blueprint.blueprint[0, i] == 0) continue;
                for(int y=0; y< gridSizeY; y++)
                {
                    for (int x=0; x< gridSizeX; x++)
                    {
                        if (placedMaterials[y, x] == blueprint.blueprint[0, i])
                        {
                            bool descrimination = true;
                            cordinates = new() { new(y,x) };
                            // 정방향
                            for(int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for(int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    // y, x : 판별 시작 그리드
                                    // row, column : 설계도 상대 그리드
                                    if (row == 0 && column <= i) continue;
                                    if (x + column - i >= gridSizeX || x + column - i < 0 || y + row >= gridSizeY)
                                    {
                                        descrimination = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (placedMaterials[y + row, x + column - i] == blueprint.blueprint[row, column])
                                        {
                                            cordinates.Add(new(y + row, x + column - i));
                                            continue;
                                        }
                                        else
                                        {
                                            descrimination = false;
                                            break;
                                        }
                                    }
                                }
                                if (!descrimination) break;
                            }
                            if(descrimination)
                            {
                                buildable = new(blueprint, cordinates);
                                buildables.Add(buildable);
                            }
                            // 90도 회전
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    if (row == 0 && column <= i) continue;
                                    if (y + column - i >= gridSizeY || y + column - i < 0 || x - row < 0)
                                    {
                                        descrimination = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (placedMaterials[y + column - i, x - row] == blueprint.blueprint[row, column])
                                        {
                                            cordinates.Add(new(y + column - i, x - row));
                                            continue;
                                        }
                                        else
                                        {
                                            descrimination = false;
                                            break;
                                        }
                                    }
                                }
                                if (!descrimination) break;
                            }
                            if (descrimination)
                            {
                                buildable = new(blueprint, cordinates);
                                buildables.Add(buildable);
                            }
                            // 180도
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    // y, x : 판별 시작 그리드
                                    // row, column : 설계도 상대 그리드
                                    if (row == 0 && column <= i) continue;
                                    if (x - column - i >= gridSizeX || x - column - i < 0 || y - row >= 0)
                                    {
                                        descrimination = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (placedMaterials[y - row, x - column - i] == blueprint.blueprint[row, column])
                                        {
                                            cordinates.Add(new(y - row, x - column - i));
                                            continue;
                                        }
                                        else
                                        {
                                            descrimination = false;
                                            break;
                                        }
                                    }
                                }
                                if (!descrimination) break;
                            }
                            if (descrimination)
                            {
                                buildable = new(blueprint, cordinates);
                                buildables.Add(buildable);
                            }
                            // 270도
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    if (row == 0 && column <= i) continue;
                                    if (y - column - i >= gridSizeY || y - column - i < 0 || x + row >= gridSizeX)
                                    {
                                        descrimination = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (placedMaterials[y - column - i, x + row] == blueprint.blueprint[row, column])
                                        {
                                            cordinates.Add(new(y - column - i, x + row));
                                            continue;
                                        }
                                        else
                                        {
                                            descrimination = false;
                                            break;
                                        }
                                    }
                                }
                                if (!descrimination) break;
                            }
                            if (descrimination)
                            {
                                buildable = new(blueprint, cordinates);
                                buildables.Add(buildable);
                            }
                        }
                    }
                }
            }
        }

        currentBuild.SetActive(buildables.Count > 0);
        if(buildables.Count > 0)
        {
            SetCurrentBuildBlueprint(0);
        }
    }

    public void SetCurrentBuildBlueprint(int indexChange)
    {
        if (indexChange == 0) currentBuildableIndex = 0;
        else currentBuildableIndex += indexChange;

        foreach(GameObject grid in placedMaterialsObject)
        {
            grid.GetComponent<Button>().enabled = false;
        }
        currentBuildBlueprint.SetBlueprint(buildables[currentBuildableIndex].blueprint);

        foreach(Vector2Int cordinate in buildables[currentBuildableIndex].cordinates)
        {
            placedMaterialsObject[cordinate.y * gridSizeY + cordinate.x].GetComponent<Button>().interactable = true;
        }
    }

    public void Build()
    {

    }
}
