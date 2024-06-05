using System.Collections.Generic;
using System.Linq;
using Code.Data.Enums;
using Code.Data.Interfaces;
using Code.GameParts.Components;
using Code.Utils;
using UnityEngine;

namespace Code.GameParts
{
    public class GamePart_1_Home: GamePart, IGameInitListener, IGameStartListener
    {
        public override GamePartName GamePartName => GamePartName.Part_1__home;
        
        [SerializeField] private CollisionObserver _leftDoor;
        [SerializeField] private CollisionObserver _rightDoor;
        [SerializeField] private CollisionObserver _centralDoor;
        
         private Dictionary<string,bool> _openedDoors = new();

         private const string LeftDoorKey = "left";
         private const string CentralDoorKey = "central";
         private const string RightDoorKey = "right";

         public void GameInit()
         {
             _openedDoors = new Dictionary<string, bool>()
             {
                 { LeftDoorKey, false },
                 { CentralDoorKey, false },
                 { RightDoorKey, false },
             };
         }

         public void GameStart()
         { 
             SubscribeToEvents(true);   
         }

         private void SubscribeToEvents(bool flag)
         {
             if (flag)
             {
                 _leftDoor.OnEnter += _ =>
                 {
                     _openedDoors[LeftDoorKey] = true;
                     InvokeUpdateDataEvent();
                 };
                 _centralDoor.OnEnter += _ =>
                 {
                     _openedDoors[CentralDoorKey] = true;
                     InvokeUpdateDataEvent();
                 };
                 _rightDoor.OnEnter += _ =>
                 {
                     _openedDoors[RightDoorKey] = true;
                     InvokeUpdateDataEvent();
                 };
             }
         }

         public override void Restart()
         {
             base.Restart();
             
             foreach (var openedDoor in _openedDoors)
             {
                 _openedDoors[openedDoor.Key] = false;
             }
   
             InvokeUpdateDataEvent();
         }

         public bool IsTryOpenAllDoor()
         {
             foreach (var openedDoor in _openedDoors)
             {
                 if (!openedDoor.Value)
                 {
                     return false;
                 }
             }
             return true;
         }
    }
}