using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CellDataWindow))]
public class CellModelPreview : Editor
{
    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("Cell Model View");
    }

    public override void OnPreviewSettings()
    {
        GUIStyle preButton = new GUIStyle("preButton");
        GUILayout.Button("Test But", preButton);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        GUI.Box (r, "Test Custom Preview");
    }

    /*
    private PreviewRenderUtility pru;
    private MeshFilter tMf;
    private MeshRenderer tMr;

    public override bool HasPreviewGUI()
    {
        if(pru == null)
        {
            pru = new PreviewRenderUtility();

            pru.camera.transform.position = new Vector3(0, 0, -3);
            pru.camera.transform.rotation = Quaternion.identity;
        }

        tMf = target as MeshFilter;
        tMr = tMr.GetComponent<MeshRenderer>();

        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (tMr == null)
            {
                EditorGUI.DropShadowLabel(r, "Mesh Renderer Required");
            }
            else
            {
                pru.BeginPreview(r, background);

                pru.DrawMesh(tMf.sharedMesh, Matrix4x4.identity, tMr.sharedMaterial, 0);
                pru.camera.Render();

                Texture resultRender = pru.EndPreview();
                GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
            }
        }
    }

    private void OnDestroy()
    {
        pru.Cleanup();
    }
    */
}
