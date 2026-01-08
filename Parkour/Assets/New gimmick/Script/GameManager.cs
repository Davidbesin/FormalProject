using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public enum GameState {
       Playing, Pause, Stop
   };
   public GameState currentState;

   //PLAY ELEMENT
   public GameObject Tunde;
   public GameObject startingChunck;
   public GameObject swipeManager;


   // UI AFTER STOP
   public GameObject stopUI;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        currentState = GameState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
      /* if (currentState = GameState.Playing)
       {
           Tunde.SetActive(true);
           startingChunck.SetActive(true);
           swipeManager.SetActive(true);
           stopUI.SetActive = 
       } */

      
    }
}
