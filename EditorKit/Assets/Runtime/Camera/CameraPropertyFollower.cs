using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPropertyFollower : MonoBehaviour
{
    public enum TargetProperty
    {
        Position        = 1<<0,
        Rotation        = 1<<1,
        Scale           = 1<<2,
        ClearFlags      = 1<<3,
        CullingMask     = 1<<4,
        FieldOfView     = 1<<5,
        NearClipPlane   = 1<<6,
        FarClipPlane    = 1<<7,
        Depth           = 1<<8
    }

    public enum UpdateType
    {
        Update,
        LateUpdate,
        FixedUpdate
    }

    [EnumFlags]
    public TargetProperty selectedProperty;
    public UpdateType updateType;

    public Camera source;
    public Camera follower;


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (updateType != UpdateType.Update)
            return;
        PropertyFollow();
    }

    void LateUpdate()
    {
        if (updateType != UpdateType.LateUpdate)
            return;
        PropertyFollow();
    }

    void FixedUpdate()
    {
        if (updateType != UpdateType.FixedUpdate)
            return;
        PropertyFollow();
    }

    private void PropertyFollow()
    {
        if (source == null || follower == null)
            return;
        int result = (int)selectedProperty;

        
        if ((result & (int)TargetProperty.Position) == (int)TargetProperty.Position)
        {
            follower.transform.position = source.transform.position;
        }
        if ((result & (int)TargetProperty.Rotation) == (int)TargetProperty.Rotation)
        {
            follower.transform.rotation = source.transform.rotation;
        }
        if ((result & (int)TargetProperty.Scale) == (int)TargetProperty.Scale)
        {
            follower.transform.localScale = source.transform.localScale;
        }
        if ((result & (int)TargetProperty.ClearFlags) == (int)TargetProperty.ClearFlags)
        {
            follower.clearFlags = source.clearFlags;
        }
        if ((result & (int)TargetProperty.CullingMask) == (int)TargetProperty.CullingMask)
        {
            follower.cullingMask = source.cullingMask;
        }
        if ((result & (int)TargetProperty.FieldOfView) == (int)TargetProperty.FieldOfView)
        {
            follower.fieldOfView = source.fieldOfView;
        }
        if ((result & (int)TargetProperty.NearClipPlane) == (int)TargetProperty.NearClipPlane)
        {
            follower.nearClipPlane = source.nearClipPlane;
        }
        if ((result & (int)TargetProperty.FarClipPlane) == (int)TargetProperty.FarClipPlane)
        {
            follower.farClipPlane = source.farClipPlane;
        }
        if ((result & (int)TargetProperty.Depth) == (int)TargetProperty.Depth)
        {
            follower.depth = source.depth;
        }
    }
}
