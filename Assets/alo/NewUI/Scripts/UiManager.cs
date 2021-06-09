using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{

    [SerializeField] private PlayerUfo playerUfo;
    [SerializeField] private UIDocument debugOverlay;

    private TextField rotation;
    private TextField playerName;

    private Button respawn;

    private void OnEnable()
    {
        var root = debugOverlay.rootVisualElement;

        rotation = root.Q<TextField>("textField-Rotation");

        playerName = root.Q<TextField>("textField-PlayerName");
        //playerName.RegisterValueChangedCallback(ctx => Debug.Log("New value: " + ctx.newValue));
        playerName.RegisterValueChangedCallback(this.playerNameUpdate);

        respawn = root.Q<Button>("button-Respawn");
        respawn.clickable.clicked += () => playerUfo.respawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerName.value = "" + playerUfo.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        rotation.value = "" + playerUfo.direction;
        //playerName.value = "" + playerUfo.playerName;
    }

    private void playerNameUpdate(ChangeEvent<string> ctx)
    {
        Debug.Log("New value: " + ctx.newValue);
        playerUfo.playerName = ctx.newValue;
    }
}
