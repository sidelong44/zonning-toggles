// Decompiled with JetBrains decompiler
// Type: ZoningToggles.Logger
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using System;
using System.Collections.Generic;

namespace ZoningToggles
{
  public static class Logger
  {
    public static string Prefix;
    private static readonly bool inGameDebug = Environment.OSVersion.Platform != PlatformID.Unix;
    private static Dictionary<string, int> count = new Dictionary<string, int>();

    public static void ResetCount(string message)
    {
    }

    public static void Count(string message, string extra = "")
    {
    }

    public static void Debug(string message, params object[] args)
    {
    }

    public static void DebugData(params object[] args)
    {
    }

    public static void Info(string message, params object[] args)
    {
    }

    public static void Warning(string message, params object[] args) => UnityEngine.Debug.LogWarning((object) Logger.format(message, args));

    public static void Error(string message, params object[] args) => UnityEngine.Debug.LogError((object) Logger.format(message, args));

    private static string format(string message, params object[] args) => Logger.Prefix + string.Format(message, args);
  }
}
