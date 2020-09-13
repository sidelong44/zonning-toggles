// Decompiled with JetBrains decompiler
// Type: ZoningToggles.ThreadingExtension
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZoningToggles.BuildToggle;

namespace ZoningToggles
{
  public class ThreadingExtension : ThreadingExtensionBase
  {
    private static ThreadingExtension _instance;
    private ToolBase.RaycastService raycastService;
    private NetTool netTool;
    private ToolError toolError;
    private bool mouseRayValid;
    private Ray mouseRay;
    private float mouseRayLength;
    private bool mouseDown;
    private UIPanel roadsPanel;
    private static bool loadingLevel = true;
    private ZoningTool11 buildTool;
    private bool ctrlDown;
    private int count;

    public static ThreadingExtension Instance
    {
      get
      {
        if (ThreadingExtension._instance == null)
          ThreadingExtension._instance = new ThreadingExtension();
        return ThreadingExtension._instance;
      }
    }

    private bool enabled => Object.op_Inequality((Object) this.buildTool, (Object) null);

    public void OnCreated()
    {
      Logger.Prefix = "[ZoningToggles] ";
      UIUtils.Prefix = "ZoningToggles";
      Logger.Debug(nameof (OnCreated));
    }

    public void OnLevelLoaded()
    {
      Logger.Debug(nameof (OnLevelLoaded));
      ThreadingExtension.loadingLevel = false;
    }

    public void OnLevelUnloading()
    {
      Logger.Debug(nameof (OnLevelUnloading));
      this.Destroy();
      ThreadingExtension.loadingLevel = true;
    }

    public virtual void OnReleased()
    {
      base.OnReleased();
      Logger.Debug(nameof (OnReleased));
      this.Destroy();
    }

    private void Destroy() => this.DestroyZoningTool();

