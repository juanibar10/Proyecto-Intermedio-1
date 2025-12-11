/*using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentSpeedManager))]
public class EnvironmentSpeedManagerEditor : Editor
{
    private EnvironmentSpeedManager manager;

    // Animated normalized values [0..1]
    private float animSpeed;
    private float animBg;
    private float animItem;
    private float animProj;

    // History for sparkline (normalized speed)
    private readonly List<float> speedHistory = new List<float>();
    private const int MaxHistoryCount = 800; // ~13s at 60 fps

    // Section foldouts
    private static bool showGeneral   = true;
    private static bool showRuntime   = true;
    private static bool showAnalytics = true;
    private static bool showTools     = true;

    // Styles
    private GUIStyle titleStyle;
    private GUIStyle subtitleStyle;
    private GUIStyle sectionTitleStyle;
    private GUIStyle cardStyle;
    private GUIStyle chipStyle;
    private GUIStyle barLabelLeftStyle;
    private GUIStyle barLabelRightStyle;
    private GUIStyle foldoutStyle;
    private GUIStyle buttonStylePrimary;
    private GUIStyle buttonStyleSecondary;

    // Colors
    private Color cardBackgroundColor;
    private Color cardBorderColor;
    private Color barBackgroundColor;
    private Color barFillColor;
    private Color sparklineLineColor;
    private Color sparklineFillColor;
    private Color accentColor;
    private Color accentSoftColor;

    private void OnEnable()
    {
        manager = (EnvironmentSpeedManager)target;
        titleStyle = null;        // force style reinit
        speedHistory.Clear();
    }

    private void InitStyles()
    {
        if (titleStyle != null)
            return;

        bool pro = EditorGUIUtility.isProSkin;

        // Palette (Apple-like, soft, blue accent)
        accentColor       = new Color(0.15f, 0.55f, 0.95f);
        accentSoftColor   = new Color(0.15f, 0.55f, 0.95f, 0.08f);
        cardBackgroundColor = pro ? new Color(0.16f, 0.17f, 0.19f)
                                  : new Color(0.96f, 0.96f, 0.98f);
        cardBorderColor   = pro ? new Color(1f, 1f, 1f, 0.04f)
                                : new Color(0f, 0f, 0f, 0.06f);
        barBackgroundColor = pro ? new Color(0.10f, 0.10f, 0.11f)
                                 : new Color(0.86f, 0.86f, 0.89f);
        barFillColor      = accentColor;
        sparklineLineColor = accentColor;
        sparklineFillColor = accentSoftColor;

        // Typography
        titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = pro ? new Color(0.92f, 0.97f, 1f) : new Color(0.08f, 0.25f, 0.45f) }
        };

        subtitleStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = pro ? new Color(0.72f, 0.72f, 0.76f) : new Color(0.35f, 0.35f, 0.40f) }
        };

        sectionTitleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = pro ? new Color(0.92f, 0.92f, 0.95f) : new Color(0.20f, 0.20f, 0.24f) }
        };

        barLabelLeftStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = pro ? new Color(0.80f, 0.80f, 0.84f) : new Color(0.30f, 0.30f, 0.34f) }
        };

        barLabelRightStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleRight,
            normal = { textColor = pro ? new Color(0.88f, 0.88f, 0.92f) : new Color(0.22f, 0.22f, 0.26f) }
        };

        foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontSize = 11,
            fontStyle = FontStyle.Bold
        };

        // Cards (flat, no heavy borders)
        cardStyle = new GUIStyle()
        {
            padding = new RectOffset(16, 16, 12, 12),
            margin  = new RectOffset(0, 0, 4, 8),
            normal  = { background = MakeTex(cardBackgroundColor) }
        };

        // Buttons
        buttonStylePrimary = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 26,
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                textColor = Color.white
            }
        };

        buttonStyleSecondary = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 26,
            alignment = TextAnchor.MiddleCenter
        };

        // Chips (LIVE / IDLE)
        chipStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            padding = new RectOffset(6, 6, 2, 2),
            normal = { textColor = Color.white }
        };
    }

    public override void OnInspectorGUI()
    {
        InitStyles();
        serializedObject.Update();

        DrawHeader();
        GUILayout.Space(4);

        DrawSectionCard("General", ref showGeneral, DrawGeneralSection);
        DrawSectionCard("Runtime", ref showRuntime, DrawRuntimeSection);
        DrawSectionCard("Analytics", ref showAnalytics, DrawAnalyticsSection);
        DrawSectionCard("Tools & Presets", ref showTools, DrawToolsSection);

        serializedObject.ApplyModifiedProperties();

        if (Application.isPlaying)
            Repaint();
    }

    // ----------------------------------------------------------------------
    // HEADER (simple, claro, sin rects raros)
    // ----------------------------------------------------------------------
    private void DrawHeader()
    {
        GUILayout.Space(6);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(4);

        // Circular icon
        var iconRect = GUILayoutUtility.GetRect(24, 24);
        DrawCircularIcon(iconRect);

        GUILayout.Space(6);

        EditorGUILayout.BeginVertical();
        GUILayout.Label("Environment Speed Manager", titleStyle);
        GUILayout.Label("Monitor and adjust environment scrolling speeds.", subtitleStyle);
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        // LIVE / IDLE chip
        var stateText = Application.isPlaying ? "LIVE" : "IDLE";
        var stateColor = Application.isPlaying
            ? new Color(0.20f, 0.70f, 0.40f)
            : new Color(0.55f, 0.55f, 0.60f);

        DrawChip(stateText, stateColor);

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);

        // Divider line
        var lineRect = GUILayoutUtility.GetRect(0, 1, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(lineRect, new Color(1f, 1f, 1f, EditorGUIUtility.isProSkin ? 0.06f : 0.15f));
        GUILayout.Space(4);
    }

    private void DrawCircularIcon(Rect rect)
    {
        var center = rect.center;
        float radiusOuter = rect.width * 0.5f;
        float radiusInner = radiusOuter * 0.65f;

        Handles.BeginGUI();
        Handles.color = accentSoftColor;
        Handles.DrawSolidDisc(center, Vector3.forward, radiusOuter);
        Handles.color = accentColor;
        Handles.DrawSolidDisc(center, Vector3.forward, radiusInner);
        Handles.EndGUI();
    }

    private void DrawChip(string text, Color background)
    {
        var prev = GUI.backgroundColor;
        GUI.backgroundColor = background;
        GUILayout.Box(text, chipStyle);
        GUI.backgroundColor = prev;
    }

    // ----------------------------------------------------------------------
    // SECTION CARD
    // ----------------------------------------------------------------------
    private void DrawSectionCard(string title, ref bool open, System.Action drawer)
    {
        EditorGUILayout.BeginVertical(cardStyle);
        {
            // Border subtle
            var r = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true));
            r.y -= 12;
            r.height = 1;
            EditorGUI.DrawRect(r, cardBorderColor);

            EditorGUILayout.BeginHorizontal();
            open = EditorGUILayout.Foldout(open, title, true, foldoutStyle);
            EditorGUILayout.EndHorizontal();

            if (open)
            {
                GUILayout.Space(4);
                drawer?.Invoke();
            }
        }
        EditorGUILayout.EndVertical();
    }

    // ----------------------------------------------------------------------
    // GENERAL SECTION
    // ----------------------------------------------------------------------
    private void DrawGeneralSection()
    {
        EditorGUILayout.LabelField("Base parameters", sectionTitleStyle);
        GUILayout.Space(2);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("startSpeed"), new GUIContent("Start Speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSpeed"),   new GUIContent("Max Speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("acceleration"), new GUIContent("Acceleration"));

        GUILayout.Space(4);
        using (new EditorGUI.DisabledScope(true))
        {
            float runtime = Application.isPlaying ? manager.GetCurrentSpeed() : 0f;
            EditorGUILayout.FloatField("Current Runtime Speed", runtime);
        }
    }

    // ----------------------------------------------------------------------
    // RUNTIME SECTION (bars + gauge)
    // ----------------------------------------------------------------------
    private void DrawRuntimeSection()
    {
        EditorGUILayout.LabelField("Live monitoring", sectionTitleStyle);
        GUILayout.Space(2);

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to see live values.", MessageType.Info);
            return;
        }

        UpdateAnimatedValues();

        EditorGUILayout.BeginHorizontal();

        // Bars
        EditorGUILayout.BeginVertical();
        DrawSpeedBar("Current",     animSpeed);
        DrawSpeedBar("Background",  animBg);
        DrawSpeedBar("Items",       animItem);
        DrawSpeedBar("Projectiles", animProj);
        EditorGUILayout.EndVertical();

        // Gauge
        var gaugeRect = GUILayoutUtility.GetRect(90, 90);
        DrawGauge(gaugeRect, animSpeed);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSpeedBar(string label, float normalized)
    {
        var rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));

        float pct = Mathf.Clamp01(normalized);

        // bar background
        var barRect = new Rect(rect.x, rect.y + 12, rect.width, 6f);
        EditorGUI.DrawRect(barRect, barBackgroundColor);

        // fill
        var fillRect = new Rect(barRect.x, barRect.y, barRect.width * pct, barRect.height);
        EditorGUI.DrawRect(fillRect, barFillColor);

        // labels
        var topRect = new Rect(rect.x, rect.y, rect.width, 12);
        EditorGUI.LabelField(topRect, label, barLabelLeftStyle);
        EditorGUI.LabelField(topRect, $"{pct * 100f:0}%", barLabelRightStyle);
    }

    private void DrawGauge(Rect rect, float normalized)
    {
        var center = rect.center;
        float r = Mathf.Min(rect.width, rect.height) * 0.45f;
        float t = Mathf.Clamp01(normalized);

        Handles.BeginGUI();
        // base circle
        Handles.color = barBackgroundColor;
        Handles.DrawSolidDisc(center, Vector3.forward, r);

        // arc
        Handles.color = accentColor;
        float startAngle = 135f;
        float endAngle = Mathf.Lerp(startAngle, 45f, t);
        Handles.DrawSolidArc(center, Vector3.forward, AngleToDir(startAngle), startAngle - endAngle, r * 0.9f);
        Handles.EndGUI();

        var labelRect = new Rect(rect.x, rect.y + rect.height * 0.35f, rect.width, 20);
        EditorGUI.LabelField(labelRect, $"{t * 100f:0}%", new GUIStyle(titleStyle)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14
        });

        var subRect = new Rect(rect.x, rect.y + rect.height * 0.60f, rect.width, 14);
        EditorGUI.LabelField(subRect, "Global speed", new GUIStyle(subtitleStyle)
        {
            alignment = TextAnchor.MiddleCenter
        });
    }

    private Vector3 AngleToDir(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private void UpdateAnimatedValues()
    {
        float max = Mathf.Max(manager.GetMaxSpeed(), 0.0001f);

        float tCurrent = Mathf.Clamp01(manager.GetCurrentSpeed()    / max);
        float tBg      = Mathf.Clamp01(manager.BackgroundSpeed      / max);
        float tItem    = Mathf.Clamp01(manager.ItemSpeed            / max);
        float tProj    = Mathf.Clamp01(manager.ProjectileSpeed      / max);

        float smooth = 0.15f;
        animSpeed = Mathf.Lerp(animSpeed, tCurrent, smooth);
        animBg    = Mathf.Lerp(animBg,    tBg,      smooth);
        animItem  = Mathf.Lerp(animItem,  tItem,    smooth);
        animProj  = Mathf.Lerp(animProj,  tProj,    smooth);

        // history for sparkline
        speedHistory.Add(tCurrent);
        if (speedHistory.Count > MaxHistoryCount)
            speedHistory.RemoveAt(0);
    }

    // ----------------------------------------------------------------------
    // ANALYTICS SECTION (Sparkline)
    // ----------------------------------------------------------------------
    private void DrawAnalyticsSection()
    {
        EditorGUILayout.LabelField("Speed history", sectionTitleStyle);
        GUILayout.Space(2);

        var rect = GUILayoutUtility.GetRect(0, 60, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, barBackgroundColor);

        if (speedHistory.Count < 2)
        {
            EditorGUI.LabelField(rect, "Collecting data...", barLabelRightStyle);
            return;
        }

        DrawSparkline(rect, speedHistory, sparklineLineColor, sparklineFillColor);

        var labelRect = new Rect(rect.x + 4, rect.y + rect.height - 14, rect.width - 8, 12);
        EditorGUI.LabelField(labelRect, "Last seconds snapshot", subtitleStyle);
    }

    private void DrawSparkline(Rect rect, List<float> values, Color lineColor, Color fillColor)
    {
        int count = values.Count;
        if (count < 2) return;

        var pts = new Vector3[count];
        float step = rect.width / Mathf.Max(count - 1, 1);

        for (int i = 0; i < count; i++)
        {
            float v = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(values[i]));
            float x = rect.x + i * step;
            float y = Mathf.Lerp(rect.yMax - 6, rect.y + 6, v);
            pts[i] = new Vector3(x, y, 0f);
        }

        // Build fill polygon
        var fillPoints = new List<Vector3>(pts);
        fillPoints.Add(new Vector3(rect.x + rect.width, rect.yMax - 6));
        fillPoints.Add(new Vector3(rect.x, rect.yMax - 6));

        Handles.BeginGUI();
        Handles.color = fillColor;
        Handles.DrawAAConvexPolygon(fillPoints.ToArray());
        Handles.color = lineColor;
        Handles.DrawAAPolyLine(2f, pts);
        Handles.EndGUI();
    }

    // ----------------------------------------------------------------------
    // TOOLS & PRESETS
    // ----------------------------------------------------------------------
    private void DrawToolsSection()
    {
        EditorGUILayout.LabelField("Quick presets", sectionTitleStyle);
        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        DrawPresetButton("Casual",   new Color(0.22f, 0.60f, 0.96f), 3f, 12f, 0.35f);
        DrawPresetButton("Normal",   new Color(0.22f, 0.75f, 0.60f), 5f, 18f, 0.50f);
        DrawPresetButton("Hardcore", new Color(0.96f, 0.65f, 0.30f), 7f, 24f, 0.70f);
        DrawPresetButton("Boss",     new Color(0.96f, 0.45f, 0.40f), 8f, 28f, 0.90f);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(8);
        EditorGUILayout.LabelField("Runtime controls", sectionTitleStyle);
        GUILayout.Space(2);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            EditorGUILayout.BeginHorizontal();

            Color prev = GUI.backgroundColor;

            // STOP (primary, red)
            GUI.backgroundColor = Application.isPlaying ? new Color(0.90f, 0.30f, 0.35f) : Color.gray;
            if (GUILayout.Button("STOP", buttonStylePrimary) && Application.isPlaying)
                manager.SendMessage("OnStop");

            // RESUME (primary, accent)
            GUI.backgroundColor = Application.isPlaying ? accentColor : Color.gray;
            if (GUILayout.Button("RESUME", buttonStylePrimary) && Application.isPlaying)
                manager.SendMessage("OnResume");

            GUI.backgroundColor = prev;

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawPresetButton(string label, Color color, float start, float max, float accel)
    {
        Color prev = GUI.backgroundColor;
        GUI.backgroundColor = color;
        if (GUILayout.Button(label, buttonStyleSecondary))
        {
            ApplyPreset(start, max, accel);
        }
        GUI.backgroundColor = prev;
    }

    private void ApplyPreset(float start, float max, float accel)
    {
        Undo.RecordObject(target, "Apply Speed Preset");

        serializedObject.FindProperty("startSpeed").floatValue = start;
        serializedObject.FindProperty("maxSpeed").floatValue   = max;
        serializedObject.FindProperty("acceleration").floatValue = accel;

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    // ----------------------------------------------------------------------
    // UTIL
    // ----------------------------------------------------------------------
    private Texture2D MakeTex(Color col)
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, col);
        tex.Apply();
        tex.hideFlags = HideFlags.HideAndDontSave;
        return tex;
    }
}
*/