// Decompiled with JetBrains decompiler
// Type: ZoningToggles.DeactivatableButtonWrapper
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework.UI;

namespace ZoningToggles
{
  public class DeactivatableButtonWrapper
  {
    public static string[] iconNames = new string[6]
    {
      "Icon",
      "Base",
      "BaseFocussed",
      "BaseHovered",
      "BasePressed",
      "BaseDisabled"
    };
    private bool m_active = true;
    private string tooltipActive = "";
    private string tooltipInactive = "";
    private int m_oldActiveStateIndex;
    public PropertyChangedEventHandler<int> onActiveStateIndexChanged;

    public UIMultiStateButton Button { get; private set; }

    public DeactivatableButtonWrapper(
      UIMultiStateButton button,
      string tooltipActive = "",
      string tooltipInactive = "")
    {
      this.Button = button;
      this.tooltipActive = tooltipActive;
      this.tooltipInactive = tooltipInactive;
      // ISSUE: method pointer
      ((UIComponent) button).add_eventMouseUp(new MouseEventHandler((object) this, __methodptr(onMouseUp)));
      this.Button.get_backgroundSprites().Clear();
      this.Button.get_foregroundSprites().Clear();
      this.Button.get_backgroundSprites().AddState();
      this.Button.get_foregroundSprites().AddState();
      this.updateButton();
    }

    public void AddEventActiveStateIndexChanged(PropertyChangedEventHandler<int> method)
    {
      this.onActiveStateIndexChanged = method;
      this.Button.add_eventActiveStateIndexChanged(method);
    }

    public bool active
    {
      get => this.m_active;
      set
      {
        if (value == this.m_active)
          return;
        this.m_active = value;
        this.updateButton();
        int num = value ? this.m_oldActiveStateIndex : this.Button.get_activeStateIndex();
        if (!value)
          this.m_oldActiveStateIndex = this.Button.get_activeStateIndex();
        if (this.Button.get_activeStateIndex() == num)
          this.onActiveStateIndexChanged.Invoke((UIComponent) this.Button, num);
        else
          this.Button.set_activeStateIndex(num);
      }
    }

    private void onMouseUp(UIComponent sender, UIMouseEventParameter p)
    {
      if (p.get_buttons() != 2)
        return;
      this.active = !this.active;
    }

    private void updateButton()
    {
      UIUtils.setSpriteSet(this.Button.get_backgroundSprites().get_Item(0), "BaseDisabled");
      UIUtils.setSpriteSet(this.Button.get_foregroundSprites().get_Item(0), "Icon");
      UIUtils.setSpriteSet(this.Button.get_backgroundSprites().get_Item(1), "BaseDisabled");
      UIUtils.setSpriteSet(this.Button.get_foregroundSprites().get_Item(1), "Icon");
      if (this.active)
      {
        UIUtils.setSpriteSet(this.Button.get_backgroundSprites().get_Item(0), "Base");
        UIUtils.setSpriteSet(this.Button.get_backgroundSprites().get_Item(1), "BaseFocussed");
        this.Button.get_backgroundSprites().get_Item(0).set_hovered("BaseHovered");
      }
      ((UIComponent) this.Button).set_tooltip(this.active ? this.tooltipActive : this.tooltipInactive);
      ((UIComponent) this.Button).RefreshTooltip();
      ((UIComponent) this.Button).set_playAudioEvents(this.active);
    }
  }
}
