﻿// Copyright (C) 2019 Alexander Bogarsukov. All rights reserved.
// See the LICENSE.md file in the project root for more information.

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityFx.Outline
{
	[CustomEditor(typeof(OutlineBehaviour))]
	public class OutlineBehaviourEditor : Editor
	{
		private readonly GUIContent _outlineSettings = new GUIContent("Outline Settings", "Allows sharing of outline settings with other components. If set, overrides component-specific outline settings.");

		private OutlineBehaviour _effect;
		private bool _renderersOpened;
		private bool _camerasOpened;

		private void OnEnable()
		{
			_effect = (OutlineBehaviour)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			// 1) Outline settings.
			EditorGUI.BeginChangeCheck();

			_effect.OutlineSettings = (OutlineSettings)EditorGUILayout.ObjectField(_outlineSettings, _effect.OutlineSettings, typeof(OutlineSettings), true);

			if (_effect.OutlineSettings)
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.indentLevel += 1;
				OutlineEditorUtility.Render(_effect);
				EditorGUILayout.HelpBox(string.Format("Settings are overriden with values from {0}.", _effect.OutlineSettings.name), MessageType.Info, true);
				EditorGUI.indentLevel -= 1;
				EditorGUI.EndDisabledGroup();
			}
			else
			{
				EditorGUI.indentLevel += 1;
				OutlineEditorUtility.Render(_effect);
				EditorGUI.indentLevel -= 1;
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(_effect.gameObject);

				if (!EditorApplication.isPlayingOrWillChangePlaymode)
				{
					EditorSceneManager.MarkSceneDirty(_effect.gameObject.scene);
				}
			}

			// 2) Renderers (read-only).
			_renderersOpened = EditorGUILayout.Foldout(_renderersOpened, "Renderers", true);

			if (_renderersOpened)
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.indentLevel += 1;

				var rendererNumber = 1;

				foreach (var renderer in _effect.OutlineRenderers)
				{
					EditorGUILayout.ObjectField("#" + rendererNumber.ToString(), renderer, typeof(Renderer), true);
					rendererNumber++;
				}

				EditorGUI.indentLevel -= 1;
				EditorGUI.EndDisabledGroup();
			}

			// 3) Cameras (read-only).
			_camerasOpened = EditorGUILayout.Foldout(_camerasOpened, "Cameras", true);

			if (_camerasOpened)
			{
				EditorGUI.BeginDisabledGroup(true);
				EditorGUI.indentLevel += 1;

				var cameraNumber = 1;

				foreach (var camera in _effect.Cameras)
				{
					EditorGUILayout.ObjectField("#" + cameraNumber.ToString(), camera, typeof(Camera), true);
					cameraNumber++;
				}

				EditorGUI.indentLevel -= 1;
				EditorGUI.EndDisabledGroup();
			}
		}
	}
}
