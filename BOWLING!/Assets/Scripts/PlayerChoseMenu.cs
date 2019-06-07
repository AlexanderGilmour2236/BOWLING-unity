using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerChoseMenu : Menu
{
    public int PlayerCount { get; set; }
    public int MaxPlayers;
    private List<InputField> _playerInputs;
    [SerializeField] private Button OkButton;
    private void Start()
    {
        MaxPlayers = transform.childCount - 1;
        _playerInputs = new List<InputField>();
       

        foreach (Transform obj in transform)
        {
            foreach (Transform input in obj)
            {
                if (input.tag == "Input")
                {
                    InputField inp = input.GetComponent<InputField>();
                    _playerInputs.Add(inp);
                }
            }
        }

        for (int i = 0; i < _playerInputs.Count; i++)
        {
            Debug.Log(_playerInputs[i].inputType);
        }
        
        OkButton.onClick.AddListener(OnOkButton);
    }

    public override void Show(bool showParameter)
    {
        base.Show(showParameter);
        for (int i = 0; i < PlayerCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = MaxPlayers - 1; i >= PlayerCount ; i--)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnOkButton()
    {
        List<Player> players = new List<Player>();
        
        for (int i = 0; i < PlayerCount; i++)
        {
            Player newPlayer = new Player();
            newPlayer.ID = i;
            newPlayer.Name = _playerInputs[i].text;

            players.Add(newPlayer);
        }
        MenuController.Instance.HideAllMenues();
        GameController.Instance.GameStart(players);
    }
    
}
