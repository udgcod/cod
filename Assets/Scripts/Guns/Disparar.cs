using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    public float dany = 10f;
    public float distancia = 100f;
    public bool automatica = false;
    public float tempsEntreTir = 0.1f;

    bool potDisparar = true;

    //COOMPONENTS
    public Camera cam;

    //Altres
    public GestorArmes gunManager;

    void Update()
    {
        if(automatica){
            if(Input.GetButton("Fire1") && potDisparar){
                Shoot();
                Invoke("ActivarDisparar", tempsEntreTir);
            }
        }
        else{
            if(Input.GetButtonDown("Fire1") && potDisparar){
                Shoot();
                Invoke("ActivarDisparar", tempsEntreTir);
            }
        }
    }

    void Shoot(){
        potDisparar = false;
        RaycastHit hit; 
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distancia)){
            Debug.Log("Hem tocat: " + hit.transform.name + ". Amb un dany de: " + dany);
            //hit.collider.gameObject.GetComponent<VidaJugador>()?.takeDamage(dany);
        }
    }

    void ActivarDisparar(){
        potDisparar = true;
    }

    public void ActualitzarValors(){
        GameObject arma = gunManager.armes[gunManager.armaIndex];
        dany = arma.transform.GetComponent<ArmaInfo>().dany;
        distancia = arma.transform.GetComponent<ArmaInfo>().distancia;
        automatica = arma.transform.GetComponent<ArmaInfo>().automatica;
        tempsEntreTir = arma.GetComponent<ArmaInfo>().tempsEntreTir;
    }
}
