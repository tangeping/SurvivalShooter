using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public class SyncConfig : ScriptableObject
    {
        private const int COLLISION_LAYERS = 32;
        // 32 layers -> 516 unique intersections
        private const int COLLISION_TOGGLES = 516;

        /**
         * @brief Synchronization window size.
         **/
        public int syncWindow = 10;

        /**
         * @brief Rollback window size.
         **/
        public int rollbackWindow = 0;

        /**
         * @brief Maximum number of ticks to wait until all other players inputs arrive.
         **/
        public int panicWindow = 100;

        /**
         * @brief Indicates if the 2D physics engine should be enabled.
         **/
        public bool physics2DEnabled = true;

        /**
         * @brief Holds which layers should be ingnored in 2D collisions
         **/
        public bool[] physics2DIgnoreMatrix = new bool[COLLISION_TOGGLES];

        /**
         *  @brief Represents the simulated gravity.
         **/
        public TSVector2 gravity2D = new TSVector2(0, -10);

        /**
         *  @brief If true enables a deeper collision detection system.
         **/
        public bool speculativeContacts2D = false;

        /**
         * @brief Indicates if the 3D physics engine should be enabled.
         **/
        public bool physics3DEnabled = true;

        /**
         * @brief Holds which layers should be ingnored in 3D collisions
         **/
        public bool[] physics3DIgnoreMatrix = new bool[COLLISION_TOGGLES];

        /**
         *  @brief Represents the simulated gravity.
         **/
        public TSVector gravity3D = new TSVector(0, -10, 0);

        /**
         *  @brief If true enables a deeper collision detection system.
         **/
        public bool speculativeContacts3D = false;

        /**
         * @brief When true shows a debug interface with a few information.
         **/
        public bool showStats = false;

        /**
         * @brief Time between each TrueSync's frame.
         **/
        public FP lockedTimeStep = 0.02f;

        public SyncConfig()
        {
        }


    }
}
