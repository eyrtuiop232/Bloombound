using UnityEditor;

namespace NavMeshPlus.Extensions.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AgentOverride2d))]
    internal class AgentOverride2dEditor : Editor
    {

        void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var agent = target as AgentOverride2d;
            EditorGUILayout.LabelField("Agent Override", agent.agentOverride?.GetType().Name);
        }
    }
}
