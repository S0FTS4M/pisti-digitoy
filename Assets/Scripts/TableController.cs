using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TableController : MonoBehaviour
{
    [SerializeField] private GameObject _table;
    
    private Deck _deck;

    public GameObject Table => _table;

    public List<ICard> Cards { get; private set; } 

    [Inject]
    private void Construct()
    {

    }

    public void Initialize(Deck deck)
    {
        _deck = deck;
     
        Cards = new List<ICard>();
    }
    public void PutCard(CardView cardView)
    {
        
    }
}
