﻿using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class DevSettingsMenu : MonoBehaviour
{
    [Header("UI references")]
    [SerializeField] private GameObject OpenMenuButton = default;
    [SerializeField] private GameObject DevMenu = default;
    [SerializeField] private TMPro.TMP_Dropdown ServerDropdown = default;
    [SerializeField] private TMPro.TMP_InputField CommandField = default;
    [SerializeField] private TMPro.TMP_Text CommandResultText = default;

    private DevCommands devCommands;

    private int ServerToDropdownNumber(Servers server)
    {
        switch(server)
        {
            case Servers.PRODUCTION:
                return 0;
            case Servers.LOCAL:
                return 1;
            default:
                throw new NotImplementedException("No dropdown value for server " + server);
        }
    }

    private Servers DropdownNumberToServer(int dropdownNumber)
    {
        switch(dropdownNumber)
        {
            case 0:
                return Servers.PRODUCTION;
            case 1:
                return Servers.LOCAL;
            default:
                throw new NotImplementedException("No server for dropdown value " + dropdownNumber);
        }
    }

    void Start()
    {
        ServerDropdown.value = ServerToDropdownNumber(StaticNetworkSettings.CurrentServer);
        devCommands = GetComponent<DevCommands>();
    }

    public void OnOpenMenuButtonClicked()
    {
        OpenMenuButton.SetActive(false);
        DevMenu.SetActive(true);
    }

    public void OnCloseButtonClicked()
    {
        OpenMenuButton.SetActive(true);
        DevMenu.SetActive(false);
    }

    public void OnChangeServerButtonClicked()
    {
        StaticNetworkSettings.CurrentServer = DropdownNumberToServer(ServerDropdown.value);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnCommandRunButtonClicked()
    {
        string command = CommandField.text;
        CommandField.text = "";

        try
        {
            MethodInfo mi = typeof(DevCommands).GetMethod(command, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi == null)
            {
                throw new ArgumentException("No available method named \"" + command + "\" in DevCommands.cs");
            }

            mi.Invoke(devCommands, null);

            CommandResultText.text = ">" + command + "\n" + CommandResultText.text;
        } catch (Exception e)
        {
            CommandResultText.text = "><color=red>Error running command \"" + command + "\": " + e.Message + "</color>\n" + CommandResultText.text;
            Debug.LogException(e, this);
        }

    }
}
