﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

    [CustomEditor(typeof(Drive_Control_CS))]
    public class Drive_Control_CSEditor : Editor
    {

        SerializedProperty TorqueProp;
        SerializedProperty Max_SpeedProp;
        SerializedProperty Turn_Brake_DragProp;
        SerializedProperty Switch_Direction_LagProp;
        SerializedProperty Allow_Pivot_TurnProp;
        SerializedProperty Pivot_Turn_RateProp;

        SerializedProperty Acceleration_FlagProp;
        SerializedProperty Acceleration_TimeProp;
        SerializedProperty Deceleration_TimeProp;
        SerializedProperty Acceleration_CurveProp;

        SerializedProperty Torque_LimitterProp;
        SerializedProperty Max_Slope_AngleProp;

        SerializedProperty Parking_Brake_VelocityProp;
        SerializedProperty Parking_Brake_Angular_VelocityProp;

        SerializedProperty Use_AntiSlipProp;
        SerializedProperty Ray_DistanceProp;

        SerializedProperty Use_DownforceProp;
        SerializedProperty DownforceProp;
        SerializedProperty Downforce_CurveProp;


        void OnEnable()
        {
            TorqueProp = serializedObject.FindProperty("Torque");
            Max_SpeedProp = serializedObject.FindProperty("Max_Speed");
            Turn_Brake_DragProp = serializedObject.FindProperty("Turn_Brake_Drag");
            Switch_Direction_LagProp = serializedObject.FindProperty("Switch_Direction_Lag");
            Allow_Pivot_TurnProp = serializedObject.FindProperty("Allow_Pivot_Turn");
            Pivot_Turn_RateProp = serializedObject.FindProperty("Pivot_Turn_Rate");

            Acceleration_FlagProp = serializedObject.FindProperty("Acceleration_Flag");
            Acceleration_TimeProp = serializedObject.FindProperty("Acceleration_Time");
            Deceleration_TimeProp = serializedObject.FindProperty("Deceleration_Time");
            Acceleration_CurveProp = serializedObject.FindProperty("Acceleration_Curve");

            Torque_LimitterProp = serializedObject.FindProperty("Torque_Limitter");
            Max_Slope_AngleProp = serializedObject.FindProperty("Max_Slope_Angle");

            Parking_Brake_VelocityProp = serializedObject.FindProperty("Parking_Brake_Velocity");
            Parking_Brake_Angular_VelocityProp = serializedObject.FindProperty("Parking_Brake_Angular_Velocity");

            Use_AntiSlipProp = serializedObject.FindProperty("Use_AntiSlip");
            Ray_DistanceProp = serializedObject.FindProperty("Ray_Distance");

            Use_DownforceProp = serializedObject.FindProperty("Use_Downforce");
            DownforceProp = serializedObject.FindProperty("Downforce");
            Downforce_CurveProp = serializedObject.FindProperty("Downforce_Curve");
        }


        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (Use_AntiSlipProp.boolValue == true)
                {
                    Debug_Ray_Distance();
                }
                return;
            }

            serializedObject.Update();

            GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Driving Wheels settings", MessageType.None, true);
            EditorGUILayout.Slider(TorqueProp, 0.0f, 500000.0f, "Torque");
            EditorGUILayout.Slider(Max_SpeedProp, 0.0f, 50.0f, "Maximum Speed");
            EditorGUILayout.Slider(Turn_Brake_DragProp, 0.0f, 5000.0f, "Turn Brake Drag");
            EditorGUILayout.Slider(Switch_Direction_LagProp, 0.0f, 5.0f, "Switch Direction Lag");
            Allow_Pivot_TurnProp.boolValue = EditorGUILayout.Toggle("Allow Pivot Turn", Allow_Pivot_TurnProp.boolValue);
            EditorGUILayout.Slider(Pivot_Turn_RateProp, 0.01f, 1.0f, "Pivot Turn Rate");

            EditorGUILayout.Space();
            Acceleration_FlagProp.boolValue = EditorGUILayout.Toggle("Acceleration", Acceleration_FlagProp.boolValue);
            if (Acceleration_FlagProp.boolValue)
            {
                EditorGUILayout.Slider(Acceleration_TimeProp, 0.01f, 30.0f, "Acceleration Time");
                EditorGUILayout.Slider(Deceleration_TimeProp, 0.01f, 30.0f, "Deceleration Time");
                Acceleration_CurveProp.animationCurveValue = EditorGUILayout.CurveField("Acceleration Curve", Acceleration_CurveProp.animationCurveValue, Color.red, new Rect(Vector2.zero, new Vector2(1.0f, 1.0f)));
            }

            EditorGUILayout.Space();
            Torque_LimitterProp.boolValue = EditorGUILayout.Toggle("Torque Limitter", Torque_LimitterProp.boolValue);
            if (Torque_LimitterProp.boolValue)
            {
                EditorGUILayout.Slider(Max_Slope_AngleProp, 0.0f, 90.0f, "Max Slope Angle");
            }
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Parking Brake settings", MessageType.None, true);
            EditorGUILayout.Slider(Parking_Brake_VelocityProp, 0.0f, 8.0f, "Work Velocity");
            EditorGUILayout.Slider(Parking_Brake_Angular_VelocityProp, 0.0f, 8.0f, "Work Angular Velocity");
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Anti Slipping settings", MessageType.None, true);
            Use_AntiSlipProp.boolValue = EditorGUILayout.Toggle("Use Anti-Slipping", Use_AntiSlipProp.boolValue);
            if (Use_AntiSlipProp.boolValue)
            {
                EditorGUILayout.Slider(Ray_DistanceProp, 0.0f, 10.0f, "Ray Distance");
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Downforce settings", MessageType.None, true);
            Use_DownforceProp.boolValue = EditorGUILayout.Toggle("Add Downforce", Use_DownforceProp.boolValue);
            if (Use_DownforceProp.boolValue)
            {
                EditorGUILayout.Slider(DownforceProp, 0.0f, 1000000.0f, "Downforce");
                Downforce_CurveProp.animationCurveValue = EditorGUILayout.CurveField("Downforce Curve", Downforce_CurveProp.animationCurveValue, Color.red, new Rect(Vector2.zero, new Vector2(1.0f, 1.0f)));
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }


        void Debug_Ray_Distance()
        {
            GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

            Drive_Control_CS controlScript = target as Drive_Control_CS;
            Transform thisTransform = controlScript.transform;
            Ray ray = new Ray
            {
                origin = thisTransform.position,
                direction = -thisTransform.up
            };
            RaycastHit rayCastHit;
            if (Physics.Raycast(ray, out rayCastHit, 10.0f, Layer_Settings_CS.Anti_Slipping_Layer_Mask) == true)
            { // The ray hits the ground.
                EditorGUILayout.HelpBox("Ray Hit Distance = " + rayCastHit.distance, MessageType.None, false);
            }
            else
            { // The ray does not hit anything.
                EditorGUILayout.HelpBox("The ground is too far.", MessageType.None, false);
            }
        }

    }

}