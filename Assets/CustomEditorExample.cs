#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomEditorExample)), CanEditMultipleObjects]
public class PlayerEditor : Editor
{
    public bool show = false;

    CustomEditorExample _playerScript;
    SerializedObject _serializedPlayerScript;
    SerializedProperty _serializedHealth;
    SerializedProperty _serializedScript;

    private void OnEnable()
    {
        _serializedScript = serializedObject.FindProperty("PlayerScript");

        _playerScript = target as CustomEditorExample;
        _serializedPlayerScript = new SerializedObject(_playerScript);
        _serializedHealth = _serializedPlayerScript.FindProperty("_health");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("CuST0m_1337 InsP3cT0r.", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.HelpBox("He-He-He. HelL0", MessageType.Error);
        EditorGUILayout.Space();

        _playerScript.Show = EditorGUILayout.Toggle(new GUIContent("�������� ���-��", "�������, ����� ������ �����-�� ����������� ����������."), _playerScript.Show);
        if (!_playerScript.Show) return;

        _playerScript.A = EditorGUILayout.IntField(new GUIContent("���������� �", "���������� ���� int"), _playerScript.A);
        _playerScript.B = EditorGUILayout.TextField(new GUIContent("���������� B:", "���������� ���� string"), _playerScript.B);
        _playerScript.C = EditorGUILayout.FloatField(new GUIContent("���������� C", "���������� ���� float"), _playerScript.C);
        EditorGUILayout.PropertyField(_serializedScript);

        _playerScript.MoveSpeed = EditorGUILayout.Slider(new GUIContent("�������� ������", "������������� �������� ������"), _playerScript.MoveSpeed, 1, 10);
        _serializedHealth.floatValue = EditorGUILayout.FloatField(new GUIContent("��������", "������� �������� ������"), _serializedHealth.floatValue);

        //Bild
        Texture2D cover = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/HP.png", typeof(Texture2D));
        float imageWidth = EditorGUIUtility.currentViewWidth;
        float imageHeight = imageWidth * cover.height / cover.width;
        Rect rect = GUILayoutUtility.GetRect(imageWidth, imageHeight);
        GUI.DrawTexture(rect, cover, ScaleMode.ScaleToFit);

        //Mark
        GUILayout.Label("By Cosyplaid", new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        _serializedPlayerScript.Update();

        //TastenBegin
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("������������ ��������", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
        {
            _serializedHealth.floatValue = 100;
            ShowHealthMessage();
        }
        if (GUILayout.Button("������� ������", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
        {
            _playerScript.ApplyDamage(20);
            ShowHealthMessage();
        }
        GUILayout.EndHorizontal();
        //TastenEnd

        void ShowHealthMessage()
        {
            if (_serializedHealth.floatValue > 0)
                Debug.Log(string.Format("Player's health is {0} now!", _serializedHealth.floatValue));
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_playerScript);
            _serializedPlayerScript.ApplyModifiedProperties();
        }
    }
}

public class CustomEditorExample : MonoBehaviour
{
    public bool Show;

    public PlayerMoveScript PlayerScript;
    public int A;
    public float C;
    public float MoveSpeed;
    public string B;

    [SerializeField] private float _health = 100;

    public void ApplyDamage(float damage) => _health -= damage;
}
#endif