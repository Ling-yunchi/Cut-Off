using System.Collections;
using System.Collections.Generic;
using BzKovSoft.CharacterSlicer.Samples;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    private CharacterSlicer _sliceCharacter;

    void Start()
    {
        _sliceCharacter = GetComponent<CharacterSlicer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_sliceCharacter.IsDead)
        {
            Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
        }
    }
}