using Assets.Scripts;
using Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovementController))]
public class MovenentControllerEditor : Editor
{
	private SerializedProperty _rigidbodyComponent;
	private SerializedProperty _movementSpeed;
	private SerializedProperty _acceleration;
	private SerializedProperty _turnSpeed;
	private SerializedProperty _velocityDependent;
	private SerializedProperty _rotateWithMovement;
	private SerializedProperty _rotationType;
	private SerializedProperty _alignToCamera;

	private AnimBool rotateWithMovemntGroup;

	private void OnEnable()
	{
		_rigidbodyComponent = serializedObject.FindProperty("rigidbodyComponent");
		_movementSpeed = serializedObject.FindProperty("movementSpeed");
		_acceleration = serializedObject.FindProperty("acceleration");
		_turnSpeed = serializedObject.FindProperty("turnSpeed");
		_velocityDependent = serializedObject.FindProperty("velocityDependent");
		_rotateWithMovement = serializedObject.FindProperty("rotateWithMovement");
		_rotationType = serializedObject.FindProperty("rotationType");
		_alignToCamera = serializedObject.FindProperty("alignToCamera");

		rotateWithMovemntGroup = new AnimBool(_rotateWithMovement.boolValue, Repaint);
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.PropertyField(_rigidbodyComponent);
		EditorGUILayout.PropertyField(_movementSpeed);
		EditorGUILayout.PropertyField(_acceleration);
		EditorGUILayout.PropertyField(_turnSpeed);
		EditorGUILayout.PropertyField(_velocityDependent);
		EditorGUILayout.PropertyField(_rotateWithMovement);

		rotateWithMovemntGroup.target = _rotateWithMovement.boolValue;
		if (EditorGUILayout.BeginFadeGroup(rotateWithMovemntGroup.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(_rotationType);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();

		EditorGUILayout.PropertyField(_alignToCamera);

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

	}
}