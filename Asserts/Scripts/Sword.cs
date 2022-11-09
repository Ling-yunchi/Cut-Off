using System;
using System.Collections;
using System.Collections.Generic;
using BzKovSoft.CharacterSlicer.Samples;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sword collided with " + collision.gameObject.name);
        var slice = collision.collider.GetComponent<CharacterSlicer>();
        if (slice)
        {
            var contactPoint = collision.GetContact(0);
            // calculate the normal of the slice
            var normal = contactPoint.normal;
            var swordUp = transform.right;
            // var sliceNormal = Vector3.Cross(swordUp, normal);
            
            slice.Slice(new Plane(swordUp, contactPoint.point),0, null);
        }
    }
}
