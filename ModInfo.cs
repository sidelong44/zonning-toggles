// Decompiled with JetBrains decompiler
// Type: ZoningToggles.ModInfo
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ICities;

namespace ZoningToggles
{
  public class ModInfo : IUserMod
  {
    public string Name
    {
      get
      {
        OptionsLoader.LoadOptions();
        return "Zoning toolset (toggle + upgrade tool)";
      }
    }

    public string Description => "Adds a zoning toggle and update tool build roads menu";

    public void OnSettingsUI(UIHelperBase helper) => OptionsWrapper.makeSettings(helper);
  }
}
