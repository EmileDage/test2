using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Ce script g�re tous les pannels relies a field et ces upgrades
//ce script contient les fonctions appeler par les bouttons des pannels
//WIP: Il check si tu peux acheter une upgarde

public class Field_UI : MonoBehaviour
{
    [SerializeField] private Field field_ref;//Le field qui se fait modifier

    private GameManager gm;
    private Player joueur;
    private GameObject cam_joueur;


    //pannels with upgrade options
   [SerializeField] private GameObject pannel_empty;
    [SerializeField] private GameObject pannel_agriculture;
    [SerializeField] private GameObject pannel_enclos;
    [SerializeField] private GameObject pannel_entrepot;
    [SerializeField] private GameObject pannel_mine;
        //Other pannels
    [SerializeField] private GameObject pannel_delete_plot;
    [SerializeField] private GameObject pannel_not_enough;
    [SerializeField] private GameObject pannel_too_much_Animal;

    //button U who need to be pressed in your area
    //button list to disable them when upgrade is purchased
    //separer en plusieurs sous categories pour clart�
    //je peux pas faire on click car si le joueur na pas assez de ressource ben le boutton nest plus jamais dispo
    //up = upgrade
    [SerializeField] private Button[] btn_up_empty;
    [SerializeField] private Button[] btn_up_terre;
    [SerializeField] private Button[] btn_up_mine;
    [SerializeField] private Button[] btn_up_enclos;
    [SerializeField] private Button[] btn_up_entrepot;

    private bool test;

    private void Start()
    {
        //s'assurer que tout est inactif au debut
        pannel_empty.SetActive(false);
        pannel_agriculture.SetActive(false);
        pannel_enclos.SetActive(false);
        pannel_entrepot.SetActive(false);
        pannel_mine.SetActive(false);

        pannel_delete_plot.SetActive(false);
        pannel_not_enough.SetActive(false);
        pannel_too_much_Animal.SetActive(false);

        gm = GameManager.gmInstance;
        joueur = gm.Joueur;
        cam_joueur = gm.Joueur.playerCam;

    }

    //le field qui veut acceder aux pannels d'upgrade doit appeler cette fonction
    //ainsi, on est sure que les upgrade que tu achete se font acheter sur le bon field
    public void SetReference(Field field) {
        field_ref = field;
    }
    
    //ouvre le bon panneau selon le type de field
    //cette fonction est aussi appeler par field
    public void Interact() {
        Cursor.visible = true;
        cam_joueur.GetComponent<CameraControl>().LockCamera();
        switch (field_ref.F_type) {
            case field_possibilities.empty: {
                    pannel_empty.SetActive(true);
                }break;
            case field_possibilities.agriculture:
                {
                    pannel_agriculture.SetActive(true);
                }
                break;
            case field_possibilities.enclos:
                {
                    pannel_enclos.SetActive(true);
                }
                break;
            case field_possibilities.entrepot:
                {
                    pannel_entrepot.SetActive(true);
                }
                break;
            case field_possibilities.mine:
                {
                    pannel_mine.SetActive(true);
                }
                break;
        }
    }

    //j'ai decide de tout mettre dans 1 script car sinon il faudrait 1 script par boutton et ca deviendrais intense si on veut changer
    //donc les noms sont long mais sont claires (et permette un certain ordre)
    //spammer moi si vous avez des questions and shit
    #region buttonFunction_pannel

    public void Btn_DeactivatePannelUpgrade()
    {
        Btn_Deactivate_pannel_not_enough();
        Btn_Delete_Denied();
        if (field_ref != null) {
            switch (field_ref.F_type)
            {
                case field_possibilities.empty:
                    {
                        pannel_empty.SetActive(false);
                    }
                    break;
                case field_possibilities.agriculture:
                    {
                        pannel_agriculture.SetActive(false);
                    }
                    break;
                case field_possibilities.enclos:
                    {
                        pannel_enclos.SetActive(false);
                    }
                    break;
                case field_possibilities.entrepot:
                    {
                        pannel_entrepot.SetActive(false);
                    }
                    break;
                case field_possibilities.mine:
                    {
                        pannel_mine.SetActive(false);
                    }
                    break;
            }
        }
       
    }//desactive le pannel upgrade appropri� basically faiut pour tous les return

    public void Btn_Delete_pannel()//Ask for confirmation
    {
        if (field_ref.F_type.Equals(field_possibilities.enclos))
        {
            if(field_ref.Enclos.GetComponent<Enclos>().Animaux.Count > 0)
            {
                pannel_too_much_Animal.SetActive(true);
            }
            else
            {
                Btn_DeactivatePannelUpgrade();
                pannel_delete_plot.SetActive(true);
            }
        }
        else
        {
            Btn_DeactivatePannelUpgrade();
            pannel_delete_plot.SetActive(true);
        }
    }

