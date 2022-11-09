using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyFracture : MonoBehaviour
{
    public SkinnedMeshRenderer _renderer;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Weapon"))
        {
            // set mesh renderer and mesh filter
            var mesh = new Mesh();
            _renderer.BakeMesh(mesh);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().material = _renderer.material;
            _renderer.enabled = false;
            GetComponent<Animator>().enabled = false;
        }
    }
}