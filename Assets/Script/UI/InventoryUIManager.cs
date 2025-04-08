using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;
    [Header("Gatos")]
    [SerializeField] private GameObject catIcon;
    [SerializeField] private TMP_Text catCountText;
    private int catCount = 0;
    [Header("Llave")]
    [SerializeField] private GameObject keyIcon;
    [SerializeField] private TMP_Text keyCountText;
    private int keyCount = 0;
    [Header("Curacion")]
    [SerializeField] private GameObject healIcon;
    [Header("Chancla")]
    [SerializeField] private GameObject chanclaIcon;
    [SerializeField] private TMP_Text chanclaCountText;
    public int chanclaCount = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetIventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCat()
    {
        catCount++;
        catIcon.SetActive(true);
        catCountText.text = catCount.ToString();
    }
    public void AddKey()
    {
        keyCount++;
        keyIcon.SetActive(true);
        keyCountText.text = keyCount.ToString();
    }
    public void UseHeal()
    {
        healIcon.SetActive(false);
    }
    public void UseChancla()
    {
        Debug.Log("UseChancla llamado. Cantidad actual: " + chanclaCount);
        if (chanclaCount <= 0) return;
        chanclaCount -= 1;
        chanclaCountText.text = chanclaCount.ToString();
        if (chanclaCount == 0)
        {
            chanclaIcon.SetActive(false);
        }
    }
    public void ResetIventoryUI()
    {
        catCount = 0;
        keyCount = 0;
        chanclaCount = 10;
        catIcon.SetActive(false);
        keyIcon.SetActive(false);
        healIcon.SetActive(true);
        chanclaIcon.SetActive(true);
        catCountText.text = "0";
        keyCountText.text = "0";
        chanclaCountText.text = "10";
    }
}
