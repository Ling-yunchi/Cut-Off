using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Polygon;
using BzKovSoft.ObjectSlicer.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BzKovSoft.CharacterSlicer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

public class CharacterSlicer : BzSliceableCharacterBase, IBzSliceableNoRepeat
{
#pragma warning disable 0649
    [HideInInspector] [SerializeField] int _sliceId;
    [HideInInspector] [SerializeField] float _lastSliceTime = float.MinValue;
    [SerializeField] int _maxSliceCount = 3;
#pragma warning restore 0649
    public bool IsDead { get; private set; }

    public delegate void OnDead();
    public event OnDead OnDeadEvent;

    /// <summary>
    /// If your code do not use SliceId, it can relay on delay between last slice and new.
    /// If real delay is less than this value, slice will be ignored
    /// </summary>
    public float delayBetweenSlices = 1f;

    public void Slice(Plane plane, int sliceId, Action<BzSliceTryResult> callBack)
    {
        float currentSliceTime = Time.time;

        // we should prevent slicing same object:
        // - if _delayBetweenSlices was not exceeded
        // - with the same sliceId
        if ((sliceId == 0 & _lastSliceTime + delayBetweenSlices > currentSliceTime) |
            (sliceId != 0 & _sliceId == sliceId))
        {
            return;
        }

        // exit if it have LazyActionRunner
        if (GetComponent<LazyActionRunner>() != null)
            return;

        _lastSliceTime = currentSliceTime;
        _sliceId = sliceId;

        Slice(plane, callBack);
    }

    protected override BzSliceTryData PrepareData(Plane plane)
    {
        if (_maxSliceCount == 0)
            return null;

        var addData = plane;

        // collider we want to participate in slicing
        var collidersArr = GetComponentsInChildren<Collider>();

        // create component manager.
        var componentManager = new CharacterComponentManagerFast(this.gameObject, plane, collidersArr);

        return new BzSliceTryData()
        {
            componentManager = componentManager,
            plane = plane,
            addData = addData
        };
    }

    protected override void OnSliceFinished(BzSliceTryResult result)
    {
        if (!result.sliced)
            return;

        var resultNeg = result.outObjectNeg.GetComponent<CharacterSlicer>();
        var resultPos = result.outObjectPos.GetComponent<CharacterSlicer>();

        var plane = (Plane) result.addData;

        result.outObjectNeg.layer = 8;
        result.outObjectNeg.GetComponent<Animator>().enabled = false;
        var body = result.outObjectNeg.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.None;
        body.AddForce(-plane.normal * 5, ForceMode.Impulse);
        // Debug.DrawRay(result.outObjectNeg.transform.position, -plane.normal * 100000, Color.red, 10f);
        Destroy(result.outObjectNeg, 10f);

        result.outObjectPos.layer = 8;
        result.outObjectPos.GetComponent<Animator>().enabled = false;
        body = result.outObjectPos.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.None;
        body.AddForce(plane.normal * 5, ForceMode.Impulse);
        // Debug.DrawRay(result.outObjectPos.transform.position, -plane.normal * 100000, Color.red, 10f);
        Destroy(result.outObjectPos, 10f);

        IsDead = true;
        resultNeg.IsDead = IsDead;
        resultPos.IsDead = IsDead;
        OnDeadEvent?.Invoke();

        --_maxSliceCount;
        resultNeg._maxSliceCount = _maxSliceCount;
        resultPos._maxSliceCount = _maxSliceCount;
    }
}