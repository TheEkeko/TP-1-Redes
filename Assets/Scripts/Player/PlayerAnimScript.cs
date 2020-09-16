using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerAnimScript : MonoBehaviourPunCallbacks
{
    Animator _anim;
    bool value = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        //Un codigo normal para controlar animaciones
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            value = !value;
            _anim.SetBool("animTest", value);
        }
    }

    /*
    Para las animaciones lo que vamos a necesitar es el animator, un controlador normal de siempre, y el componente PhotonAnimatorView. Dentro del mismo vemos el desplegable
    Synchronize Parameters que ahi adrentro tenemos todas las variables del animator. Y al lado un desplegable que dice "disable", hay 2 opciones "Discrete" y "continuous".
    Discrete manda menos paquetes actualizando y Continuous manda mas paquetes (consume mas).

    Dentro de lo mismo tenemos las Layers de la animacion, pero como en este caso solo tenemos 1, ni nos preocupamos
    */

    /*
    Despues se la pasa haciendo lo mismo pero sin el PhotonViewAnim solo para romper las bolas, pero basicamente le pasamos el valor de la velocidad por photonView.RPC y que cada cliente le setee la anim.
    */
}