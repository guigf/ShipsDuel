using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Game : MonoBehaviour
{
    public Material mat;
    public Vector2 screenBoard;
    public Player player1;
    public Player player2;

    public bool gameOn = false;
    public bool pause = false;
    public bool endRoud = false;

    public Text player1Info;
    public Text player2Info;
    public Text txtWinner;
    public GameObject roundPanel;
    public Button btnPlay;
    public Button btnControls;
    public Button btnBack;
    public Button btnMenu;

    private void Start()
    {
        screenBoard = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    private void Update()
    {
        screenBoard = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        if (gameOn && Input.GetKeyDown(KeyCode.P))
        {
            pause = !pause;
        }

        if (endRoud && Input.GetKeyDown(KeyCode.R))
        {
            ResetRound();
        }

        if (!gameOn && Input.GetKeyDown(KeyCode.J))
        {
            btnPlay.onClick.Invoke();
        }

        if (!gameOn && Input.GetKeyDown(KeyCode.C))
        {
            btnControls.onClick.Invoke();
        }

        if (!gameOn && Input.GetKeyDown(KeyCode.V))
        {
            btnBack.onClick.Invoke();
        }

        if (gameOn && Input.GetKeyDown(KeyCode.M))
        {
            btnMenu.onClick.Invoke();
        }

        if (gameOn && !pause && !endRoud)
        {
            player1.Update(screenBoard);
            player2.Update(screenBoard);

            player1.CheckProjectileHit(player2);
            player2.CheckProjectileHit(player1);

            CheckWinner();

            UpdateInfo();
        }
    }

    private void OnPostRender()
    {
        if (gameOn)
        { 
            player1.Render(mat);
            player2.Render(mat);
        }
    }

    public void StartGame()
    {
        player1 = new Player(
            Color.magenta, 
            screenBoard.x * (-1) + 1,
            1,
            KeyCode.W,
            KeyCode.S,
            KeyCode.D,
            "Player 1"
            );


        player2 = new Player(
            Color.green, 
            screenBoard.x - 2,
            -1,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            "Player 2"
            );

        UpdateInfo();
        gameOn = true;
    }

    public void ResetRound()
    {
        player1.ResetRound();
        player2.ResetRound();

        UpdateInfo();

        roundPanel.SetActive(false);
        endRoud = false;
    }

    private void EndRound(Player winner)
    {
        endRoud = true;

        txtWinner.text = winner.Name + " venceu!";

        roundPanel.SetActive(true);
    }

    private void UpdateInfo()
    {
        player1Info.text = GetPlayerInfoText(player1);
        player2Info.text = GetPlayerInfoText(player2);
    }

    private string GetPlayerInfoText(Player player)
    {
        return String.Format("{0} \nVida: {1} \nVitórias: {2}", player.Name, player.Life, player.Score);
    }

    private void CheckWinner()
    {
        if (player2.Life <= 0 && player1.Life > 0)
        {
            player1.Score++;
            EndRound(player1);
        } else if (player2.Life > 0 && player1.Life <= 0)
        {
            player2.Score++;
            EndRound(player2);
        } else if (player2.Life <= 0 && player1.Life <= 0)
        {
            
        }
    }

    public void GoToMenu()
    {
        gameOn = false;
        endRoud = false;
        pause = false;
        player1 = null;
        player2 = null;
    }
}
