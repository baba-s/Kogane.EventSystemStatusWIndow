using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kogane.Internal
{
    internal sealed class EventSystemStatusWindow : EditorWindow
    {
        private Vector2  m_scrollPosition;
        private GUIStyle m_textAreaStyleCache;

        private GUIStyle TextAreaStyle =>
            m_textAreaStyleCache ??= new GUIStyle( "PreOverlayLabel" )
            {
                richText  = true,
                alignment = TextAnchor.UpperLeft,
                fontStyle = FontStyle.Normal
            };

        [MenuItem( "Window/Kogane/Event System Status" )]
        private static void Open()
        {
            GetWindow<EventSystemStatusWindow>( "Event System" );
        }

        private void OnGUI()
        {
            if ( !Application.isPlaying ) return;

            var eventSystem = EventSystem.current;

            if ( eventSystem == null ) return;

            var status = eventSystem.ToString();

            if ( GUILayout.Button( "Copy", EditorStyles.toolbarButton ) )
            {
                EditorGUIUtility.systemCopyBuffer = Regex.Replace( status, "<.*?>", string.Empty );
            }

            using var scrollViewScope = new EditorGUILayout.ScrollViewScope( m_scrollPosition );

            EditorGUILayout.TextArea( status, TextAreaStyle );

            m_scrollPosition = scrollViewScope.scrollPosition;
        }

        private void Update()
        {
            Repaint();
        }
    }
}