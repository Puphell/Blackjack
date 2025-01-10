using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> playerHand;
    public int handValue;
    public Deck deck;

    void Start()
    {
        playerHand = new List<Card>();
        handValue = 0;
    }

    public void DrawCard()
    {
        Card newCard = deck.DrawCard();
        if (newCard != null)
        {
            playerHand.Add(newCard);
            CalculateHandValue();
            CheckForBust();
        }
    }

    private void CalculateHandValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in playerHand)
        {
            if (card.cardName.Contains("Ace")) // As kartı kontrolü
            {
                aceCount++;
                value += 11; // As'ı başlangıçta 11 olarak say
            }
            else
            {
                value += card.cardValue;
            }
        }

        // Eğer toplam değer 21'i geçiyorsa, As'ları 1 olarak say
        while (value > 21 && aceCount > 0)
        {
            value -= 10; // As'ı 11'den 1'e düşür
            aceCount--;
        }

        handValue = value;
    }

    public void CheckForBust()
    {
        if (handValue > 21)
        {
            Debug.Log("Bust! Oyuncu kaybetti.");
            // Burada oyunun bitmesi veya oyuncunun kaybetmesi için gerekli işlemleri yapabilirsiniz.
        }
    }
}