// Decompiled with JetBrains decompiler
// Type: ZoningToggles.LoadingExtension
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZoningToggles
{
  public class LoadingExtension : LoadingExtensionBase
  {
    public const string Name = "Zoning toolset (toggle + upgrade tool)";
    private ILoading loading;
    public static ZoningTogglesUI Ui;

    public virtual void OnCreated(ILoading loading)
    {
      this.loading = loading;
      ThreadingExtension.Instance.OnCreated();
      this.init();
    }

    public virtual void OnLevelLoaded(LoadMode mode)
    {
      base.OnLevelLoaded(mode);
      ThreadingExtension.Instance.OnLevelLoaded();
      this.init();
    }

    public virtual void OnLevelUnloading()
    {
      base.OnLevelUnloading();
      ThreadingExtension.Instance.OnLevelUnloading();
      this.DestroyView();
    }

    public virtual void OnReleased()
    {
      Logger.Info(nameof (OnReleased));
      ((ThreadingExtensionBase) ThreadingExtension.Instance).OnReleased();
      this.DestroyView();
    }

    private void init()
    {
      Logger.Info("Init");
      UIPanel roadOptionsPanel = UIUtils.Find<UIPanel>("RoadsOptionPanel(RoadsPanel)");
      if (Object.op_Equality((Object) roadOptionsPanel, (Object) null))
      {
        UITabstrip uiTabstrip = UIUtils.Find<UITabstrip>("MainToolstrip");
        if (Object.op_Equality((Object) uiTabstrip, (Object) null))
        {
          Logger.Warning("Could not find MainToolstrip");
        }
        else
        {
          Logger.Debug("eventSelectedIndexChanged Init");
          // ISSUE: method pointer
          uiTabstrip.add_eventSelectedIndexChanged(new PropertyChangedEventHandler<int>((object) this, __methodptr(mainToolStrip_eventSelectedIndexChanged)));
        }
      }
      else
        this.onWaitComplete(roadOptionsPanel);
    }

    private void DestroyView()
    {
      if (LoadingExtension.Ui == null)
        return;
      LoadingExtension.Ui.DestroyView();
      LoadingExtension.Ui = (ZoningTogglesUI) null;
    }

    private void mainToolStrip_eventSelectedIndexChanged(UIComponent component, int value)
    {
      Logger.Debug("eventSelectedIndexChanged");
      if (value != 0)
        return;
      Logger.Debug("eventSelectedIndexChanged right tab selected");
      ((MonoBehaviour) component).StartCoroutine((IEnumerator) this.GetRoadOptionsPanel((UITabstrip) component));
    }

    private IEnumerator<object> GetRoadOptionsPanel(UITabstrip tabstrip)
    {
      yield return (object) null;
      UIPanel roadsOptionsPanel = this.getRoadsOptionsPanel();
      if (Object.op_Inequality((Object) roadsOptionsPanel, (Object) null))
      {
        // ISSUE: method pointer
        tabstrip.remove_eventSelectedIndexChanged(new PropertyChangedEventHandler<int>((object) this, __methodptr(mainToolStrip_eventSelectedIndexChanged)));
        this.onWaitComplete(roadsOptionsPanel);
      }
    }

    private UIPanel getRoadsOptionsPanel() => UIUtils.Find<UIPanel>("RoadsOptionPanel(RoadsPanel)");

    private void onWaitComplete(UIPanel roadOptionsPanel)
    {
      Logger.Info(nameof (onWaitComplete));
      using (IEnumerator<PluginManager.PluginInfo> enumerator = Singleton<PluginManager>.get_instance().GetPluginsInfo().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          PluginManager.PluginInfo current = enumerator.Current;
          PublishedFileId publishedFileId = current.get_publishedFileID();
          bool flag = ((PublishedFileId) ref publishedFileId).get_AsUInt64() == 543722850UL;
          Logger.Info((flag && current.get_isEnabled()).ToString() ?? "");
          if (flag)
            break;
        }
      }
      if (this.loading.get_currentMode() != null || LoadingExtension.Ui != null)
        return;
      LoadingExtension.Ui = new ZoningTogglesUI();
      LoadingExtension.Ui.CreateButtons(roadOptionsPanel);
    }

    public LoadingExtension() => base.\u002Ector();
  }
}
