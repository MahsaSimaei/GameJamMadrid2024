using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Money_mangement : MonoBehaviour
{
    public static int money;
    public TMP_Text amount;  // Champ public pour assigner via l'inspecteur

    void Start()
    {
        money = 1000;  // Valeur initiale d'argent
        amount.text = "Money: " + money;
    }

    void Update()
    {
        amount.text = "Money: " + money;  // Mise à jour de l'affichage
    }
}