    public void Btn_Delete_Confirmed()//Revert back to Empty
    {

        if (field_ref != null) {
            switch (field_ref.F_type)
            {
                case field_possibilities.agriculture:
                    {
                        foreach (Button btn in btn_up_terre)
                            btn.interactable = true;
                    }
                    break;
                case field_possibilities.enclos:
                    {
                        foreach (Button btn in btn_up_enclos)
                            btn.interactable = true;
                    }
                    break;
                case field_possibilities.entrepot:
                    {
                        foreach (Button btn in btn_up_entrepot)
                            btn.interactable = true;
                    }
                    break;
                case field_possibilities.mine:
                    {
                        foreach (Button btn in btn_up_mine)
                            btn.interactable = true;
                    }
                    break;
            }
            field_ref.Deactivate_type();
            field_ref.F_type = field_possibilities.empty;
            pannel_delete_plot.SetActive(false);
            pannel_empty.SetActive(true);
        }
     
    }

    public void Btn_Delete_Denied()//Revert back to Empty
    {
        pannel_delete_plot.SetActive(false);

        if (field_ref != null) { 
            switch (field_ref.F_type)
        {
            case field_possibilities.agriculture:
                {
                    pannel_agriculture.SetActive(true);
                }
                break;
            case field_possibilities.enclos:
                {
                    pannel_enclos.SetActive(true);
                }
                break;
            case field_possibilities.entrepot:
                {
                    pannel_entrepot.SetActive(true);
                }
                break;
            case field_possibilities.mine:
                {
                    pannel_mine.SetActive(true);
                }
                break;
        }

        }

    }

    public void Btn_Deactivate_pannel_not_enough() {
        pannel_not_enough.SetActive(false);
        pannel_too_much_Animal.SetActive(false);
    }

    public void Return_cursor_norm() {
        Cursor.visible = false;
        cam_joueur.GetComponent<CameraControl>().UnLockCamera();
    }

    #endregion

    #region Upgrade_from_empty
    public void Btn_empty_agriculture()
    {
        if(GameManager.gmInstance.GetChronoCoin() <= btn_up_empty[0].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if(joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_empty[0].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.F_type = field_possibilities.agriculture;
            field_ref.Activate_type();
            pannel_empty.SetActive(false);
            pannel_agriculture.SetActive(true);
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[0].GetComponent<ShowPrice>().ChronoCoinPrice,true);
        }
        else {
            pannel_not_enough.SetActive(true);
        }

    }

