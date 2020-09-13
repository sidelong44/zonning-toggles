// Decompiled with JetBrains decompiler
// Type: ZoningToggles.BuildToggle.ZoningTool11
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework;
using System.Collections.Generic;
using UnityEngine;

namespace ZoningToggles.BuildToggle
{
  public class ZoningTool11 : ToolBase
  {
    public HashSet<int> SegmentsTouched;
    public NetSegment segment;
    public ToolError toolError;
    public bool isHoveringSegment;
    public ToolMode toolMode;
    public int segmentIndex;
    public NetTool.ControlPoint startPoint;
    public NetTool.ControlPoint middlePoint;
    public NetTool.ControlPoint endPoint;
    private Color add;
    private Color remove;

    public CursorInfo ToolCursor
    {
      get => base.get_ToolCursor();
      set => base.set_ToolCursor(value);
    }

    protected virtual void Awake() => this.m_toolController = (__Null) Object.FindObjectOfType<ToolController>();

    protected virtual void OnToolUpdate()
    {
      if (this.isHoveringSegment)
      {
        Vector3 center = ((Bounds) ref this.segment.m_bounds).get_center();
        string str = "";
        if (this.toolError == ToolError.OutOfArea)
          str += "Out of city limits!";
        else if (this.toolError == ToolError.CannotUpgradeThisType)
          str += "Road's zoning can't be toggled";
        this.ShowToolInfo(true, str, center);
      }
      else
        this.ShowToolInfo(false, (string) null, Vector3.get_zero());
    }

    public virtual void SimulationStep()
    {
      base.SimulationStep();
      if (!this.isHoveringSegment)
        return;
      ushort num1;
      ushort num2;
      int num3;
      int num4;
      NetTool.CreateNode(((NetSegment) ref this.segment).get_Info(), this.startPoint, this.middlePoint, this.endPoint, (FastList<NetTool.NodePosition>) NetTool.m_nodePositionsSimulation, 1000, true, false, true, false, false, false, (ushort) 0, ref num1, ref num2, ref num3, ref num4);
    }

    public virtual void RenderGeometry(RenderManager.CameraInfo cameraInfo)
    {
      base.RenderGeometry(cameraInfo);
      if (!this.isHoveringSegment)
        return;
      ((ToolController) this.m_toolController).RenderCollidingNotifications(cameraInfo, (ushort) 0, (ushort) 0);
    }

    public virtual void RenderOverlay(RenderManager.CameraInfo cameraInfo)
    {
      base.RenderOverlay(cameraInfo);
      NetManager instance = Singleton<NetManager>.get_instance();
      Color toolColor1 = this.GetToolColor(false, (uint) this.toolError > 0U);
      foreach (int index in this.SegmentsTouched)
      {
        NetSegment netSegment = (NetSegment) ((Array16<NetSegment>) instance.m_segments).m_buffer[index];
        NetTool.RenderOverlay(cameraInfo, ref netSegment, toolColor1, toolColor1);
      }
      if (!this.isHoveringSegment)
        return;
      Color color = toolColor1;
      if (this.SegmentsTouched.Count == 0 && this.toolError == ToolError.None)
      {
        if (this.toolMode == ToolMode.add)
          color = this.add;
        else if (this.toolMode == ToolMode.remove)
          color = this.remove;
      }
      NetTool.RenderOverlay(cameraInfo, ref this.segment, color, color);
      Color toolColor2 = this.GetToolColor(true, false);
      ((ToolController) this.m_toolController).RenderColliding(cameraInfo, toolColor1, toolColor2, toolColor1, toolColor2, (ushort) this.segmentIndex, (ushort) 0);
    }

    protected virtual void OnEnable()
    {
      base.OnEnable();
      Singleton<TerrainManager>.get_instance().set_RenderZones(true);
    }

    protected virtual void OnDisable()
    {
      base.OnDisable();
      base.set_ToolCursor((CursorInfo) null);
      Singleton<TerrainManager>.get_instance().set_RenderZones(false);
    }

    public static bool RayCast(ToolBase.RaycastInput input, out ToolBase.RaycastOutput output) => ToolBase.RayCast(input, ref output);

    public ZoningTool11() => base.\u002Ector();
  }
}
