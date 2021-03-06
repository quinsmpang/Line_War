﻿using UnityEngine;
using UnityEngine.Serialization;

namespace TrueSync {

    /**
     *  @brief Represents a physical 3D rigid body.
     **/
    [RequireComponent(typeof(TSCollider))]
    [AddComponentMenu("TrueSync/Physics/TSRigidBody", 11)]
    public class TSRigidBody : MonoBehaviour {

        public enum InterpolateMode { None, Interpolate, Extrapolate };

        [FormerlySerializedAs("mass")]
        [SerializeField]
        private FP _mass = 1;

        /**
         *  @brief Mass of the body. 
         **/
        public FP mass {
            get {
                if (tsCollider._body != null) {
                    return tsCollider._body.Mass;
                }

                return _mass;
            }

            set {
                _mass = value;

                if (tsCollider._body != null) {
                    tsCollider._body.Mass = value;
                }
            }
        }

        [SerializeField]
        private bool _useGravity = true;

        /**
         *  @brief If true it uses gravity force. 
         **/
        public bool useGravity {
            get {
                if (tsCollider.IsBodyInitialized) {
                    return tsCollider.Body.TSAffectedByGravity;
                }

                return _useGravity;
            }

            set {
                _useGravity = value;

                if (tsCollider.IsBodyInitialized) {
                    tsCollider.Body.TSAffectedByGravity = _useGravity;
                }
            }
        }

        [SerializeField]
        private bool _isKinematic;

        /**
         *  @brief If true it doesn't get influences from external forces. 
         **/
        public bool isKinematic {
            get {
                if (tsCollider.IsBodyInitialized) {
                    return tsCollider.Body.TSIsKinematic;
                }

                return _isKinematic;
            }

            set {
                _isKinematic = value;

                if (tsCollider.IsBodyInitialized) {
                    tsCollider.Body.TSIsKinematic = _isKinematic;
                }
            }
        }

        /**
         *  @brief Interpolation mode that should be used. 
         **/
        public InterpolateMode interpolation;

        /**
         *  @brief If true it freezes Z rotation of the RigidBody (it only appears when in 2D Physics).
         **/
        [HideInInspector]
        public bool freezeZAxis;

        private TSCollider _tsCollider;

        /**
         *  @brief Returns the {@link TSCollider} attached.
         */
        public TSCollider tsCollider {
            get {
                if (_tsCollider == null) {
                    _tsCollider = GetComponent<TSCollider>();
                }

                return _tsCollider;
            }
        }

        private TSTransform _tsTransform;

        /**
         *  @brief Returns the {@link TSTransform} attached.
         */
        public TSTransform tsTransform {
            get {
                if (_tsTransform == null) {
                    _tsTransform = GetComponent<TSTransform>();
                }

                return _tsTransform;
            }
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link TSVector} representing the force to be applied.
         **/
        public void AddForce(TSVector force) {
            AddForce(force, ForceMode.Force);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link TSVector} representing the force to be applied.
         *  @param mode Indicates how the force should be applied.
         **/
        public void AddForce(TSVector force, ForceMode mode) {
            if (mode == ForceMode.Force) {
                tsCollider.Body.TSApplyForce(force);
            } else if (mode == ForceMode.Impulse) {
                tsCollider.Body.TSApplyImpulse(force);
            }
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link TSVector} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForce(TSVector force, TSVector position) {
            AddForce(force, position, ForceMode.Impulse);
        }

        /**
         *  @brief Applies the provided force in the body. 
         *  
         *  @param force A {@link TSVector} representing the force to be applied.
         *  @param position Indicates the location where the force should hit.
         **/
        public void AddForce(TSVector force, TSVector position, ForceMode mode) {
            if (mode == ForceMode.Force) {
                tsCollider.Body.TSApplyForce(force, position);
            } else if (mode == ForceMode.Impulse) {
                tsCollider.Body.TSApplyImpulse(force, position);
            }
        }

        /**
         *  @brief Simulates the provided tourque in the body. 
         *  
         *  @param torque A {@link TSVector} representing the torque to be applied.
         **/
        public void AddTorque(TSVector torque) {
            tsCollider.Body.TSApplyTorque(torque);
        }


        /**
         *  @brief Changes orientation to look at target position. 
         *  
         *  @param target A {@link TSVector} representing the position to look at.
         **/
        public void LookAt(TSVector target) {
            rotation = TSQuaternion.CreateFromMatrix(TSMatrix.CreateFromLookAt(position, target));
            tsCollider.Body.TSUpdate();
        }

        /**
         *  @brief Moves the body to a new position. 
         **/
        public void MovePosition(TSVector position) {
            this.position = position;
        }

        /**
         *  @brief Rotates the body to a provided rotation. 
         **/
        public void MoveRotation(TSQuaternion rot) {
            this.rotation = rot;
        }

        /**
        *  @brief Position of the body. 
        **/
        public TSVector position {
            get {
                return tsTransform.position;
            }

            set {
                tsTransform.position = value;
            }
        }

        /**
        *  @brief Orientation of the body. 
        **/
        public TSQuaternion rotation {
            get {
                return tsTransform.rotation;
            }

            set {
                tsTransform.rotation = value;
            }
        }

        /**
        *  @brief LinearVelocity of the body. 
        **/
        public TSVector velocity {
            get {
                return tsCollider.Body.TSLinearVelocity;
            }

            set {
                tsCollider.Body.TSLinearVelocity = value;
            }
        }

        /**
        *  @brief AngularVelocity of the body. 
        **/
        public TSVector angularVelocity {
            get {
                return tsCollider.Body.TSAngularVelocity;
            }

            set {
                tsCollider.Body.TSAngularVelocity = value;
            }
        }

    }

}