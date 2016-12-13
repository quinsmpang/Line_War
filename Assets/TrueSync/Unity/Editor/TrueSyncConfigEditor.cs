using UnityEngine;
using UnityEditor;

namespace TrueSync {

    /**
    *  @brief Custom editor to {@link TrueSyncConfig}.
    **/
    [CustomEditor(typeof(TrueSyncConfig))]
    public class TrueSyncConfigEditor : Editor {

        public override void OnInspectorGUI() {
            serializedObject.Update();

            TrueSyncConfig settings = (TrueSyncConfig) target;
            Undo.RecordObject(settings, "Edit TrueSyncConfig");

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            settings.syncWindow = EditorGUILayout.IntField("Sync Window", settings.syncWindow);
            if (settings.syncWindow < 0) {
                settings.syncWindow = 0;
            }

            settings.rollbackWindow = EditorGUILayout.IntField("Rollback Window", settings.rollbackWindow);
            if (settings.rollbackWindow < 0) {
                settings.rollbackWindow = 0;
            }

            settings.panicWindow = EditorGUILayout.IntField("Panic Window", settings.panicWindow);
            if (settings.panicWindow < 0) {
                settings.panicWindow = 0;
            }

            settings.lockedTimeStep = EditorGUILayout.FloatField("Locked Time Step", settings.lockedTimeStep.AsFloat());
            if (settings.lockedTimeStep < 0) {
                settings.lockedTimeStep = 0;
            }

            settings.showStats = EditorGUILayout.Toggle("Show Stats", settings.showStats);

            EditorGUI.indentLevel--;

            GUILayout.Space(10);

            settings.physics2DEnabled = EditorGUILayout.BeginToggleGroup("2D Physics", settings.physics2DEnabled);

            if (settings.physics2DEnabled) {
                settings.physics3DEnabled = false;

                EditorGUI.indentLevel++;

                Vector2 gField2D = EditorGUILayout.Vector2Field("Gravity", settings.gravity2D.ToVector());
                settings.gravity2D.x = gField2D.x;
                settings.gravity2D.y = gField2D.y;

                settings.speculativeContacts2D = EditorGUILayout.Toggle("Speculative Contacts", settings.speculativeContacts2D);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndToggleGroup();

            settings.physics3DEnabled = EditorGUILayout.BeginToggleGroup("3D Physics", settings.physics3DEnabled);

            if (settings.physics3DEnabled) {
                settings.physics2DEnabled = false;

                EditorGUI.indentLevel++;

                Vector3 gField3D = EditorGUILayout.Vector3Field("Gravity", settings.gravity3D.ToVector());
                settings.gravity3D.x = gField3D.x;
                settings.gravity3D.y = gField3D.y;
                settings.gravity3D.z = gField3D.z;

                settings.speculativeContacts3D = EditorGUILayout.Toggle("Speculative Contacts", settings.speculativeContacts3D);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndToggleGroup();

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(20);

            if (GUILayout.Button("Highlight Settings")) {
                EditorGUIUtility.PingObject(target);
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }

    }

}