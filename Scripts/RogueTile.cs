using UnityEngine;

/* Questa classe e' lo script component da attaccare ai tile per renderli utilizzabili
 * dalla classe CityGenerator per generare la mappa, ha un GateTuple per indicare il tipo di
 * uscite e un float per indicare l'angolo rispetto al quale sono indicate le uscite, utile 
 * nel caso in cui un tile ha le uscite disposte in modo diverso a secondadi come lo si gira, 
 * tipo se un tile con angolo 0 ha uscite a nord=strada e a est=marciapiede, ruotato di 90 gradi 
 * ha est=strada e sud=marciapiede. Queste info sono importanti quando si istanzia il prefab del tile.
 * Vedi anche GateTuple.
 */
public class RogueTile : MonoBehaviour
{
    public GateTuple gates = new GateTuple();       //Indica le uscite del tile e se questo e' un tile di partenza 
    public float angle = 0;                         //Indica l'angolo di eulero lungo l'asse y rispetto a quale sono definiti
                                                    //i gate in GateTuple
}
