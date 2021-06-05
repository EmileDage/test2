using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerAgriculture : AbstractSpawner, IFarmable
{
    //very important :)
    //https://twitter.com/RabbitEveryHour/status/1397930644143960066?s=20

    //cycle grandissement
    //doit etre arroser pour grandir donc si hydratation trop low pas de grandissement
    private int timeToGrowHour;//combien d'heures avant que la plante soit pr�te a fleurir
    private bool IsGrown;

    //hydration
    //le jour doit au moins arrose la plantation adulte au moins une fois apr recolte
    //sinon moins de ressources produites
    //+ de chance de maladie
    [SerializeField] [Range(0, 100)] private int hydration;//en %
    [SerializeField] [Range(5, 50)] private int hydration_hour;//for baby and adult plant
    [SerializeField] private Abreuvoir water_container;

    //Sante/maladie
    //en ce moment la maladie arrete la production de produit on pourra modifier �a plus tard
    [SerializeField] [Range(0, 100)] private int health;//en %
    [SerializeField] [Range(0, 100)] private int sickness_resistance;//en %
    private int sicknessLvl;//en % si 0 pas sick

    //je prefere creer un autre array pour q'on puisse changer plus facilement les upgrades si on chnage d'id�e pour le nombre
    //ou si chaque ressources a une upgrade diff�rente
    [SerializeField] private bool upgrade_fertilizer;//rich ferterlizer
    [SerializeField] private Transform[] upgrade_slot;//si l'upgrade est acheter l'arbre donne plus de ressources
    private GameObject[] upgrade_produit;

    //cest pour le pannel info
    private Text text;

    public bool Upgrade { get => upgrade_fertilizer; set => upgrade_fertilizer = value; }//rich fertilizer upgrade 

    protected override void Start()
    {
        sicknessLvl = 0;
        base.Start();
        upgrade_produit = new GameObject[upgrade_slot.Length];

        if (timeToGrowHour <= 0) {
            timeToGrowHour = 10;
        }

    }

    public void CrystalCure(int strenght) {
        //utlise un crystal pour baisser la maladie
        sicknessLvl -= strenght;

    }

    public void GrowthCheck() {//verifie que la plante peut grandir
        if (sicknessLvl < sickness_resistance)
        { //check si plante est PAS malade
            if (water_container.Qte_level >= hydration_hour)//est ce que la plante est assez hydrate
            {
                water_container.RemoveWater(hydration_hour);
                timeToGrowHour--;
                sicknessLvl -= 5;

                if (timeToGrowHour <= 0)
                {
                    IsGrown = true;
                }
            }
            else
            {
                Debug.Log("The plant isnt watered so it wont grow");
                sicknessLvl += 10;
            }
        }
        else {
            Debug.Log("Your growing plant is sick it will wilt !");
            timeToGrowHour++;
            sicknessLvl += 5;
        }
       
    }

  

    public void OnCrystalUpgrade()
    {
        sickness_resistance += 30;
    }

    public override void OnGHourPassed(object source)
    {
        if (sickness_resistance < sicknessLvl)
        { //si la plante est malade
            sicknessLvl += 5;

            if (sicknessLvl >=  100) {
                DestroyAll();
            }
        }
        else {
            if (!IsGrown)
            {
                GrowthCheck();
            }
            else
            {
                //produce if time is correct
                if (disponibleStart == time.Hour)
                {
                    if (water_container.Qte_level >= hydration_hour)
                    {
                        //si la plante est hydrat�e elle produit des produits

                        water_container.RemoveWater(hydration_hour);
                        Spawn();

                    }
                    else
                    {
                        //la plante est pas hydrate so no produit for u
                        Despawn();
                        sicknessLvl += 5;
                    }

                }
                else if (disponibleEnd == time.Hour)
                {
                    Despawn();
                }
            }
        }
       
    }

    private void Spawn() {//pour faciliter la lecture du code jai mis �a en fonction
        foreach (GameObject produit in produits)
        {
            if (produit.GetComponent<RessourceNode>().GetSpawned()) //on ne veut pas activer le node si il n'a pas eu le temps de respawn
            {//note la ressourceNode.GetSpawned ne va jamais retourne vrai si le node est mort
                produit.SetActive(true);
            }
        }

        if (upgrade_fertilizer)
        {
            foreach (GameObject produit in upgrade_produit)
            {
                if (produit.GetComponent<RessourceNode>().GetSpawned())
                {
                    produit.SetActive(true);
                }
            }
        }
    }

    private void Despawn()
    {
        foreach (GameObject produit in produits)
        {
            produit.SetActive(false);
        }

        if (upgrade_fertilizer)
        {
            foreach (GameObject produit in upgrade_produit)
            {
                if (produit.GetComponent<RessourceNode>().GetSpawned())
                {
                    produit.SetActive(false);
                }
            }
        }
    }

    public override void SpawnProduce()
    {
        for (int a = 0; a < produit_spawn.Length; a++)
        {
            if (produits[a] == null)
            {
                produits[a].GetComponent<RessourceNode>().SetSpawnedTrue();
            }

            if (upgrade_fertilizer) {
                if (upgrade_produit[a] == null)
                {
                    upgrade_produit[a].GetComponent<RessourceNode>().SetSpawnedTrue();
                }
            }
        }
    }

    public override void DestroyAll()
    {
        for (int a = 0; a < produits.Length; a++)
        {
            produits[a].GetComponent<RessourceNode>().KillNode();
        }

        if (upgrade_fertilizer) {
            for (int a = 0; a < upgrade_produit.Length; a++)
            {
                upgrade_produit[a].GetComponent<RessourceNode>().KillNode();
            }
        }

        Debug.Log("All products on this spawner are destroyed");
    }

    public Text InfoPannelAgriculture() {

        text.text = "Hello world"; 

        return text;
    }

    public override void SpawnSpawner(Materiaux toSpawn)
    {
        if (toSpawn.Funct.Equals(Fonctions.plantes))
        {
            base.SpawnSpawner(toSpawn);
        }
    }

    public void FarmIt()
    {
        //  x////x

    }
}