    public void Btn_empty_enclos()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_empty[1].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_empty[1].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.F_type = field_possibilities.enclos;
            field_ref.Activate_type();
            pannel_empty.SetActive(false);
            pannel_enclos.SetActive(true);
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[1].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_empty_entrepot()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_empty[3].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_empty[3].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.F_type = field_possibilities.entrepot;
            field_ref.Activate_type();
            pannel_empty.SetActive(false);
            pannel_entrepot.SetActive(true);
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[3].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_empty_mine()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_empty[2].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_empty[2].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.F_type = field_possibilities.mine;
            field_ref.Activate_type();
            pannel_empty.SetActive(false);
            pannel_mine.SetActive(true);
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[2].GetComponent<ShowPrice>().ChronoCoinPrice, true);
        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    #endregion

    #region Upgrade_agriculture
    public void Btn_A_info_pannel() {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_terre[0].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_terre[0].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.jardin.GetComponent<Garden>().InformationPannel_Activate();
            btn_up_terre[0].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[0].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_A_irr_sys()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_terre[1].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_terre[1].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.jardin.GetComponent<Garden_UI>().Irr_sys_Activate();
            btn_up_terre[1].interactable = false;
            btn_up_terre[2].interactable = false;//disable autre options du choix
            GameManager.gmInstance.ModifyChronoCoin(btn_up_empty[1].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
               
    }

    public void Btn_A_rich_fert()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_terre[2].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_terre[2].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.jardin.GetComponent<Garden_UI>().Rich_fer_Activate();
            btn_up_terre[2].interactable = false;
            btn_up_terre[1].interactable = false;//disable autre options du choix
            GameManager.gmInstance.ModifyChronoCoin(btn_up_terre[2].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_A_chrono()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_terre[3].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_terre[3].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.jardin.GetComponent<Garden_UI>().Chrono_Activate();
            btn_up_terre[3].interactable = false;
            btn_up_terre[4].interactable = false;//disable autre options du choix
            GameManager.gmInstance.ModifyChronoCoin(btn_up_terre[3].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }          
    }

    public void Btn_A_crystal ()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_terre[4].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_terre[4].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.jardin.GetComponent<Garden_UI>().Crystal_Activate();
            btn_up_terre[4].interactable = false;
            btn_up_terre[3].interactable = false;//disable autre options du choix
            GameManager.gmInstance.ModifyChronoCoin(btn_up_terre[4].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }   
    }
    #endregion

    #region Upgrade_mineraux
    public void Btn_M_info_pannel()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_mine[0].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_mine[0].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Mine.GetComponent<Mine>().InformationPannel_Activate();
            btn_up_mine[0].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_mine[0].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }

    }

    public void Btn_M_stalactite()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_mine[1].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_mine[1].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Mine.GetComponent<Mine_UI>().Stalactite_Activate();
            btn_up_mine[1].interactable = false;
            btn_up_mine[2].interactable = false;//disable l'autre option mecanique
            GameManager.gmInstance.ModifyChronoCoin(btn_up_mine[1].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_M_rich_soil()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_mine[2].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_mine[2].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Mine.GetComponent<Mine_UI>().Rich_soil_Activate();
            btn_up_mine[2].interactable = false;
            btn_up_mine[1].interactable = false;//disable l'autre upgrade terrain
            GameManager.gmInstance.ModifyChronoCoin(btn_up_mine[2].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }

    }

    public void Btn_M_chrono()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_mine[3].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_mine[3].GetComponent<ShowPrice>().Liste_prix))
        {
        
        field_ref.Mine.GetComponent<Mine_UI>().Chrono_Activate();
        btn_up_mine[3].interactable = false;
        btn_up_mine[4].interactable = false;//disable l'autre option mecanique
            GameManager.gmInstance.ModifyChronoCoin(btn_up_mine[3].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }

    }

    public void Btn_M_rock()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_mine[4].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_mine[4].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Mine.GetComponent<Mine_UI>().Rare_rock_Activate();
            btn_up_mine[4].interactable = false;
            btn_up_mine[3].interactable = false;//disable l'autre upgrade terrain
            GameManager.gmInstance.ModifyChronoCoin(btn_up_mine[4].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }



    }
    #endregion

    #region Upgrade_enclos
    public void Btn_enclos_info_pannel()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_enclos[0].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_enclos[0].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Enclos.GetComponent<Enclos>().InfoPan_Activate();
            btn_up_enclos[0].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_enclos[0].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }


    }

    public void Btn_enclos_feeder()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_enclos[1].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_enclos[1].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Enclos.GetComponent<Enclos>().Auto_feeder_Activate();
            btn_up_enclos[1].interactable = false;
            btn_up_enclos[2].interactable = false;//disable other option
            GameManager.gmInstance.ModifyChronoCoin(btn_up_enclos[1].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    

    }

    public void Btn_enclos_grass_d()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_enclos[2].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_enclos[2].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Enclos.GetComponent<Enclos>().Grass_d_Activate();
            btn_up_enclos[2].interactable = false;
            btn_up_enclos[1].interactable = false;//disable other option
            GameManager.gmInstance.ModifyChronoCoin(btn_up_enclos[2].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    

    }

    public void Btn_enclos_water()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_enclos[3].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_enclos[3].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Enclos.GetComponent<Enclos>().Water_Activate();
            btn_up_enclos[3].interactable = false;
            btn_up_enclos[4].interactable = false;//disable other option
            GameManager.gmInstance.ModifyChronoCoin(btn_up_enclos[3].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }


    }

    public void Btn_enclos_grass_b()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_enclos[4].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_enclos[4].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Enclos.GetComponent<Enclos>().Grass_b_Activate();
            btn_up_enclos[4].interactable = false;
            btn_up_enclos[3].interactable = false;//disable other option
            GameManager.gmInstance.ModifyChronoCoin(btn_up_enclos[4].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
      
    }


    #endregion

    #region Upgrade_entrepot
    public void Btn_storage_chest1()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_entrepot[0].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_entrepot[0].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Entrepot.GetComponent<Entrepot>().Chest1_Activate();
            btn_up_entrepot[0].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_entrepot[0].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }

    }

    public void Btn_storage_chest2()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_entrepot[1].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_entrepot[1].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Entrepot.GetComponent<Entrepot>().Chest2_Activate();
            btn_up_entrepot[1].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_entrepot[1].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }
    }

    public void Btn_storage_chest3()
    {
        if (GameManager.gmInstance.GetChronoCoin() <= btn_up_entrepot[2].GetComponent<ShowPrice>().ChronoCoinPrice)
            return;

        if (joueur.BarreInventaire.TryPayWithMultipleItems(btn_up_entrepot[2].GetComponent<ShowPrice>().Liste_prix))
        {
            field_ref.Entrepot.GetComponent<Entrepot>().Chest3_Activate();
            btn_up_entrepot[2].interactable = false;
            GameManager.gmInstance.ModifyChronoCoin(btn_up_entrepot[2].GetComponent<ShowPrice>().ChronoCoinPrice, true);

        }
        else
        {
            pannel_not_enough.SetActive(true);
        }

    }
    #endregion
}
