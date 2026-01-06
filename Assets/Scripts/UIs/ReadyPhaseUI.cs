using System;
using System.Collections.Generic;
using TMPro;
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
    int currentMaterialInt;
    RewardFormat currentMaterialReward;
    [SerializeField] GameObject currentMaterial;
    const int maxStoreCapacity = 3;
    int storeCapacity;
    public int StoreCapacity
    {
        get => storeCapacity;
        set
        {
            storeCapacity = Math.Clamp(value, 0, maxStoreCapacity);
        }
    }
    [SerializeField] GameObject storeButton;
    [SerializeField] GameObject[] storedMaterials;
    int[] storedMaterialsInfo = new int[maxStoreCapacity];
    [SerializeField] GameObject[] placedMaterialsObject;
    [SerializeField] int[,] placedMaterials = new int[Blueprint.ColumnCount, Blueprint.RowCount];
    int selectedGrid = -1;
    [SerializeField] GameObject placeButton;
    [SerializeField] GameObject discardButton;
    [SerializeField] BlueprintDrawer[] blueprints;

    [Header("Build")]
    [SerializeField] GameObject currentBuild;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] BlueprintDrawer currentBuildBlueprint;

    readonly RewardFormat[] randomReward = new RewardFormat[] { new(RewardType.AttackDamage, 0.2f) };
    bool isShop;
    bool isDisplace;
    public bool IsShop => isShop;

    public void SetReadyPhase()
    {
        startNextStage.SetActive(false);
        selectedOption = -1;
        selectedGrid = -1;
        rewardOptionsBoxOuter.SetActive(true);
        hideOrShow.SetActive(true);
        currentMaterial.SetActive(false);
        storeButton.SetActive(false);
        for (int i = 0; i < storedMaterials.Length; i++)
        {
            storedMaterials[i].SetActive(i < StoreCapacity);
            storedMaterials[i].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[storedMaterialsInfo[i]];
        }
        currentBuild.SetActive(false);

        for(int i=0; i< Blueprint.RowCount; i++)
        {
            for(int j=0; j< Blueprint.ColumnCount; j++)
            {
                if (placedMaterials[i, j] == -1) placedMaterialsObject[Blueprint.RowCount * i + j].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[0];
                else placedMaterialsObject[Blueprint.RowCount * i + j].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[placedMaterials[i, j]];
                placedMaterialsObject[Blueprint.RowCount * i+j].GetComponent<Button>().interactable = placedMaterials[i, j] == 0;
            }
        }
        placeButton.SetActive(false);
        discardButton.SetActive(false);

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

    public void StartNextStage()
    {
        if(isShop)
        {
            GameManager.Instance.readyPhaseWindow.SetActive(false);
        }
        else
        {
            GameManager.Instance.StartBattlePhase();
        }
    }
    
    public void HideOrShowRewardOptions()
    {
        rewardOptionsBox.SetActive(!rewardOptionsBox.activeSelf);
    }

    void SetOptions()
    {
        foreach(RewardOption rewardOption in rewardOptions)
        {
            rewardOption.SetOption("", UnityEngine.Random.Range(1, 5), randomReward[UnityEngine.Random.Range(0, randomReward.Length)]);
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
        store = true;
        storeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Store";
        storeButton.SetActive(true);
        currentMaterialInt = rewardOptions[selectedOption].materialType;
        currentMaterial.SetActive(true);
        currentMaterial.GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
        placeButton.SetActive(true);
        placeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Place";
        discardButton.SetActive(true);
        isShop = false;
        isDisplace = false;
    }

    public void SetPurchasedMaterial(ShopManager.MerchandiseInfo merchandise)
    {
        startNextStage.SetActive(false);
        rewardOptionsBoxOuter.SetActive(false);
        store = true;
        storeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Store";
        storeButton.SetActive(true);
        for (int i = 0; i < storedMaterials.Length; i++)
        {
            storedMaterials[i].SetActive(i < StoreCapacity);
            storedMaterials[i].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[storedMaterialsInfo[i]];
        }
        currentMaterialInt = merchandise.materialType;
        currentMaterialReward = merchandise.reward;
        currentMaterial.SetActive(true);
        currentMaterial.GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
        placeButton.SetActive(true);
        placeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Place";
        discardButton.SetActive(true);
        isShop = true;
        isDisplace = false;
    }

    public void SetDisplace()
    {
        startNextStage.SetActive(true);
        rewardOptionsBoxOuter.SetActive(false);
        currentMaterial.SetActive(false);
        placeButton.SetActive(true);
        placeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Displace";
        discardButton.SetActive(false);
        isShop = false;
        isDisplace = true;
    }

    public void FocusGrid(int index)
    {
        selectedGrid = index;
    }

    public void Place()
    {
        if(selectedGrid == -1) return;
        if (isDisplace)
        {
            if(placedMaterials[selectedGrid / Blueprint.RowCount, selectedGrid % Blueprint.RowCount] != 0)
            {
                placedMaterials[selectedGrid / Blueprint.RowCount, selectedGrid % Blueprint.RowCount] = 0;
                placedMaterialsObject[selectedGrid].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[0];
                storeButton.SetActive(storedMaterialsInfo[0] != 0);
                storeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Take Out";
                placeButton.SetActive(false);
                startNextStage.SetActive(true);
                startNextStage.GetComponentInChildren<TextMeshProUGUI>().text = isShop ? "Return to shop" : "Start next stage";
            }
        }
        else
        {
            if (placedMaterials[selectedGrid / Blueprint.RowCount, selectedGrid % Blueprint.RowCount] == 0)
            {
                placedMaterials[selectedGrid / Blueprint.RowCount, selectedGrid % Blueprint.RowCount] = currentMaterialInt;
                placedMaterialsObject[selectedGrid].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
                currentMaterialInt = 0;
                currentMaterial.SetActive(false);
                storeButton.SetActive(storedMaterialsInfo[0] != 0);
                storeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Take Out";
                placeButton.SetActive(false);
                discardButton.SetActive(false);
                startNextStage.SetActive(true);
                startNextStage.GetComponentInChildren<TextMeshProUGUI>().text = isShop ? "Return to shop" : "Start next stage";
                if(currentMaterialReward != null) ApplyMaterialStat(currentMaterialReward);
                CheckBuildable();
            }
        }
    }

    public void Discard()
    {
        currentMaterial.SetActive(false);
        storeButton.SetActive(false);
        placeButton.SetActive(false);
        discardButton.SetActive(false);
        startNextStage.SetActive(true);
    }

    bool store;
    public void Store()
    {
        if(store)
        {
            bool noStorageSpace = true;
            for(int i = 0; i < storeCapacity; i++)
            {
                if (storedMaterialsInfo[i] == 0)
                {
                    storedMaterialsInfo[i] = currentMaterialInt;
                    storedMaterials[i].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
                    noStorageSpace = false;
                    break;
                }
            }
            if(noStorageSpace)
            {
                int temp = storedMaterialsInfo[storeCapacity - 1];
                storedMaterialsInfo[storeCapacity - 1] = currentMaterialInt;
                storedMaterials[storeCapacity - 1].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
                currentMaterialInt = temp;
                currentMaterial.GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[temp];
            }
            currentMaterial.SetActive(noStorageSpace);
            store = noStorageSpace;
            storeButton.GetComponentInChildren<TextMeshProUGUI>().text = noStorageSpace ? "Store" : "Take Out";
            placeButton.SetActive(noStorageSpace);
            discardButton.SetActive(noStorageSpace);
            startNextStage.SetActive(!noStorageSpace);
        }
        else
        {
            currentMaterialInt = storedMaterialsInfo[0];
            currentMaterial.SetActive(true);
            currentMaterial.GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[currentMaterialInt];
            store = true;
            storeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Store";
            placeButton.SetActive(true);
            discardButton.SetActive(true);
            for(int i=0; i<StoreCapacity; i++)
            {
                storedMaterials[i].GetComponentsInChildren<Image>()[1].color = i == StoreCapacity - 1 ? MaterialsColor.colors[0] : MaterialsColor.colors[storedMaterialsInfo[i + 1]];
                storedMaterialsInfo[i] = i == StoreCapacity - 1 ? 0 : storedMaterialsInfo[i + 1];
            }
            startNextStage.SetActive(false);
        }
    }

    void ApplyMaterialStat(RewardFormat reward)
    {
        switch(reward.rewardType)
        {
            case RewardType.AttackDamage:
                return;
            default:
                return;
        }
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
            bool check = false;
            for(int i=0; i<blueprint.blueprint.GetLength(0); i++)
            {
                if (check) break;
                if (blueprint.blueprint[0, i] == 0) continue;
                check = true;
                for(int y=0; y< Blueprint.RowCount; y++)
                {
                    for (int x=0; x< Blueprint.ColumnCount; x++)
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
                                    if (row == 0 && column <= i || blueprint.blueprint[row, column] == 0) continue;
                                    if (x + column - i >= Blueprint.ColumnCount || x + column - i < 0 || y + row >= Blueprint.RowCount)
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
                            descrimination = true;
                            cordinates = new() { new(y, x) };
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    if (row == 0 && column <= i || blueprint.blueprint[row, column] == 0) continue;
                                    if (y + column - i >= Blueprint.RowCount || y + column - i < 0 || x - row < 0)
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
                            descrimination = true;
                            cordinates = new() { new(y, x) };
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    // y, x : 판별 시작 그리드
                                    // row, column : 설계도 상대 그리드
                                    if (row == 0 && column <= i || blueprint.blueprint[row, column] == 0) continue;
                                    if (x - column - i >= Blueprint.ColumnCount || x - column - i < 0 || y - row < 0)
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
                            descrimination = true;
                            cordinates = new() { new(y, x) };
                            for (int row = 0; row < blueprint.blueprint.GetLength(0); row++)
                            {
                                for (int column = 0; column < blueprint.blueprint.GetLength(1); column++)
                                {
                                    if (row == 0 && column <= i || blueprint.blueprint[row, column] == 0) continue;
                                    if (y - column - i >= Blueprint.RowCount || y - column - i < 0 || x + row >= Blueprint.ColumnCount)
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
        else
        {
            foreach (GameObject grid in placedMaterialsObject)
            {
                grid.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void SetCurrentBuildBlueprint(int indexChange)
    {
        if (indexChange == 0) currentBuildableIndex = 0;
        else currentBuildableIndex += indexChange;

        foreach(GameObject grid in placedMaterialsObject)
        {
            grid.GetComponent<Button>().interactable = false;
        }
        currentBuildBlueprint.SetBlueprint(buildables[currentBuildableIndex].blueprint);

        foreach(Vector2Int cordinate in buildables[currentBuildableIndex].cordinates)
        {
            // 좌표를 (y, x)로 썼는데 Vecter2Int는 호출할 떄 (x, y)로 저장되어있어서 반대로
            placedMaterialsObject[cordinate.x * Blueprint.RowCount + cordinate.y].GetComponent<Button>().interactable = true;
        }

        leftArrow.interactable = currentBuildableIndex != 0;
        rightArrow.interactable = buildables.Count > 0 && currentBuildableIndex != buildables.Count - 1;
    }

    public void Build()
    {
        Vector2Int check = new(selectedGrid / Blueprint.ColumnCount, selectedGrid % Blueprint.ColumnCount);
        int index = buildables[currentBuildableIndex].cordinates.FindIndex(x => x == check);
        if (index < 0) return;
        for(int i = 0; i < buildables[currentBuildableIndex].cordinates.Count; i++)
        {
            if(i == index)
            {
                placedMaterialsObject[buildables[currentBuildableIndex].cordinates[i].x * Blueprint.ColumnCount + buildables[currentBuildableIndex].cordinates[i].y].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[7];
                placedMaterials[buildables[currentBuildableIndex].cordinates[i].x, buildables[currentBuildableIndex].cordinates[i].y] = -1;
            }
            else
            {
                placedMaterialsObject[buildables[currentBuildableIndex].cordinates[i].x * Blueprint.ColumnCount + buildables[currentBuildableIndex].cordinates[i].y].GetComponentsInChildren<Image>()[1].color = MaterialsColor.colors[0];
                placedMaterials[buildables[currentBuildableIndex].cordinates[i].x, buildables[currentBuildableIndex].cordinates[i].y] = 0;
            }
        }
        ApplyMaterialStat(buildables[currentBuildableIndex].blueprint.reward);
        CheckBuildable();
    }
}