    public virtual void OnUpdate(float realTimeDelta, float simulationTimeDelta)
    {
      if (ThreadingExtension.loadingLevel)
        return;
      this.ctrlDown = this.ctrlDown ? !Input.GetKeyUp((KeyCode) 306) && !Input.GetKeyUp((KeyCode) 305) : Input.GetKeyDown((KeyCode) 306) || Input.GetKeyDown((KeyCode) 305);
      if (LoadingExtension.Ui != null && !LoadingExtension.Ui.selectedModeChangedSet)
        LoadingExtension.Ui.selectedToolModeChanged += (Action<bool>) (enabled => this.SetToolEnabled(enabled));
      if (Object.op_Equality((Object) this.roadsPanel, (Object) null))
        this.roadsPanel = (UIPanel) UIView.Find<UIPanel>("RoadsPanel");
      if (Object.op_Equality((Object) this.roadsPanel, (Object) null) || !((UIComponent) this.roadsPanel).get_isVisible())
      {
        if (!this.enabled)
          return;
        this.SetToolEnabled(false, true);
      }
      else
      {
        if (Object.op_Equality((Object) this.netTool, (Object) null))
        {
          foreach (ToolBase tool in ToolsModifierControl.get_toolController().get_Tools())
          {
            NetTool netTool = tool as NetTool;
            if (Object.op_Inequality((Object) netTool, (Object) null) && Object.op_Inequality((Object) netTool.m_prefab, (Object) null))
            {
              this.netTool = netTool;
              break;
            }
          }
          if (Object.op_Equality((Object) this.netTool, (Object) null))
            return;
          this.raycastService = new ToolBase.RaycastService((ItemClass.Service) ((ItemClass) ((NetInfo) this.netTool.m_prefab).m_class).m_service, (ItemClass.SubService) ((ItemClass) ((NetInfo) this.netTool.m_prefab).m_class).m_subService, (ItemClass.Layer) ((ItemClass) ((NetInfo) this.netTool.m_prefab).m_class).m_layer);
        }
        if (this.enabled)
        {
          this.mouseDown = Input.GetMouseButton(0);
          this.mouseRayValid = !ToolsModifierControl.get_toolController().get_IsInsideUI() && Cursor.get_visible();
          this.mouseRay = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
          this.mouseRayLength = Camera.get_main().get_farClipPlane();
          if (!Object.op_Inequality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) this.buildTool))
            return;
          this.SetToolEnabled(false, true);
        }
        else
        {
          if (!Object.op_Equality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) this.buildTool))
            return;
          ToolsModifierControl.get_toolController().set_CurrentTool((ToolBase) this.netTool);
        }
      }
    }

    public void SetToolEnabled(bool enabled, bool resetNetToolModeToStraight = false)
    {
      if (enabled)
      {
        ToolsModifierControl.get_toolController().set_CurrentTool((ToolBase) this.CreateZoningTool());
      }
      else
      {
        if (Object.op_Equality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) this.buildTool) || Object.op_Equality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) null))
          ToolsModifierControl.get_toolController().set_CurrentTool((ToolBase) this.netTool);
        this.DestroyZoningTool();
      }
      LoadingExtension.Ui.UpdateSelectedIndex(enabled);
      if (!resetNetToolModeToStraight)
        return;
      LoadingExtension.Ui.updateZoneButton.set_activeStateIndex(0);
    }

    private ZoningTool11 CreateZoningTool()
    {
      if (Object.op_Inequality((Object) this.buildTool, (Object) null))
        return this.buildTool;
      this.buildTool = (ZoningTool11) ((Component) ToolsModifierControl.get_toolController()).get_gameObject().GetComponent<ZoningTool11>();
      if (Object.op_Equality((Object) this.buildTool, (Object) null))
        this.buildTool = (ZoningTool11) ((Component) ToolsModifierControl.get_toolController()).get_gameObject().AddComponent<ZoningTool11>();
      return this.buildTool;
    }

    private void DestroyZoningTool()
    {
      Logger.DebugData((object) Object.op_Equality((Object) ToolsModifierControl.get_toolController(), (Object) null));
      if (Object.op_Inequality((Object) ToolsModifierControl.get_toolController(), (Object) null) && (Object.op_Equality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) this.buildTool) || Object.op_Equality((Object) ToolsModifierControl.get_toolController().get_CurrentTool(), (Object) null)))
        ToolsModifierControl.get_toolController().set_CurrentTool((ToolBase) this.netTool);
      if (Object.op_Inequality((Object) this.buildTool, (Object) null))
      {
        Logger.Debug("Destroying tool: " + ((object) this.buildTool).GetType().Name + " it is: " + (object) this.buildTool);
        Object.Destroy((Object) this.buildTool);
        this.buildTool = (ZoningTool11) null;
      }
      else
        Logger.Debug("Destroying tool: " + typeof (ZoningTool11).Name + " it is: " + (object) this.buildTool);
    }

    public virtual void OnBeforeSimulationTick()
    {
      if (!this.enabled)
        return;
      if (Object.op_Inequality((Object) this.buildTool, (Object) null))
      {
        if (!this.mouseDown && this.buildTool.SegmentsTouched.Count > 0)
          this.buildTool.SegmentsTouched = new HashSet<int>();
        this.buildTool.isHoveringSegment = false;
        this.buildTool.ToolCursor = (CursorInfo) this.netTool.m_upgradeCursor;
        // ISSUE: variable of the null type
        __Null texture = ((CursorInfo) this.netTool.m_upgradeCursor).m_texture;
      }
      if (!this.mouseRayValid)
        return;
      ToolBase.RaycastInput input;
      ((ToolBase.RaycastInput) ref input).\u002Ector(this.mouseRay, this.mouseRayLength);
      input.m_netService = (__Null) this.raycastService;
      input.m_ignoreTerrain = (__Null) 1;
      input.m_ignoreNodeFlags = (__Null) -1;
      input.m_ignoreSegmentFlags = (__Null) 32;
      ToolBase.RaycastOutput output;
      if (ZoningTool11.RayCast(input, out output))
      {
        int netSegment = (int) output.m_netSegment;
        if (netSegment != 0)
        {
          NetManager instance = Singleton<NetManager>.get_instance();
          NetTool.ControlPoint startPoint;
          NetTool.ControlPoint middlePoint;
          NetTool.ControlPoint endPoint;
          this.GetSegmentControlPoints(netSegment, out startPoint, out middlePoint, out endPoint);
          ushort num1;
          ushort num2;
          int num3;
          int num4;
          if ((NetTool.CreateNode(((NetSegment) ref ((Array16<NetSegment>) instance.m_segments).m_buffer[netSegment]).get_Info(), startPoint, middlePoint, endPoint, (FastList<NetTool.NodePosition>) NetTool.m_nodePositionsSimulation, 1000, true, false, true, false, false, false, (ushort) 0, ref num1, ref num2, ref num3, ref num4) & 32L) != null)
            this.toolError = ToolError.OutOfArea;
          else if (this.mouseDown)
            this.HandleMouseDrag(ref output, ref this.toolError, false, ref netSegment);
          else
            this.HandleMouseDrag(ref output, ref this.toolError, true, ref netSegment);
          if (Object.op_Inequality((Object) this.buildTool, (Object) null))
          {
            this.buildTool.segment = (NetSegment) ((Array16<NetSegment>) instance.m_segments).m_buffer[netSegment];
            this.buildTool.segmentIndex = netSegment;
            if (Object.op_Equality((Object) (((NetSegment) ref this.buildTool.segment).get_Info().m_netAI as RoadAI), (Object) null) && this.toolError != ToolError.OutOfArea)
              this.toolError = ToolError.CannotUpgradeThisType;
            this.buildTool.isHoveringSegment = this.toolError != ToolError.CannotUpgradeThisType;
            this.GetSegmentControlPoints(netSegment, out this.buildTool.startPoint, out this.buildTool.middlePoint, out this.buildTool.endPoint);
          }
        }
      }
      if (!Object.op_Inequality((Object) this.buildTool, (Object) null))
        return;
      this.buildTool.toolError = this.toolError;
    }

    private void HandleMouseDrag(
      ref ToolBase.RaycastOutput raycastOutput,
      ref ToolError error,
      bool test,
      ref int newSegmentIndex)
    {
      Vector3.get_zero();
      int netSegment = (int) raycastOutput.m_netSegment;
      if (netSegment == 0)
        return;
      ((NetSegment) ref ((Array16<NetSegment>) Singleton<NetManager>.get_instance().m_segments).m_buffer[netSegment]).get_Info();
      int num = this.RebuildSegment(netSegment, ref error, test);
      if (num == 0 || error != ToolError.None)
        return;
      newSegmentIndex = num;
      this.buildTool.SegmentsTouched.Add(num);
    }

    private void GetSegmentControlPoints(
      int segmentIndex,
      out NetTool.ControlPoint startPoint,
      out NetTool.ControlPoint middlePoint,
      out NetTool.ControlPoint endPoint)
    {
      NetManager instance = Singleton<NetManager>.get_instance();
      NetSegment netSegment = (NetSegment) ((Array16<NetSegment>) instance.m_segments).m_buffer[segmentIndex];
      NetInfo info = ((NetSegment) ref netSegment).get_Info();
      NetNode netNode1 = (NetNode) ((Array16<NetNode>) instance.m_nodes).m_buffer[netSegment.m_startNode];
      startPoint.m_node = netSegment.m_startNode;
      startPoint.m_segment = (__Null) 0;
      startPoint.m_position = netNode1.m_position;
      startPoint.m_direction = netSegment.m_startDirection;
      startPoint.m_elevation = (__Null) (double) (float) netNode1.m_elevation;
      startPoint.m_outside = (__Null) ((netNode1.m_flags & 1024) > 0 ? 1 : 0);
      NetNode netNode2 = (NetNode) ((Array16<NetNode>) instance.m_nodes).m_buffer[netSegment.m_endNode];
      endPoint.m_node = netSegment.m_endNode;
      endPoint.m_segment = (__Null) 0;
      endPoint.m_position = netNode2.m_position;
      endPoint.m_direction = (__Null) Vector3.op_UnaryNegation((Vector3) netSegment.m_endDirection);
      endPoint.m_elevation = (__Null) (double) (float) netNode2.m_elevation;
      endPoint.m_outside = (__Null) ((netNode2.m_flags & 1024) > 0 ? 1 : 0);
      middlePoint.m_node = (__Null) 0;
      middlePoint.m_segment = (__Null) (int) (ushort) segmentIndex;
      middlePoint.m_position = (__Null) Vector3.op_Addition((Vector3) startPoint.m_position, Vector3.op_Multiply((Vector3) startPoint.m_direction, info.GetMinNodeDistance() + 1f));
      middlePoint.m_direction = startPoint.m_direction;
      middlePoint.m_elevation = (__Null) (double) Mathf.Lerp((float) startPoint.m_elevation, (float) endPoint.m_elevation, 0.5f);
      middlePoint.m_outside = (__Null) 0;
    }

    private int RebuildSegment(int segmentIndex, ref ToolError error, bool test)
    {
      Logger.Debug("Segment selected:" + (object) segmentIndex);
      NetManager instance = Singleton<NetManager>.get_instance();
      NetSegment netSegment = (NetSegment) ((Array16<NetSegment>) instance.m_segments).m_buffer[segmentIndex];
      if (this.buildTool.SegmentsTouched.Contains(segmentIndex))
      {
        this.buildTool.ToolCursor = (CursorInfo) this.netTool.m_upgradeCursor;
        return 0;
      }
      NetInfo info = ((NetSegment) ref netSegment).get_Info();
      RoadAI netAi = info.m_netAI as RoadAI;
      if (Object.op_Equality((Object) netAi, (Object) null))
      {
        this.toolError = ToolError.CannotUpgradeThisType;
        return 0;
      }
      bool flag = netSegment.m_blockStartLeft != null || netSegment.m_blockStartRight != null || netSegment.m_blockEndLeft != null || netSegment.m_blockEndRight > 0;
      if (test)
      {
        this.buildTool.toolMode = flag ? ToolMode.remove : ToolMode.add;
        this.toolError = ToolError.None;
        return 0;
      }
      bool enableZoning = (bool) netAi.m_enableZoning;
      netAi.m_enableZoning = !flag ? (__Null) 1 : (__Null) (this.ctrlDown ? 1 : 0);
      int num = this.RebuildSegmentImpl(instance, info, segmentIndex, ref error);
      netAi.m_enableZoning = (__Null) (enableZoning ? 1 : 0);
      return num;
    }

    private int RebuildSegmentImpl(
      NetManager net,
      NetInfo prefab,
      int segmentIndex,
      ref ToolError error)
    {
      NetTool.ControlPoint startPoint;
      NetTool.ControlPoint middlePoint;
      NetTool.ControlPoint endPoint;
      this.GetSegmentControlPoints(segmentIndex, out startPoint, out middlePoint, out endPoint);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = true;
      bool flag4 = false;
      bool flag5 = false;
      ushort num1 = 0;
      ushort num2 = 0;
      int num3 = 0;
      int num4 = 0;
      NetManager instance1 = Singleton<NetManager>.get_instance();
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      bool flag6 = (^(NetSegment&) ref ((Array16<NetSegment>) instance1.m_segments).m_buffer[(int) num2]).m_blockStartLeft > 0;
      NetTool.CreateNode(prefab, startPoint, middlePoint, endPoint, (FastList<NetTool.NodePosition>) NetTool.m_nodePositionsSimulation, 1000, flag1, flag2, flag3, flag4, flag5, false, (ushort) 0, ref num1, ref num2, ref num3, ref num4);
      if (num2 == (ushort) 0)
        return 0;
      if (((ItemClass) prefab.m_class).m_service == 9)
        Singleton<CoverageManager>.get_instance().CoverageUpdated((ItemClass.Service) 0, (ItemClass.SubService) 0, (ItemClass.Level) -1);
      error = ToolError.None;
      if (this.ctrlDown)
      {
        ZoneManager instance2 = Singleton<ZoneManager>.get_instance();
        Logger.Info(flag6.ToString() ?? "");
        NetSegment netSegment = (NetSegment) ((Array16<NetSegment>) instance1.m_segments).m_buffer[(int) num2];
        ushort num5 = 0;
        string str = "";
        switch (this.count)
        {
          case 0:
            num5 = (ushort) netSegment.m_blockStartLeft;
            netSegment.m_blockStartLeft = (__Null) 0;
            str = "bl.m_blockStartLeft";
            break;
          case 1:
            num5 = (ushort) netSegment.m_blockStartRight;
            netSegment.m_blockStartRight = (__Null) 0;
            str = "bl.m_blockStartRight";
            break;
          case 2:
            num5 = (ushort) netSegment.m_blockEndLeft;
            netSegment.m_blockEndLeft = (__Null) 0;
            str = "bl.m_blockEndLeft";
            break;
          case 3:
            num5 = (ushort) netSegment.m_blockEndRight;
            netSegment.m_blockEndRight = (__Null) 0;
            str = "bl.m_blockEndRight";
            break;
        }
        ++this.count;
        this.count %= 4;
        Logger.Info(string.Concat((object) this.count));
        ZoneBlock zoneBlock = (ZoneBlock) ((Array16<ZoneBlock>) instance2.m_blocks).m_buffer[(int) num5];
        Logger.DebugData((object) ("block " + str), (object) ("m_buildIndex:" + (object) (uint) zoneBlock.m_buildIndex), (object) ("m_nextGridBlock:" + (object) (ushort) zoneBlock.m_nextGridBlock));
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(ZoneBlock&) ref ((Array16<ZoneBlock>) instance2.m_blocks).m_buffer[(int) num5] = (ZoneBlock) null;
        ZoneManager zoneManager = instance2;
        zoneManager.m_blockCount = (__Null) (zoneManager.m_blockCount - 1);
      }
      return (int) num2;
    }

    public ThreadingExtension() => base.\u002Ector();
  }
}
