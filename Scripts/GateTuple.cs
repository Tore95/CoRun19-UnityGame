using System.Collections.Generic;

/* Questa classe rappresenta le caratteristiche identificative del singolo tile, cioe':
 * -quali sono le possibili uscite (nord, sud, ecc) , 
 * -di che tipo sono (marciapiede,strada o muro se non si puo' uscire da quella direzione)
 * -se questo tile e' il punto da cui inizia il gioco (isStart) 
 * Questa classe fa da supporto alla classe RogueTile e, visto che non va attaccata a un 
 * GameObject, non estende MonoBehaviour, la classe e' Serializable cosi' esce nell'ispector
 */
[System.Serializable]
public class GateTuple
{
    // enum che indica il tipo di uscita, Wall indica assenza di uscita
    public enum GateType { Wall, Street, Sidewalk }

    //Per ogni direzione c'e' un'istanza della enum che indica che tipo di uscita c'e'
    // nord e' l'asse -X, sud +X, est +Z, ovest -Z
    public GateType north;      
    public GateType west;
    public GateType south;
    public GateType east;

    public bool isStart;    //Flag per indicare che questo e' un possibile tile di partenza per il player
    public bool isFinal;

    //Costr. di default, crea un tile senza uscite non di partenza
    public GateTuple()
    {
        north = GateType.Wall;
        west = GateType.Wall;
        south = GateType.Wall;
        east = GateType.Wall;
        isStart = false;
        isFinal = false;
    }

    // I metodi seguenti servono per poter usare la classe come chiave di un Dictionary
    // nella classe CityGenerator, me li sono fatti generare in automatico da Visual studio
    public override bool Equals(object obj)
    {
        return obj is GateTuple tuple &&
               north == tuple.north &&
               west == tuple.west &&
               south == tuple.south &&
               east == tuple.east &&
               isStart == tuple.isStart &&
               isFinal == tuple.isFinal;
    }

    public override int GetHashCode()
    {
        int hashCode = 103734278;
        hashCode = hashCode * -1521134295 + north.GetHashCode();
        hashCode = hashCode * -1521134295 + west.GetHashCode();
        hashCode = hashCode * -1521134295 + south.GetHashCode();
        hashCode = hashCode * -1521134295 + east.GetHashCode();
        hashCode = hashCode * -1521134295 + isStart.GetHashCode();
        hashCode = hashCode * -1521134295 + isFinal.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(GateTuple left, GateTuple right)
    {
        return EqualityComparer<GateTuple>.Default.Equals(left, right);
    }

    public static bool operator !=(GateTuple left, GateTuple right)
    {
        return !(left == right);
    }
}

