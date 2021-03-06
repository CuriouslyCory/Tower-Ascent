using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using CodeMonkey.Utils;
using TMPro;

public class TavernActions : MonoBehaviour
{
    private GameState gameState;

    [SerializeField]
    private TextMeshProUGUI goldText;
    
    [SerializeField]
    private GameObject pfUpgradeButton;

    [SerializeField]
    private GameObject buyItemContainer;
    [SerializeField]
    private GameObject sellItemContainer;
    [SerializeField]
    private GameObject upgradeStatsContainer;

    [SerializeField]
    private GameObject pfStatText;

    [SerializeField]
    private Transform statsPanel;

    [SerializeField]
    private TextMeshProUGUI maxHealthText;

    private List<GameObject> buttonObjs;
    private List<GameObject> statTexts;

    [SerializeField]
    private GameObject inventoryContainer;

    [SerializeField]
    private GameObject characterStatsContainer;
    


    private void Start() 
    {
        buttonObjs = new List<GameObject>();
        statTexts = new List<GameObject>();
        gameState = GameAssets.i.gameState;

        if(gameState.stats == null || gameState.stats.Count == 0){
            gameState.Initialize();
        }
        
        UpdateInventoryUIComponents();
        
        gameState.inventory.OnItemListChanged += Inventory_OnItemListChanged;
        gameState.inventory.OnGoldChanged += State_OnGoldChanged;

        GenerateUpgradeMenu();
        maxHealthText.text = "Max HP: " + gameState.playerMaxHealth.ToString();
        
    }
    private void OnDestroy() {
        gameState.inventory.OnItemListChanged -= Inventory_OnItemListChanged;
        gameState.inventory.OnGoldChanged -= State_OnGoldChanged;
    }

    private void GenerateUpgradeMenu()
    {
        foreach(UpgradeableStat stat in gameState.stats.Values)
        {
            CreateUpgradeButton(stat);
            CreateStatText(stat);
        }
    }

    private void CreateUpgradeButton(UpgradeableStat stat)
    {
        buttonObjs.Add(Instantiate(pfUpgradeButton, upgradeStatsContainer.transform));
        buttonObjs[buttonObjs.Count - 1].GetComponent<UpgradeButton>().SetStat(stat);
        
        buttonObjs[buttonObjs.Count - 1].GetComponent<Button_UI>().ClickFunc = () => {
            OnUpgradeClick(stat);
        };
    }

    private void CreateStatText(UpgradeableStat stat)
    {
        statTexts.Add(Instantiate(pfStatText, statsPanel));
        statTexts[statTexts.Count - 1].GetComponent<StatText>().SetStat(stat);
    }

    public void OnUpgradeClick(UpgradeableStat stat)
    {
        int statLevel = stat.statLevel;
        int statCost = stat.price[statLevel - 1];
        if(gameState.inventory.gold > statCost){
            stat.statLevel++;
            gameState.inventory.gold -= statCost;
        }
        if(stat.statType == UpgradeableStat.StatType.Constitution){
            gameState.playerMaxHealth = stat.statLevel * 10;
            maxHealthText.text = "Max HP: " + gameState.playerMaxHealth.ToString();
        }
    }

    

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryUIComponents();
    }

    private void State_OnGoldChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryUIComponents();
    }

    private void UpdateInventoryUIComponents()
    {
        goldText.text = gameState.inventory.gold.ToString() + "G";

    }

    // Start is called before the first frame update
    public void OnClickSmashTower()
    {
        SceneManager.LoadScene("Tower", LoadSceneMode.Single);
    }

    public void OnClickBuyItemsButton()
    {
        sellItemContainer.SetActive(false);
        upgradeStatsContainer.SetActive(false);
        buyItemContainer.SetActive(true);
    }

    public void OnClickSellItemsButton()
    {
        sellItemContainer.SetActive(true);
        upgradeStatsContainer.SetActive(false);
        buyItemContainer.SetActive(false);
    }

    public void OnClickUpgradeStatsButton()
    {
        sellItemContainer.SetActive(false);
        upgradeStatsContainer.SetActive(true);
        buyItemContainer.SetActive(false);
    }

    public void OnClickStatsButton()
    {
        characterStatsContainer.SetActive(true);
        inventoryContainer.SetActive(false);
    }

    public void OnClickInventoryButton()
    {
        characterStatsContainer.SetActive(false);
        inventoryContainer.SetActive(true);
    }
} 