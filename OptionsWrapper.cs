// Decompiled with JetBrains decompiler
// Type: ZoningToggles.OptionsWrapper
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework.UI;
using ICities;

namespace ZoningToggles
{
  public static class OptionsWrapper
  {
    public static Options Options = new Options();
    private static UIDropDown defaultState = (UIDropDown) null;
    private static UICheckBox toggleVisible = (UICheckBox) null;

    public static ToggleStates ToggleState
    {
      get => (ToggleStates) OptionsWrapper.Options.toggleState;
      set => OptionsWrapper.Options.toggleState = (int) value;
    }

    public static void makeSettings(UIHelperBase helper)
    {
      helper.AddGroup("Zoning toolset (toggle + upgrade tool)");
      // ISSUE: method pointer
      OptionsWrapper.toggleVisible = helper.AddCheckbox("Show the zoning toggle (off sets zoning to vanilla settings)", OptionsWrapper.Options.ToggleVisible, new OnCheckChanged((object) null, __methodptr(onSettingToggleVisibleChanged))) as UICheckBox;
      // ISSUE: method pointer
      OptionsWrapper.defaultState = helper.AddDropdown("Default zoning toggle state", new string[3]
      {
        "off",
        "on",
        "deactivated"
      }, OptionsWrapper.Options.toggleState, new OnDropdownSelectionChanged((object) null, __methodptr(onSettingToggleStateChanged))) as UIDropDown;
    }

    private static void onSettingToggleStateChanged(int newToggleState)
    {
      OptionsWrapper.Options.toggleState = newToggleState;
      OptionsLoader.SaveOptions();
    }

    private static void onSettingToggleVisibleChanged(bool newValue)
    {
      OptionsWrapper.Options.ToggleVisible = newValue;
      if (LoadingExtension.Ui != null)
        LoadingExtension.Ui.UpdateVisible(newValue);
      OptionsLoader.SaveOptions();
    }
  }
}
