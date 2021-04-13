using OctanGames.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHeathEditor : Editor
{
	private SerializedProperty _startingHealth;
	private SerializedProperty _deadHeight;
	private SerializedProperty _deadEvent;
	private SerializedProperty _hitDamageEvent;

	private bool eventsGroupUnfolded;
	private string eventsGroupText = "Events";

	private void OnEnable()
	{
		_startingHealth = serializedObject.FindProperty("startingHealth");
		_deadHeight = serializedObject.FindProperty("deadHeight");
		_deadEvent = serializedObject.FindProperty("DeathEvent");
		_hitDamageEvent = serializedObject.FindProperty("HitDamageEvent");
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.PropertyField(_startingHealth);
		EditorGUILayout.PropertyField(_deadHeight);

		eventsGroupUnfolded = EditorGUILayout.Foldout(eventsGroupUnfolded, eventsGroupText, toggleOnLabelClick: true);
		if (eventsGroupUnfolded)
		{
			EditorGUILayout.PropertyField(_deadEvent);
			EditorGUILayout.PropertyField(_hitDamageEvent);
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}