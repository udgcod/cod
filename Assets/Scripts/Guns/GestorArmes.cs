using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorArmes : MonoBehaviour
{
    public GameObject[] armes;
    public GameObject aimImage;

    public int armaIndex; 
    int armaPreviaIndex = -1;

    //COMPONENTS
    public Disparar disparar;

    void Start(){
        for(int i=0; i<armes.Length; i++){
            armes[i].SetActive(false);
        }
        EquiparArma(0);
    }

    void Update(){
        for(int i=0; i<=armes.Length; i++){
            if(Input.GetKeyDown((i+1).ToString())){
                EquiparArma(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") >0f){
            if(armaIndex >= armes.Length -1) EquiparArma(0);
            else EquiparArma(armaIndex+1);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") <0f){
            if(armaIndex <= 0) EquiparArma(armes.Length -1);
            else EquiparArma(armaIndex-1);
        }
    }

    void EquiparArma(int _index){
        if(_index < armes.Length){
            if(_index == armaPreviaIndex){
                return;
            }
            armaIndex = _index;
            armes[armaIndex].SetActive(true);
            if(armaPreviaIndex != -1){
                armes[armaPreviaIndex].SetActive(false);
            }

            //Assignem dades
            disparar.ActualitzarValors();

            aimImage.GetComponent<Image>().sprite = armes[armaIndex].transform.GetComponent<ArmaInfo>().aimImage;
            aimImage.GetComponent<Image>().SetNativeSize();

            armaPreviaIndex = armaIndex;
        }
    }
}
