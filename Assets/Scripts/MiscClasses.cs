using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#region enums
#endregion
#region classes

public class LevelData
{
    public string levelName;
    public Sprite levelImage;
    public List<bool> collectableStates;
    public float bestTime;
    public bool beaten;

    public LevelData()
    {
        levelName = "Dummy";
        levelImage = null;
        bestTime = 9999;
        collectableStates = new List<bool> { false , false ,false};
        beaten = false;
    }
}
public class HelperScripts
{
    public static int Wrapped(int i, int min,int max)
    {
        return i;
    }
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
