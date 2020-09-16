using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun
{
    float speed = 3f;
    Material _mat;
    [SerializeField] string bulletPrefabPath;

    private void Awake()
    {
        Debug.Log("Controles: A - Cambia de animacion | S - Dispara | Espacio - Cambia a color rojo");
        //asi agarramos el material del objeto donde este este script.
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Esto lo vamos a tener que hacer mas prolijo con un script de movimiento supongo, aparte no se si esto funcionara cuando hagamos el server pero creo que eso es para el proximo tp
        if (!photonView.IsMine) return;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v) * Time.deltaTime * speed;

        //ejemplo de modificacion hacia MI objeto, pero que sea visible en el resto (sirve como ejemplo de cosas como para sacar vida y demas)
        if (Input.GetKeyDown(KeyCode.Space)) photonView.RPC("ChangeColor", RpcTarget.All); //se ejecuta la function ChangeColor en MI usuario, pero le aviso a TODOS (incluido a mi mismo)
                                                                                           //Tambien, despues del Rcptarget, podemos pasarle parametros (en caso de que la funcion que llamemos requiera parametros). Simplemente photonView.RPC("funcion", RpcTarget.Others, var1, var2, var3, var4);

        /*
        RpcTarget
            .All = Todos incluido yo
            .Others = Todos menos yo
            .MasterClient = Solo el cliente Maestro (seriviria para balas con Raycast)
            .AllBuffered/.OtherBuffered  =  Lo mismo, lo unico que guarda que funciones se ejecutaron en un buffer, y si un cliente nuevo se conecta, 
                                            ejecuta todas estas funciones que se guardaron en el buffer. Si no lo hiciera, el cliente recien conectado no veria los cambios.
        */

        /*
        Para mandar "requests y Responses", hacemos: RpcTarget.MasterClient y despues el MasterClient reproduce la funcion en RpcTarget.Others.
        A esto se le pueden mandar parametros de esta forma: photonView.RPC("funcion", RpcTarget.Others, var1, var2, var3, var4);
        Hay restricciones, pero ahi entra lo que vimos en la clase 3, de la serialización. Si no podemos pasar un parametro, lo podemos serializar y mandarlo.
        */

        // Con photonViwer.owner podemos pasarle el poseedor de ese elemento en el mundo

        /*
        Full-Authoritive: Todo lo hace el server
        Non-Authoritive: Todo lo hace cada cliente
        Hybrid: Las cosas importantes las hace el Server, lo demas lo hace cada cliente -- Me parece esta la mejor opcion
        */

        if (Input.GetKeyDown(KeyCode.S)) photonView.RPC("Shoot", RpcTarget.MasterClient);
    }

    [PunRPC] //Todas estas funciones que deban ser ejecutadas en otros clientes, requieren esta propiedad, sino, nos tira un error de que no encuentra la funcion
    void ChangeColor()
    {
        _mat.color = Color.red;
    }

    [PunRPC]
    void Shoot()
    {
        var bullet = PhotonNetwork.Instantiate(bulletPrefabPath, transform.position, Quaternion.identity).GetComponent<BulletTest>();
        var dir = transform.forward;
        dir.y = 0.5f;
        bullet.Shoot(dir);
    }
}
