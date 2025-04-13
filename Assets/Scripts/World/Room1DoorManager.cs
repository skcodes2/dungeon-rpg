using UnityEngine;
using System.Collections.Generic;

public class Room1DoorManager: MonoBehaviour{

    public static Room1DoorManager Instance;

     void Awake(){
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public GameObject section1Doors; 
    
    public GameObject section2Doors;

    private int enemiesLeftSection1 = 2;

    private int enemiesLeftSection2 = 3;

    public void OpenSection1Doors(){
        section1Doors.SetActive(false);
        
    }
    public void OpenSection2Doors(){
        section2Doors.SetActive(false);
    }

    public void EnemyDefeatedSection1(){
        enemiesLeftSection1--;
        if(enemiesLeftSection1 <= 0){
            OpenSection1Doors();
        }
    }
    public void EnemyDefeatedSection2(){
        enemiesLeftSection2--;
        if(enemiesLeftSection2 <= 0){
            OpenSection2Doors();
        }
    }

}

