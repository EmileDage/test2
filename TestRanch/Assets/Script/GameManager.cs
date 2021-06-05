using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;
    [SerializeField] private Player joueur;
    [SerializeField] private Item emptyItem;
    [HideInInspector]public ItemStack emptyItemItemStack;
    [SerializeField] private int chronoCoin = 0;
    public Player Joueur { get => joueur;}

    void Awake()
    {
        if (gmInstance != null && gmInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gmInstance = this;
        }
        emptyItemItemStack = new ItemStack(emptyItem, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyChronoCoin(int value, bool RemoveValue = false)
    {
        if (RemoveValue)
            chronoCoin -= value;
        else
            chronoCoin += value;
    }

    public int GetChronoCoin()
    {
        return chronoCoin;
    }

}