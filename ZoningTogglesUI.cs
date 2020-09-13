// Decompiled with JetBrains decompiler
// Type: ZoningToggles.ZoningTogglesUI
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZoningToggles
{
  public class ZoningTogglesUI
  {
    private const string toggleZoneName = "ZoningToggle";
    private const string updateZoneName = "ZoningUpdateToggle";
    private static Dictionary<RoadBaseAI, bool> defaultZoningSettings = new Dictionary<RoadBaseAI, bool>();
    private PropertyChangedEventHandler<int> builtinModeChangedHandler;
    private UIPanel roadsOptionPanel;
    private UITabstrip builtinTabstrip;
    public DeactivatableButtonWrapper TZWrapper;
    public UIMultiStateButton updateZoneButton;
    private UIDropDown noPillarsDropDown;
    private bool sendActiveState = true;
    private int originalSelectedIndex;
    private bool listenToModeChanged = true;

    public event Action<bool> selectedToolModeChanged;

    public bool selectedModeChangedSet => this.selectedToolModeChanged != null;

    public void DestroyView()
    {
      if (Object.op_Inequality((Object) this.roadsOptionPanel, (Object) null))
        this.RemoveButtons(new string[2]
        {
          "ZoningToggle",
          "ZoningUpdateToggle"
        });
      if (Object.op_Inequality((Object) this.builtinTabstrip, (Object) null))
        this.builtinTabstrip.remove_eventSelectedIndexChanged(this.builtinModeChangedHandler);
      this.resetToolSettings();
    }

    private void RemoveButtons(string[] oldButtons)
    {
      foreach (string oldButton in oldButtons)
      {
        UIMultiStateButton multiStateButton = (UIMultiStateButton) ((UIComponent) this.roadsOptionPanel).Find<UIMultiStateButton>(oldButton);
        if (Object.op_Inequality((Object) multiStateButton, (Object) null))
        {
          ((UIComponent) multiStateButton).Hide();
          ((UIComponent) multiStateButton).get_parent().RemoveUIComponent((UIComponent) multiStateButton);
          Object.Destroy((Object) ((Component) multiStateButton).get_gameObject());
        }
      }
    }

    public bool CreateButtons(UIPanel uiPanel)
    {
      if (Object.op_Equality((Object) uiPanel, (Object) null))
        uiPanel = UIUtils.Find<UIPanel>("RoadsOptionPanel(RoadsPanel)");
      if (Object.op_Equality((Object) uiPanel, (Object) null))
      {
        Logger.Warning("Could not find RoadsOptionPanel");
        return false;
      }
      this.roadsOptionPanel = uiPanel;
      this.builtinTabstrip = (UITabstrip) ((UIComponent) this.roadsOptionPanel).Find<UITabstrip>("ToolMode");
      this.RemoveButtons(new string[2]
      {
        "ZoningToggle",
        "ZoningUpdateToggle"
      });
      this.roadsOptionPanel.get_atlas().get_material();
      UIMultiStateButton button = UIUtils.AddToggle((UIComponent) this.roadsOptionPanel, "ZoningToggle", "togglezoning_icon.png", -38, 38, 36, 36, DeactivatableButtonWrapper.iconNames);
      this.TZWrapper = new DeactivatableButtonWrapper(button, "Toggle zoning\nRight click reset default", "Default zoning set\nRight click enable toggle");
      // ISSUE: method pointer
      this.TZWrapper.AddEventActiveStateIndexChanged(new PropertyChangedEventHandler<int>((object) this, __methodptr(handleActiveStateIndexChanged)));
      UIPanel uiPanel1 = UIUtils.Find<UIPanel>("NoPillarsPanel");
      if (Object.op_Inequality((Object) uiPanel1, (Object) null))
      {
        Logger.Info("NoPillarsPanel found");
        using (IEnumerator<UIComponent> enumerator = ((IEnumerable<UIComponent>) ((UIComponent) uiPanel1).get_components()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            UIDropDown current = enumerator.Current as UIDropDown;
            Logger.Info("dropDown != null" + Object.op_Inequality((Object) current, (Object) null).ToString());
            if (Object.op_Inequality((Object) current, (Object) null))
              Logger.Info("dropDown.tooltip: '" + ((UIComponent) current).get_tooltip() + "'");
            if (Object.op_Inequality((Object) current, (Object) null) && ((UIComponent) current).get_tooltip().Equals("Change zoning/collision mode"))
            {
              this.noPillarsDropDown = current;
              // ISSUE: method pointer
              current.add_eventSelectedIndexChanged(new PropertyChangedEventHandler<int>((object) this, __methodptr(\u003CCreateButtons\u003Eb__19_0)));
            }
          }
        }
      }
      Logger.Debug(string.Concat((object) OptionsWrapper.ToggleState));
      button.set_activeStateIndex((int) OptionsWrapper.ToggleState % 2);
      this.TZWrapper.active = OptionsWrapper.ToggleState != ToggleStates.deactivated;
      this.UpdateVisible(OptionsWrapper.Options.ToggleVisible);
      this.CreateUpdateButtons();
      return true;
    }

    public void UpdateVisible(bool visible)
    {
      ((UIComponent) this.TZWrapper.Button).set_isVisible(visible);
      if (!visible)
        this.changeToolSettings(2);
      else if (this.TZWrapper.active)
        this.changeToolSettings(this.TZWrapper.Button.get_activeStateIndex());
      else
        this.resetToolSettings();
    }

    private void handleActiveStateIndexChanged(UIComponent sender, int value)
    {
      if (this.TZWrapper.active)
      {
        this.changeToolSettings(value);
      }
      else
      {
        if (Object.op_Inequality((Object) this.noPillarsDropDown, (Object) null))
          this.noPillarsDropDown.set_selectedIndex(0);
        this.resetToolSettings();
      }
    }

    private void resetToolSettings()
    {
      using (Dictionary<RoadBaseAI, bool>.Enumerator enumerator = ZoningTogglesUI.defaultZoningSettings.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<RoadBaseAI, bool> current = enumerator.Current;
          ((RoadAI) current.Key).m_enableZoning = (__Null) (current.Value ? 1 : 0);
        }
      }
    }

    private void changeToolSettings(int intValue)
    {
      bool flag = intValue == 1;
      if (this.sendActiveState && Object.op_Inequality((Object) this.noPillarsDropDown, (Object) null))
      {
        int num = !this.TZWrapper.active ? 0 : (flag ? 3 : 2);
        Logger.Info("newSetting: " + (object) num);
        this.noPillarsDropDown.set_selectedIndex(num);
      }
      foreach (UIComponent componentsInChild in (UIComponent[]) ((Component) UIUtils.Find<UIComponent>("RoadsPanel")).GetComponentsInChildren<UIButton>())
      {
        try
        {
          object objectUserData = componentsInChild.get_objectUserData();
          if (objectUserData is NetInfo)
          {
            if (objectUserData != null)
            {
              if (((NetInfo) objectUserData).m_netAI is RoadAI)
                ZoningTogglesUI.zoningToggleAction((RoadAI) ((NetInfo) objectUserData).m_netAI, flag);
            }
          }
        }
        catch (Exception ex)
        {
          Logger.Warning("Skipping " + ((Object) componentsInChild).get_name());
        }
      }
    }

    private static bool zoningToggleAction(RoadAI ai, bool value)
    {
      if (!ZoningTogglesUI.defaultZoningSettings.ContainsKey((RoadBaseAI) ai))
      {
        string[] strArray = new string[14]
        {
          "Small Busway",
          "HighwayRamp",
          "Small Rural Highway",
          "Rural Highway",
          "Highway",
          "Four-Lane Highway",
          "Five-Lane Highway",
          "Large Highway",
          "Bundesstrasse",
          "Autobahn",
          "Planning Road (Small)",
          "Planning Road (Large)",
          "Small Highway",
          "Dike"
        };
        ZoningTogglesUI.defaultZoningSettings.Add((RoadBaseAI) ai, true);
        foreach (string str in strArray)
        {
          if (((Object) ai).get_name().StartsWith(str))
            ZoningTogglesUI.defaultZoningSettings[(RoadBaseAI) ai] = false;
        }
      }
      ai.m_enableZoning = (__Null) (value ? 1 : 0);
      return true;
    }

    private void CreateUpdateButtons()
    {
      this.updateZoneButton = UIUtils.AddToggle((UIComponent) this.roadsOptionPanel, "ZoningUpdateToggle", "upgrade_zoning_icon.png", 58, 38, 31, 31, new string[5]
      {
        "Base",
        "BaseFocussed",
        "BaseHovered",
        "Icon",
        "IconFocussed"
      });
      ((UIComponent) this.updateZoneButton).set_tooltip("Toggle zoning update");
      this.updateZoneButton.get_backgroundSprites().AddState();
      this.updateZoneButton.get_foregroundSprites().AddState();
      UIUtils.setSpriteSet(this.updateZoneButton.get_backgroundSprites().get_Item(0), "Base");
      UIUtils.setSpriteSet(this.updateZoneButton.get_foregroundSprites().get_Item(0), "Icon");
      UIUtils.setSpriteSet(this.updateZoneButton.get_backgroundSprites().get_Item(1), "BaseFocussed");
      UIUtils.setSpriteSet(this.updateZoneButton.get_foregroundSprites().get_Item(1), "IconFocussed");
      this.updateZoneButton.get_backgroundSprites().get_Item(0).set_hovered("BaseHovered");
      this.updateZoneButton.get_backgroundSprites().get_Item(1).set_hovered("BaseHovered");
      // ISSUE: method pointer
      this.updateZoneButton.add_eventActiveStateIndexChanged(new PropertyChangedEventHandler<int>((object) this, __methodptr(changeToolEnabled)));
      // ISSUE: method pointer
      this.builtinModeChangedHandler = new PropertyChangedEventHandler<int>((object) this, __methodptr(\u003CCreateUpdateButtons\u003Eb__25_0));
      this.builtinTabstrip.add_eventSelectedIndexChanged(this.builtinModeChangedHandler);
    }

    private void changeToolEnabled(UIComponent component, int value)
    {
      bool flag = value == 1;
      if (this.selectedToolModeChanged == null || !this.listenToModeChanged)
        return;
      this.selectedToolModeChanged(flag);
    }

    public void UpdateSelectedIndex(bool enabled)
    {
      if (enabled)
      {
        if (this.builtinTabstrip.get_selectedIndex() >= 0)
          this.originalSelectedIndex = this.builtinTabstrip.get_selectedIndex();
        this.setSelectedIndex(-2);
      }
      else
      {
        if (this.builtinTabstrip.get_selectedIndex() >= 0 || this.originalSelectedIndex < 0)
          return;
        this.setSelectedIndex(this.originalSelectedIndex);
        this.originalSelectedIndex = -1;
      }
    }

    public void setSelectedIndex(int value)
    {
      if (value < 0)
        return;
      this.listenToModeChanged = false;
      this.builtinTabstrip.set_selectedIndex(value);
      this.listenToModeChanged = true;
    }
  }
}
