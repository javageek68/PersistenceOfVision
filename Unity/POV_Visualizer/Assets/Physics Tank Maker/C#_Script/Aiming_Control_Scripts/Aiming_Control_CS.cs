using System.Collections;
using UnityEngine;
using System.Collections.Generic;


namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+1)] // (Note.) This script is executed after other scripts, in order to detect the target certainly.
    public class Aiming_Control_CS : MonoBehaviour
	{
		/*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the aiming of the tank.
		 * "Turret_Horizontal_CS" and "Cannon_Vertical_CS" scripts rotate the turret and cannon referring to this variables.
		*/

		// User options >>
		public float OpenFire_Angle = 180.0f;
        // << User options


        int inputType;
        Turret_Horizontal_CS[] turretHorizontalScripts;
        Cannon_Vertical_CS[] cannonVerticalScripts;
		public bool Use_Auto_Turn; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
        public bool Use_Auto_Lead; // Referred to from "Turret_Horizontal_CS".
        public float Aiming_Blur_Multiplier = 1.0f; // Referred to from "Turret_Horizontal_CS".

        // For auto-turn.
        public int Mode; // Referred to from "UI_Aim_Marker_Control_CS". // 0 => Keep the initial positon, 1 => Free aiming,  2 => Locking on.
        Transform rootTransform;
        Rigidbody thisRigidbody;
        public Vector3 Target_Position; // Referred to from "Turret_Horizontal_CS", "Cannon_Vertical_CS", "UI_Aim_Marker_Control_CS", "ReticleWheel_Control_CS".
		public Transform Target_Transform; // Referred to from "UI_Aim_Marker_Control_CS", "UI_HP_Bars_Target_CS".
        Vector3 targetOffset;
		public Rigidbody Target_Rigidbody; // Referred to from "Turret_Horizontal_CS".
		public Vector3 Adjust_Angle; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".
        const float spherecastRadius = 3.0f;
        Camera_Rotation_CS cameraRotationScript;
        public float Turret_Speed_Multiplier; // Referred to from "Turret_Horizontal_CS" and "Cannon_Vertical_CS".

        // For manual-turn.
        public float Turret_Turn_Rate; // Referred to from "Turret_Horizontal_CS".
		public float Cannon_Turn_Rate; // Referred to from "Cannon_Vertical_CS".


		protected Aiming_Control_Input_00_Base_CS inputScript;

		public bool Is_Selected; // Referred to from "UI_HP_Bars_Target_CS".


        void Start()
		{
			Initialize();
		}


        void Initialize()
		{
            rootTransform = transform.root;
            thisRigidbody = GetComponent<Rigidbody>();
            Turret_Speed_Multiplier = 1.0f;

            // Get the input type.
            if (inputType != 10)
            { // This tank is not an AI tank.
                inputType = General_Settings_CS.Input_Type;
                Use_Auto_Lead = General_Settings_CS.Use_Auto_Lead;
            }
            
            // Get the "Turret_Horizontal_CS" and "Cannon_Vertical_CS" scripts in the tank.
            turretHorizontalScripts = GetComponentsInChildren<Turret_Horizontal_CS>();
            cannonVerticalScripts = GetComponentsInChildren<Cannon_Vertical_CS>();

            // Get the "Camera_Rotation_CS" script in the tank.
            cameraRotationScript = transform.parent.GetComponentInChildren<Camera_Rotation_CS>();

            // Set the input script.
            Set_Input_Script(inputType);

            // Prepare the input script.
            if (inputScript != null)
            {
                inputScript.Prepare(this);
            }
        }


        protected virtual void Set_Input_Script(int type)
        {
            switch (type)
            {
                case 0: // Mouse + Keyboard (Stepwise)
                case 1: // Mouse + Keyboard (Pressing)
                    inputScript = gameObject.AddComponent<Aiming_Control_Input_01_Mouse_Keyboard_CS>();
                    break;

                case 2: // GamePad (Single stick)
                    inputScript = gameObject.AddComponent<Aiming_Control_Input_02_For_Single_Stick_Drive_CS>();
                    break;

                case 3: // GamePad (Twin sticks)
                    inputScript = gameObject.AddComponent<Aiming_Control_Input_03_For_Twin_Sticks_Drive_CS>();
                    break;

                case 4: // GamePad (Triggers)
                    inputScript = gameObject.AddComponent<Aiming_Control_Input_04_For_Triggers_Drive_CS>();
                    break;

                case 10: // AI
                    // The order is sent from "AI_CS".
                    break;
            }
        }


        void Update()
		{
			if (Is_Selected == false)
            {
				return;
			}

			if (inputScript != null)
            {
				inputScript.Get_Input();
			}
		}


        void FixedUpdate()
        {
            // Update the target position.
            if (Target_Transform)
            {
                Update_Target_Position();
            }
            else
            {
                Target_Position += thisRigidbody.velocity * Time.fixedDeltaTime;
            }
        }


        void Update_Target_Position()
        {
            // Check the target is living.
            if (Target_Transform.root.tag == "Finish")
            { // The target has been destroyed.
                Target_Transform = null;
                Target_Rigidbody = null;
                return;
            }

            // Update the target position.
            Target_Position = Target_Transform.position + (Target_Transform.forward * targetOffset.z) + (Target_Transform.right * targetOffset.x) + (Target_Transform.up * targetOffset.y);
        }


        public void Switch_Mode ()
        { // Called also from "Aiming_Control_Input_##_###".
            switch (Mode)
            {
                case 0: // Keep the initial positon.
                    Target_Transform = null;
                    Target_Rigidbody = null;
                    for (int i = 0; i < turretHorizontalScripts.Length; i++)
                    {
                        turretHorizontalScripts[i].Stop_Tracking();
                    }
                    for (int i = 0; i < cannonVerticalScripts.Length; i++)
                    {
                        cannonVerticalScripts[i].Stop_Tracking();
                    }
                    break;

                case 1: // Free aiming.
                    Target_Transform = null;
                    Target_Rigidbody = null;
                    for (int i = 0; i < turretHorizontalScripts.Length; i++)
                    {
                        turretHorizontalScripts[i].Start_Tracking();
                    }
                    for (int i = 0; i < cannonVerticalScripts.Length; i++)
                    {
                        cannonVerticalScripts[i].Start_Tracking();
                    }
                    break;

                case 2: // Locking on.
                    for (int i = 0; i < turretHorizontalScripts.Length; i++)
                    {
                        turretHorizontalScripts[i].Start_Tracking();
                    }
                    for (int i = 0; i < cannonVerticalScripts.Length; i++)
                    {
                        cannonVerticalScripts[i].Start_Tracking();
                    }
                    break;
            }
        }


        public void Cast_Ray_Lock(Vector3 screenPos)
        { // Called from "Aiming_Control_Input_##_###".
            // Find a target by casting a ray from the camera.
            var mainCamera = Camera.main;
            var ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 2048.0f, Layer_Settings_CS.Aiming_Layer_Mask))
            {
                var colliderTransform = raycastHit.collider.transform;
                if (colliderTransform.root != rootTransform)
                { // The hit collider is not itself.

                    // Check the rigidbody.
                    // (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHit.transform'.
                    var targetRigidbody = raycastHit.rigidbody;
                    if (targetRigidbody)
                    { // The target has a rigidbody.
                        Target_Transform = colliderTransform; // Set the hit collider as the target.
                        targetOffset = Target_Transform.InverseTransformPoint(raycastHit.point);
                        if (Target_Transform.localScale != Vector3.one)
                        { // for "Armor_Collider".
                            targetOffset.x *= Target_Transform.localScale.x;
                            targetOffset.y *= Target_Transform.localScale.y;
                            targetOffset.z *= Target_Transform.localScale.z;
                        }
                        Target_Rigidbody = targetRigidbody;
                    }
                    else
                    { // The target does not have rigidbody.
                        Target_Transform = null;
                        Target_Rigidbody = null;
                        Target_Position = raycastHit.point;
                    }
                    Mode = 2; // Lock on.
                    Switch_Mode();
                    return;
                }
                else
                { // The ray hits itself.
                    Target_Transform = null;
                    Target_Rigidbody = null;
                    Mode = 0; // Keep the initial position.
                    Switch_Mode();
                    return;
                }
            }
            else
            { // The ray does not hit anythig.
                Target_Transform = null;
                Target_Rigidbody = null;
                screenPos.z = 1024.0f;
                Target_Position = mainCamera.ScreenToWorldPoint(screenPos);
                Mode = 2; // Lock on.
                Switch_Mode();
                return;
            }
        }


        public void Cast_Ray_Free(Vector3 screenPos)
        { // Called from "Aiming_Control_Input_##_###".
            // Find a target by casting a ray from the camera.
            var mainCamera = Camera.main;
            var ray = mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 2048.0f, Layer_Settings_CS.Aiming_Layer_Mask))
            {
                var colliderTransform = raycastHit.collider.transform;
                if (colliderTransform.root != rootTransform && colliderTransform.root.tag != "Finish")
                { // The hit collider is not itself, and is not destroyed.

                    // Check the rigidbody.
                    // (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHit.transform'.
                    var targetRigidbody = raycastHit.rigidbody;
                    if (targetRigidbody)
                    { // The target has a rigidbody.
                        // Set the hit collider as the target.
                        Target_Transform = colliderTransform;
                        targetOffset = Target_Transform.InverseTransformPoint(raycastHit.point);
                        if (Target_Transform.localScale != Vector3.one)
                        { // for "Armor_Collider".
                            targetOffset.x *= Target_Transform.localScale.x;
                            targetOffset.y *= Target_Transform.localScale.y;
                            targetOffset.z *= Target_Transform.localScale.z;
                        }
                        Target_Rigidbody = targetRigidbody;
                        Target_Position = raycastHit.point;
                        return;
                    } // The target does not have rigidbody.
                } // The ray hits itself, or the target is already dead.
            } // The ray does not hit anythig.

            // Set the position through this tank.
            Target_Transform = null;
            Target_Rigidbody = null;
            screenPos.z = 64.0f;
            Target_Position = mainCamera.ScreenToWorldPoint(screenPos);
        }


        public void Reticle_Aiming(Vector3 screenPos, int thisRelationship)
        { // Called from "Aiming_Control_Input_##_###".
            // Find a target by casting a sphere from the camera.
            var ray = Camera.main.ScreenPointToRay(screenPos);
            var raycastHits = Physics.SphereCastAll(ray, spherecastRadius, 2048.0f, Layer_Settings_CS.Aiming_Layer_Mask);
            for (int i = 0; i < raycastHits.Length; i++)
            {
                Transform colliderTransform = raycastHits[i].collider.transform;
                if (colliderTransform.root != rootTransform && colliderTransform.root.tag != "Finish")
                { // The hit collider is not itself, and is not destroyed.

                    // Check the rigidbody.
                    // (Note.) When the hit collider has no rigidbody and its parent has a rigidbody, the parent's transform is set as 'RaycastHits.transform'.
                    var targetRigidbody = raycastHits[i].rigidbody;
                    if (targetRigidbody == null)
                    {
                        continue;
                    }

                    // Check the layer.
                    if (targetRigidbody.gameObject.layer != Layer_Settings_CS.Body_Layer)
                    { // The target is not a MainBody. >> It might be a track piece.
                        continue;
                    }

                    // Check the relationship.
                    var idScript = raycastHits[i].transform.GetComponentInParent<ID_Settings_CS>();
                    if (idScript && idScript.Relationship == thisRelationship)
                    { // The target is a friend.
                        continue;
                    }

                    // Check the obstacle.
                    if (Physics.Linecast(ray.origin, raycastHits[i].point, out RaycastHit raycastHit, Layer_Settings_CS.Aiming_Layer_Mask))
                    { // The target is obstructed by anything.
                        if (raycastHit.transform.root != rootTransform)
                        { // The obstacle is not itself.
                            continue;
                        }
                    }

                    // Set the MainBody as the target.
                    Target_Transform = raycastHits[i].transform;
                    targetOffset = Vector3.zero;
                    targetOffset.y = 0.5f;
                    Target_Rigidbody = targetRigidbody;
                    Target_Position = raycastHits[i].point;
                    Adjust_Angle = Vector3.zero;
                    return;
                }
            } // Target with a rigidbody cannot be found.
        }


        public void Auto_Lock (int direction, int thisRelationship)
        { // Called from "Aiming_Control_Input_##_###". (0 = Left, 1 = Right, 2 = Front)

            // Check the "AI_Headquaters_CS" is set in the scene.
            if (AI_Headquaters_CS.Instance == null)
            {
                return;
            }

            // Get the base angle to detect the new target.
            float baseAng;
            var mainCamera = Camera.main;
            if (direction != 2 && Target_Transform)
            {
                Vector3 currentLocalPos = mainCamera.transform.InverseTransformPoint(Target_Position);
                baseAng = Vector2.Angle(Vector2.up, new Vector2(currentLocalPos.x, currentLocalPos.z)) * Mathf.Sign(currentLocalPos.x);
            }
            else
            {
                baseAng = 0.0f;
            }

            // Get the tank list from the "AI_Headquaters_Helper_CS".
            List<AI_Headquaters_Helper_CS> enemyTankList;
            if (thisRelationship == 0)
            {
                enemyTankList = AI_Headquaters_CS.Instance.Hostile_Tanks_List;
            }
            else{
                enemyTankList = AI_Headquaters_CS.Instance.Friendly_Tanks_List;
            }
            
            // Find a new target.
            int targetIndex = 0;
            int oppositeTargetIndex = 0;
            float minAng = 180.0f;
            float maxAng = 0.0f;
            for (int i = 0; i < enemyTankList.Count; i++)
            {
                if (enemyTankList[i].Body_Transform.root.tag == "Finish" || (Target_Transform && Target_Transform == enemyTankList[i].Body_Transform))
                { // The tank is dead, or the tank is the same as the current target.
                    continue;
                }
                Vector3 localPos = mainCamera.transform.InverseTransformPoint(enemyTankList[i].Body_Transform.position);
                float tempAng = Vector2.Angle(Vector2.up, new Vector2(localPos.x, localPos.z)) * Mathf.Sign(localPos.x);
                float deltaAng = Mathf.Abs(Mathf.DeltaAngle(tempAng, baseAng));
                if ((direction == 0 && tempAng > baseAng) || (direction == 1 && tempAng < baseAng))
                { // On the opposite side.
                    if (deltaAng > maxAng)
                    {
                        oppositeTargetIndex = i;
                        maxAng = deltaAng;
                    }
                    continue;
                }
                if (deltaAng < minAng)
                {
                    targetIndex = i;
                    minAng = deltaAng;
                }
            }

            if (minAng != 180.0f)
            { // Target is found on the indicated side.
                Target_Transform = enemyTankList[targetIndex].Body_Transform;
                Target_Rigidbody = Target_Transform.GetComponent<Rigidbody>();
                targetOffset = Vector3.zero;
                targetOffset.y = 0.5f;
                Mode = 2; // Lock on.
                Switch_Mode();
            }
            else if (maxAng != 0.0f)
            { // Target is not found on the indicated side, but it could be found on the opposite side.
                Target_Transform = enemyTankList[oppositeTargetIndex].Body_Transform;
                Target_Rigidbody = Target_Transform.GetComponent<Rigidbody>();
                targetOffset = Vector3.zero;
                targetOffset.y = 0.5f;
                Mode = 2; // Lock on.
                Switch_Mode();
            }

            if (Target_Transform)
            {
                StartCoroutine("Send_Target_Position");
            }
        }


        IEnumerator Send_Target_Position ()
		{
            // Send the target position to the "Camera_Rotation_CS" in the 'Camera_Pivot' object.
            if (cameraRotationScript)
            {
                yield return new WaitForFixedUpdate();
                cameraRotationScript.Look_At_Target(Target_Transform.position);
            }
        }


        public void AI_Lock_On (Transform tempTransform)
		{ // Called from "AI_CS".
			Target_Transform = tempTransform;
			Target_Rigidbody = Target_Transform.GetComponent <Rigidbody>();
			Mode = 2;
			Switch_Mode ();
		}


        public void AI_Lock_Off ()
		{ // Called from "AI_CS".
			Target_Transform = null;
            Target_Rigidbody = null;
            Mode = 0;
			Switch_Mode ();
		}


        public void AI_Random_Offset ()
		{ // Called from "Cannon_Fire".

            // Set the new offset.
			Vector3 newOffset;
			newOffset.x = Random.Range (-0.5f, 0.5f);
			newOffset.y = Random.Range ( 0.0f, 1.0f);
			newOffset.z = Random.Range (-1.0f, 1.0f);
			targetOffset = newOffset;
            
            // Set the new aiming blur.
            Aiming_Blur_Multiplier = Random.Range(0.5f, 1.5f);
        }


        void Get_AI_CS(AI_CS aiScript)
		{ // Called from "AI_CS".
			inputType = 10;
            Use_Auto_Turn = true;
            Use_Auto_Lead = true;
            Aiming_Blur_Multiplier = 1.0f;
            OpenFire_Angle = aiScript.Fire_Angle;
        }


        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            this.Is_Selected = isSelected;

            if (isSelected == false)
            {
                return;
            } // This tank is selected.

            // Send this reference to the "UI_HP_Bars_Target_CS" in the scene.
            if (UI_HP_Bars_Target_CS.Instance)
            {
                UI_HP_Bars_Target_CS.Instance.Get_Aiming_Script(this);
            }
        }


        void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS".
			Destroy (inputScript as Object);
			Destroy (this);
		}


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}
