using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public List<Card> dealerHand;
    public int handValue;
    public Deck deck;

    void Start()
    {
        dealerHand = new List<Card>();
        handValue = 0;
    }

    public void PlayTurn()
    {
        while (handValue < 17)
        {
            DrawCard();
        }
        CheckForBust();
    }

    public void DrawCard()
    {
        Card newCard = deck.DrawCard();
        if (newCard != null)
        {
            dealerHand.Add(newCard);
            handValue += newCard.cardValue;
        }
    }

    void CheckForBust()
    {
        if (handValue > 21)
        {
            Debug.Log("Dealer Bust! Oyuncu kazandý.");
        }
    }
}