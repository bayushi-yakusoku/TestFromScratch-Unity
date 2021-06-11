using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class DisplayingInfo : MonoBehaviour
{

    [SerializeField] private TMP_Text displayBox;
    [SerializeField] private PlayerUfo playerUfo;

    //private PlayerUfo ufo;


    private void Awake()
    {
        Assert.IsNotNull(playerUfo, "playerUfo is null");
        Assert.IsNotNull(displayBox, "displayBox is null");
    }


    // Start is called before the first frame update
    void Start()
    {
        //ufo = playerUfo.GetComponent<PlayerUfo>();
    }

    // Update is called once per frame
    void Update()
    {
        string info = "";
        
        info += "Thrust: " + playerUfo.Collective + "\n";
        info += "Direction: " + playerUfo.Cyclic + "\n";

        displayBox.text = info;
    }
}
