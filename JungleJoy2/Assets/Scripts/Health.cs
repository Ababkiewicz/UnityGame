using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int numOfHearts;

    public Image[] hearts;
    public Sprite heart;


    // Update is called once per frame
    void Update()
    {
        for (int i=0; i  < hearts.Length; i++){         

            if(i < numOfHearts){
                hearts[i].enabled = true;

            } else {
                hearts[i].enabled = false;
            }

        }
        
    }

public void Damage()
{

numOfHearts-=1;

}
}