using Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.InputSystem;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
	private SerializedProperty _movementController;
	private SerializedProperty _gunController;
	private SerializedProperty _playerInput;
	private SerializedProperty _aimHeight;
	private SerializedProperty _minAimRadius;
	private SerializedProperty _useAimDistance;
	private SerializedProperty _aimDistance;
	private SerializedProperty _aimEvent;
	private SerializedProperty _detectEvent;

	private PlayerController controller;
	private AnimBool useAimDistanceFlag;

	private bool eventsGroupUnfolded;
	private string eventsGroupText = "Events";

	private void OnEnable()
	{
		_movementController = serializedObject.FindProperty("movementController");
		_gunController = serializedObject.FindProperty("gunController");
		_playerInput = serializedObject.FindProperty("playerInput");
		_aimHeight = serializedObject.FindProperty("aimHeight");
		_minAimRadius = serializedObject.FindProperty("minAimRadius");
		_useAimDistance = serializedObject.FindProperty("useAimDistance");
		_aimDistance = serializedObject.FindProperty("aimDistance");
		_aimEvent = serializedObject.FindProperty("aimEvent");
		_detectEvent = serializedObject.FindProperty("detectEvent");

		controller = target as PlayerController;
		useAimDistanceFlag = new AnimBool(true, Repaint);
	}

	public override void OnInspectorGUI()
	{
		//serializedObject.Update();

		EditorGUI.BeginChangeCheck();

		EditorGUILayout.PropertyField(_movementController);
		EditorGUILayout.PropertyField(_gunController);
		EditorGUILayout.PropertyField(_playerInput);
		EditorGUILayout.PropertyField(_aimHeight);
		EditorGUILayout.PropertyField(_minAimRadius);
		EditorGUILayout.PropertyField(_useAimDistance);

		useAimDistanceFlag.target = _useAimDistance.boolValue;
		if (EditorGUILayout.BeginFadeGroup(useAimDistanceFlag.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(_aimDistance);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();

		eventsGroupUnfolded = EditorGUILayout.Foldout(eventsGroupUnfolded, eventsGroupText, toggleOnLabelClick: true);
		if (eventsGroupUnfolded)
		{
			EditorGUILayout.PropertyField(_aimEvent);
			EditorGUILayout.PropertyField(_detectEvent);
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}

	private void OnSceneGUI()
	{
		Vector3 position = controller.transform.position;
		position.y = _aimHeight.floatValue;

		Handles.color = Color.red;
		Handles.DrawWireDisc(position, Vector3.up, _minAimRadius.floatValue);

		if (_useAimDistance.boolValue)
		{
			Handles.color = Color.green;
			Handles.DrawWireDisc(position, Vector3.up, _minAimRadius.floatValue + _aimDistance.floatValue);
		}
	}
}