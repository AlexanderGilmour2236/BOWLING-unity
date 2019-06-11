using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChosePlayersMenuResultArgs
{
    public List<Player> Players;
}

public class PlayerChoseMenu : Menu
{   
    #region events

    public delegate void onMenuResult(object sender, ChosePlayersMenuResultArgs a);
    public event onMenuResult OnMenuResult;
    
    #endregion
    
    public int PlayerCount { get; set; }
    public int MaxPlayers;
    private List<InputField> _playerInputs;
    [SerializeField] private Button okButton;
    
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
        
        okButton.onClick.AddListener(OnOkButton);
    }

    private void OnDestroy()
    {
        OnMenuResult = delegate(object sender, ChosePlayersMenuResultArgs args) {  };
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
            newPlayer.id = i;
            newPlayer.name = _playerInputs[i].text;
            
            players.Add(newPlayer);
        }

        if (OnMenuResult != null)
        {
            OnMenuResult(this, new ChosePlayersMenuResultArgs{Players = players} );
        }
    }
    
}
