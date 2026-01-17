using UnityEngine;
using System; 

public class GameManager : MonoBehaviour
{
     public static GameManager Instance;

     public enum GameState {
         Playing, Pause, Stop
     };

     public GameState currentState;

   //PLAY ELEMENT
   public GameObject tunde;
   public GameObject startingChunck;
   public GameObject swipeManager;
   private MoveSlowly mS;
   
   public GameObject start;
    private void OnEnable() {InputStateMachine.death += Stopping;}
    private void OnDisable() {InputStateMachine.death -= Stopping;}

   // UI AFTER STOP
   public GameObject stopUI;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       Instance = this;
       currentState = GameState.Stop;
       mS = startingChunck.GetComponent<MoveSlowly>();
   }
      
   // Update is called once per frame
   public void Play()
   {
       currentState = GameState.Playing;
       tunde.SetActive(true);
       startingChunck.SetActive(true);
       swipeManager.SetActive(true);
       stopUI.SetActive(false);
       start.SetActive(false);
   }

   public void Stopping()
   { 
       currentState = GameState.Stop;
       tunde.SetActive(false);
       startingChunck.SetActive(false);
       swipeManager.SetActive(false);
       stopUI.SetActive(true);
       start.SetActive(true);
   }
    
}
