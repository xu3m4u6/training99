using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VersusGameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private CreateArea[] createAreas;
    [SerializeField] private VersusBullet clone;
    public GameObject bulletNode;
    public VersusPlayer player1;
    public VersusPlayer player2;
    public PropSelectPanelController propPanel1;
    public PropSelectPanelController propPanel2;
    
    [Header("Game Config")]
    [SerializeField] private Color[] colors = new Color[2];
    public CardController[] AvailableCards;
    public int Hp_max;
    public int Energy_max;
    public float PlayerSpeed;
    public float BulletSpeed;
    
    public static string winner = null;

    [Header("Data Management")]
    [SerializeField] private DataManager dataManager;
    public Dictionary<string, int> PropUsage = new Dictionary<string, int>();
    
    void Start()
    {
        // Init PropUsage dictionary
        foreach (CardController card in AvailableCards)
        {
            PropUsage.Add(card.name, 0);
        }
    }
    
    public void SendForm()
    {
        string prop1 = PropUsage["InvincibleCard"].ToString();
        string prop2 = PropUsage["HealCard"].ToString();
        string prop3 = PropUsage["BulletChangeColorCard"].ToString();
        dataManager.SendVersus(Time.timeSinceLevelLoad.ToString("0.0"), winner, prop1, prop2, prop3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            propPanel1.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            propPanel2.gameObject.SetActive(true);
        }
        
        
        foreach (CreateArea ca in createAreas)
        {
            if (ca.CheckTime())
            {
                Vector3 pos = ca.GetRandomPos();
                // This function makes a copy of an object in a similar way to the Duplicate command in the editor.
                VersusBullet bullet = Instantiate(clone, pos, Quaternion.identity, bulletNode.transform);
                bullet.Color = colors[Random.Range(0, colors.Length)];

                if (player1.Prop && player1.Prop.name == "RandomBulletProp" && bullet.Color == player1.Color)
                {
                    bullet.IsRandomMoving = true;
                }

                if (player2.Prop && player2.Prop.name == "RandomBulletProp" && bullet.Color == player2.Color)
                {
                    bullet.IsRandomMoving = true;
                }      
                
                ca.NextTime();
            }
        }
    }
}
