// Decompiled with JetBrains decompiler
// Type: ZoningToggles.OptionsLoader
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace ZoningToggles
{
  public static class OptionsLoader
  {
    private const string FileName = "ZoningToggles.xml";

    public static void LoadOptions()
    {
      try
      {
        try
        {
          using (StreamReader streamReader = new StreamReader("ZoningToggles.xml"))
            OptionsWrapper.Options = (Options) new XmlSerializer(typeof (Options)).Deserialize((TextReader) streamReader);
        }
        catch (FileNotFoundException ex)
        {
        }
      }
      catch (Exception ex)
      {
        Logger.Error("Unexpected {0} while loading options: {1}\n{2}", (object) ex.GetType().Name, (object) ex.Message, (object) ex.StackTrace);
      }
    }

    public static void SaveOptions()
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter("ZoningToggles.xml"))
          new XmlSerializer(typeof (Options)).Serialize((TextWriter) streamWriter, (object) OptionsWrapper.Options);
      }
      catch (Exception ex)
      {
        Debug.LogErrorFormat("Unexpected {0} while saving options: {1}\n{2}", new object[3]
        {
          (object) ex.GetType().Name,
          (object) ex.Message,
          (object) ex.StackTrace
        });
      }
    }
  }
}
