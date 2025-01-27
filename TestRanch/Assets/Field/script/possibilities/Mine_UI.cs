using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine_UI : MonoBehaviour
{
    //garde les fonctions appeler par field_ui qui activent les upgrades specifiques
    //some stuff to keep track of upgrades and whatnot
    private Mine mine_;

    //if true the upgrade linked to thoses bools werent able to be done because the spawner was null
    private bool soil;
    private bool chrono;
    private bool rarerock;
    private bool stalactite;


    public void Set_ref(Mine minecraft)
    {
        this.mine_ = minecraft;
    }

    public void CheckPendingUpgrades()
    {
        Debug.Log("Checking pending upgrades");

        if (soil)
        {//il y a deja le check pour si cest null dans la fnct
            Rich_soil_Activate();
        }

        if (chrono)
        {//il y a deja le check pour si cest null dans la fnct
            Chrono_Activate();
        }

        if (rarerock)
        {//il y a deja le check pour si cest null dans la fnct
            Rare_rock_Activate();
        }

        if (stalactite) {
            Stalactite_Activate();
        }
    }

    internal void Delete_pending_upgrades()
    {
        soil = false;
        chrono =  false;
        rarerock = false;
        stalactite = false;
    }


    #region Upgrades
    //les functions se font appeler par field_ui
    //si les fonctions se font appeler cets que le joueur a acheter l'upgrade
    //le check pour le cout est dans field_ui

    public void Stalactite_Activate()
    {//cr�e un plafond 
     //double spawner
     //le second spawner ne peut pas recevoir lupgrade rich soil sinon cest ben trop peter les ressources obtenut
        mine_.Upgrades[1].SetActive(true);

        if (mine_.SpawnerInstance != null)
        {
            Debug.Log("stalactite Upgrade");

            mine_.OnStalactiteUpgrade();
            stalactite = false;
        }
        else {
            stalactite = true;
        }

    }

    public void Rich_soil_Activate()
    {//augmente qte de ressources re�u

        mine_.Upgrades[3].SetActive(true);

        if (mine_.SpawnerInstance != null)
        {
            Debug.Log("rich soil Upgrade");

            mine_.SpawnerInstance.GetComponent<SpawnerMinerals>().OnUpgradeSoil();

            soil = false;
        }
        else
            soil = true;

    }

    public void Chrono_Activate()//chrono system
    {//augmente vitesse
        mine_.Upgrades[0].SetActive(true);

        if (mine_.SpawnerInstance != null)
        {
            Debug.Log("Chrono Upgrade");
            mine_.SpawnerInstance.GetComponent<SpawnerMinerals>().OnChronoUpgrade();

            chrono = false;
        }
        else
            chrono = true;
       

    }

    public void Rare_rock_Activate()
    {//augmente la chance d'avoir un minerai rare
        mine_.Upgrades[2].SetActive(true);

        if (mine_.SpawnerInstance != null)
        {        
            Debug.Log("Rare rock Upgrade");

            mine_.SpawnerInstance.GetComponent<SpawnerMinerals>().OnUpgradeRR();

            rarerock = false;
        }
        else
            rarerock = true;
 

    }
    #endregion


}

