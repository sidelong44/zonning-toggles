// Decompiled with JetBrains decompiler
// Type: ZoningToggles.Properties.Resources
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ZoningToggles.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (ZoningToggles.Properties.Resources.resourceMan == null)
          ZoningToggles.Properties.Resources.resourceMan = new ResourceManager("ZoningToggles.Properties.Resources", typeof (ZoningToggles.Properties.Resources).Assembly);
        return ZoningToggles.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ZoningToggles.Properties.Resources.resourceCulture;
      set => ZoningToggles.Properties.Resources.resourceCulture = value;
    }
  }
}
