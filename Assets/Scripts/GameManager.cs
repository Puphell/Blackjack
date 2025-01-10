using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Dealer dealer;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        player.DrawCard();
        player.DrawCard();
        dealer.DrawCard();
        dealer.DrawCard();
    }

    public void PlayerHit()
    {
        player.DrawCard();
    }

    public void PlayerStand()
    {
        dealer.PlayTurn();
        DetermineWinner();
    }

    void DetermineWinner()
    {
        if (player.handValue > 21)
        {
            Debug.Log("Oyuncu kaybetti.");
        }
        else if (dealer.handValue > 21 || player.handValue > dealer.handValue)
        {
            Debug.Log("Oyuncu kazandý!");
        }
        else if (player.handValue < dealer.handValue)
        {
            Debug.Log("Dealer kazandý.");
        }
        else
        {
            Debug.Log("Beraberlik.");
        }
    }
}