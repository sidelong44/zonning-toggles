// Decompiled with JetBrains decompiler
// Type: ZoningToggles.UIUtils
// Assembly: ZoningToggles, Version=1.0.6719.2836, Culture=neutral, PublicKeyToken=null
// MVID: FFB208AB-A9FB-49A7-85DD-5B4D116898E3
// Assembly location: C:\Users\sid44\Desktop\ZonningToggles\ZoningToggles.dll

using ColossalFramework.UI;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ZoningToggles
{
  public class UIUtils
  {
    public static string Prefix;

    public static T Find<T>(string name) where T : Object
    {
      foreach (T obj in Object.FindObjectsOfType<T>())
      {
        if (((Object) (object) obj).get_name().Equals(name))
          return obj;
      }
      return default (T);
    }

    public static void SetButtonPositionAndSize(
      UIMultiStateButton button,
      int x,
      int y,
      int width,
      int height)
    {
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector((float) x, (float) y);
      Vector3 vector3_2;
      ((Vector3) ref vector3_2).\u002Ector((float) width, (float) height);
      ((UIComponent) button).set_relativePosition(vector3_1);
      ((UIComponent) button).set_size(Vector2.op_Implicit(vector3_2));
    }

    public static UIMultiStateButton AddToggle(
      UIComponent optionPanel,
      string name,
      string spriteImage,
      int x,
      int y,
      int width,
      int height,
      string[] iconNames)
    {
      M0 m0 = optionPanel.AddUIComponent<UIMultiStateButton>();
      UIUtils.SetButtonPositionAndSize((UIMultiStateButton) m0, x, y, width, height);
      ((UIComponent) m0).set_playAudioEvents(true);
      ((Object) m0).set_name(name);
      ((UIComponent) m0).set_tooltip("");
      ((UIComponent) m0).set_isTooltipLocalized(false);
      ((UIMultiStateButton) m0).set_spritePadding(new RectOffset());
      ((UIMultiStateButton) m0).set_atlas(UIUtils.CreateTextureAtlas(spriteImage, name + "Atlas", ((UIPanel) optionPanel).get_atlas().get_material(), width, height, iconNames));
      return (UIMultiStateButton) m0;
    }

    public static UITextureAtlas CreateTextureAtlas(
      string textureFile,
      string atlasName,
      Material baseMaterial,
      int spriteWidth,
      int spriteHeight,
      string[] spriteNames)
    {
      Logger.Debug("Processing, textureFile: " + textureFile + ", atlasName: " + atlasName + ", width: " + (object) spriteWidth + ", height: " + (object) spriteHeight + ", baseMaterial" + (object) baseMaterial);
      int width = spriteWidth * spriteNames.Length;
      int height = spriteHeight;
      Texture2D texture = UIUtils.createTexture(textureFile, width, height);
      UITextureAtlas instance = (UITextureAtlas) ScriptableObject.CreateInstance<UITextureAtlas>();
      Material material = (Material) Object.Instantiate<Material>((M0) baseMaterial);
      material.set_mainTexture((Texture) texture);
      instance.set_material(material);
      ((Object) instance).set_name(atlasName);
      for (int index = 0; index < spriteNames.Length; ++index)
      {
        float num = 1f / (float) spriteNames.Length;
        UITextureAtlas.SpriteInfo spriteInfo1 = new UITextureAtlas.SpriteInfo();
        spriteInfo1.set_name(spriteNames[index]);
        spriteInfo1.set_texture(texture);
        spriteInfo1.set_region(new Rect((float) index * num, 0.0f, num, 1f));
        UITextureAtlas.SpriteInfo spriteInfo2 = spriteInfo1;
        instance.AddSprite(spriteInfo2);
      }
      return instance;
    }

    public static Texture2D createTexture(string textureFile, int width, int height)
    {
      Texture2D texture2D = new Texture2D(width, height, (TextureFormat) 5, false);
      ((Texture) texture2D).set_filterMode((FilterMode) 1);
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string str = UIUtils.Prefix + ".Assets." + textureFile;
      string name = str;
      Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
      if (manifestResourceStream == null)
        Logger.Warning("Could not load texture " + str);
      byte[] buffer = new byte[manifestResourceStream.Length];
      manifestResourceStream.Read(buffer, 0, buffer.Length);
      if (!texture2D.LoadImage(buffer))
        return (Texture2D) null;
      texture2D.Apply(true, true);
      return texture2D;
    }

    public static void setSpriteSet(UIMultiStateButton.SpriteSet spriteSet, string baseName)
    {
      spriteSet.set_normal(baseName);
      spriteSet.set_disabled(baseName);
      spriteSet.set_hovered(baseName);
      spriteSet.set_pressed(baseName);
      spriteSet.set_focused(baseName);
    }
  }
}
