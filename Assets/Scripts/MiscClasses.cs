using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#region enums
#endregion
#region classes
public class HelperScripts
{
}
#endregion
#region interfaces
public interface IGrabbable
{
    public bool canBeGrabbed { get; set; }
    public void OnGrab();
    public void OnThrow(Vector3 _direction);
}

public interface ISwingable
{
    public void OnGrab();
    public void OnRelease();
}

public interface IChargeSource
{
    public bool isCharged { get; set;}
}

public interface IPhysicsUser
{
    public void SetVelocity(Vector3 _newVel);
}

public interface IHurtable
{
    public void OnHit(int _dmg = 1);
}
#endregion
