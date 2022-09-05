using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kogane.Internal
{
    internal sealed class EventSystemStatusWindow : EditorWindow
    {
        private string   m_filteringText;
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

            m_filteringText = EditorGUILayout.TextField( "Filter", m_filteringText );

            if ( GUILayout.Button( "Copy", EditorStyles.toolbarButton ) )
            {
                EditorGUIUtility.systemCopyBuffer = Regex.Replace( status, "<.*?>", string.Empty );
            }

            using var scrollViewScope = new EditorGUILayout.ScrollViewScope( m_scrollPosition );

            var texts = string.IsNullOrWhiteSpace( m_filteringText )
                    ? status
                    : string.Join( "\n", status.Split( '\n' ).Where( x => x.Contains( m_filteringText, StringComparison.OrdinalIgnoreCase ) ) )
                ;

            EditorGUILayout.TextArea( texts, TextAreaStyle );

            m_scrollPosition = scrollViewScope.scrollPosition;
        }

        private void Update()
        {
            Repaint();
        }
    }
}