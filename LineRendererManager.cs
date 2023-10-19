using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    public static LineRendererManager instance;
    public LineRenderer referanceLineRenderer;
    private List<LineRenderer> renderers = new List<LineRenderer>();
    private List<LineRenderer> pool = new List<LineRenderer>();

    private void Awake() => instance = this;

    public void ManageRenderer(List<Vector3> poses) => ManageRenderer(poses.ToArray());
    public void ManageRenderer(Vector3[] poses)
    {
        if(poses.Length <= renderers.Count && poses.Length > 1)
        {
            LineRenderer[] renderersToRemove = renderers.GetRange(poses.Length - 2, renderers.Count - (poses.Length - 1)).ToArray();
            RemoveRenderer(renderersToRemove);
        }
        for (int i = 0; i < poses.Length; i++)
        {
            if (i == 0)
                continue;
            if(i > renderers.Count)
                AddRenderer(poses[i - 1], poses[i]);
            else
                SetRendererPositions(renderers[i - 1], poses[i - 1], poses[i]);
        }
    }

    private void RemoveRenderer(LineRenderer[] renderersToRemove) => Array.ForEach(renderersToRemove, x => RemoveRenderer(x));
    private void RemoveRenderer(LineRenderer renderer)
    {
        renderers.Remove(renderer);
        //Destroy(renderer.gameObject);
        Pool_Send(renderer);
    }

    private void AddRenderer(Vector3 from, Vector3 to)
    {
        LineRenderer renderer = Pool_Get();
        renderer.widthCurve = referanceLineRenderer.widthCurve;
        renderer.startColor = referanceLineRenderer.startColor;
        renderer.endColor = referanceLineRenderer.endColor;
        renderer.alignment = referanceLineRenderer.alignment;
        renderer.sharedMaterials = referanceLineRenderer.sharedMaterials;
        renderer.sortingOrder = referanceLineRenderer.sortingOrder;

        SetRendererPositions(renderer, from, to);

        renderers.Add(renderer);
    }

    private void SetRendererPositions(LineRenderer renderer, Vector3 from, Vector3 to)
    {
        renderer.positionCount = 2;
        renderer.SetPosition(0, from);
        renderer.SetPosition(1, to);
    }

    public void ClearRenderers() => renderers.ForEach(x => RemoveRenderer(x));

    private LineRenderer Pool_Get()
    {
        LineRenderer renderer = pool.Where(x => x.gameObject.activeSelf == false).FirstOrDefault();
        if (renderer == null)
        {
            GameObject gO = new GameObject("LineRenderer(Do NoT Delete)");
            renderer = gO.gameObject.AddComponent<LineRenderer>();
            pool.Add(renderer);
        }
        else
            renderer.gameObject.SetActive(true);
        return renderer;
    }
    private void Pool_Send(LineRenderer renderer) => renderer.gameObject.SetActive(false);
}