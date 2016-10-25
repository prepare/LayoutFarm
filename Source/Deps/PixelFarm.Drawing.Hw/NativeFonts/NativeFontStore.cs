﻿//MIT, 2014-2016, WinterDev
//-----------------------------------
//use FreeType and HarfBuzz wrapper
//native dll lib
//plan?: port  them to C#  :)
//-----------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
namespace PixelFarm.Drawing.Fonts
{

    /// <summary>
    /// to load and cache native font 
    /// </summary>
    public class NativeFontStore
    {
        Dictionary<string, NativeFontFace> fonts = new Dictionary<string, NativeFontFace>();
        Dictionary<RequestFont, NativeFont> registerFonts = new Dictionary<RequestFont, NativeFont>();
        //--------------------------------------------------

        static Dictionary<string, InstalledFont> regular_Fonts = new Dictionary<string, InstalledFont>();
        static Dictionary<string, InstalledFont> bold_Fonts = new Dictionary<string, InstalledFont>();
        static Dictionary<string, InstalledFont> italic_Fonts = new Dictionary<string, InstalledFont>();
        static Dictionary<string, InstalledFont> boldItalic_Fonts = new Dictionary<string, InstalledFont>();
        static Dictionary<string, InstalledFont> gras_Fonts = new Dictionary<string, InstalledFont>();
        static Dictionary<string, InstalledFont> grasItalic_Fonts = new Dictionary<string, InstalledFont>();
        //--------------------------------------------------

        static NativeFontStore()
        {

            List<InstalledFont> installedFonts = InstalledFontCollection.ReadInstallFonts();

            //do 
            int j = installedFonts.Count;
            for (int i = 0; i < j; ++i)
            {
                InstalledFont f = installedFonts[i];
                if (f == null || f.FontName == "" || f.FontName.StartsWith("\0"))
                {
                    //no font name?
                    continue;
                }
                switch (f.FontSubFamily)
                {
                    case "Normal":
                    case "Regular":
                        {
                            regular_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    case "Italic":
                    case "Italique":
                        {
                            italic_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    case "Bold":
                        {
                            bold_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    case "Bold Italic":
                        {
                            boldItalic_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    case "Gras":
                        {
                            gras_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    case "Gras Italique":
                        {
                            grasItalic_Fonts.Add(f.FontName.ToUpper(), f);
                        } break;
                    default:
                        throw new NotSupportedException();
                }
            }


        }
        static void SetShapingEngine(NativeFontFace fontFace, string lang, HBDirection hb_direction, int hb_scriptcode)
        {
            ExportTypeFaceInfo exportTypeInfo = new ExportTypeFaceInfo();
            NativeMyFontsLib.MyFtSetupShapingEngine(fontFace.Handle,
               lang,
               lang.Length,
               hb_direction,
               hb_scriptcode,
               ref exportTypeInfo);
            fontFace.HBFont = exportTypeInfo.hb_font;
        }
        public RequestFont LoadFont(string fontName, string filename, float fontSizeInPoint)
        {
            RequestFont font = new RequestFont(fontName, fontSizeInPoint);
            LoadFont(font, filename);
            return font;
        }
        public RequestFont LoadFont(string fontName, float fontSizeInPoint)
        {
            RequestFont font = new RequestFont(fontName, fontSizeInPoint);
            LoadFont(font);
            return font;
        }
        public void LoadFont(RequestFont font)
        {
            //request font from installed font
            InstalledFont found;
            switch (font.Style)
            {
                case (FontStyle.Bold | FontStyle.Italic):
                    {
                        //check if we have bold & italic 
                        //version of this font ?


                        if (!boldItalic_Fonts.TryGetValue(font.Name.ToUpper(), out found))
                        {
                            //if not found then goto italic 
                            goto case FontStyle.Italic;
                        }

                        LoadFont(font, found.FontPath);
                    } break;
                case FontStyle.Bold:
                    {

                        if (!bold_Fonts.TryGetValue(font.Name.ToUpper(), out found))
                        {
                            //goto regular
                            goto default;
                        }
                        LoadFont(font, found.FontPath);
                    } break;
                case FontStyle.Italic:
                    {
                        //if not found then choose regular
                        if (!italic_Fonts.TryGetValue(font.Name.ToUpper(), out found))
                        {
                            goto default;
                        }
                        LoadFont(font, found.FontPath);
                    } break;
                default:
                    {
                        //we skip gras style ?
                        if (!regular_Fonts.TryGetValue(font.Name.ToUpper(), out found))
                        {
                            //if not found this font 
                            //the choose other ?
                            throw new NotSupportedException();
                        }
                        LoadFont(font, found.FontPath);

                    } break;
            }
        }
        public void LoadFont(RequestFont font, string filename)
        {
            //load font from specific file 
            int major, minor, revision;
            NativeMyFontsLib.MyFtLibGetFullVersion(out major, out minor, out revision);

            NativeFontFace fontFace;
            if (!fonts.TryGetValue(filename, out fontFace))
            {
                //if not found
                //then load it
                byte[] fontFileContent = File.ReadAllBytes(filename);
                int filelen = fontFileContent.Length;
                IntPtr unmanagedMem = Marshal.AllocHGlobal(filelen);
                Marshal.Copy(fontFileContent, 0, unmanagedMem, filelen);
                //---------------------------------------------------
                //convert font point size to pixel size 
                //---------------------------------------------------
                //load font from memory
                IntPtr faceHandle = NativeMyFontsLib.MyFtNewMemoryFace(unmanagedMem, filelen);
                if (faceHandle != IntPtr.Zero)
                {

                    //ok pass 
                    //-------------------
                    //test change font size
                    //NativeMyFontsLib.MyFtSetCharSize(faceHandle,
                    //    0, //char_width in 1/64th of points, value 0 => same as height
                    //    16 * 64,//16 pt //char_height in 1*64 of ppoints
                    //    96,//horizontal device resolution (eg screen resolution 96 dpi)
                    //    96);// vertical device resolution  
                    //------------------- 
                    fontFace = new NativeFontFace(unmanagedMem, faceHandle);
                    fontFace.LoadFromFilename = filename;
                    ExportTypeFaceInfo exportTypeInfo = new ExportTypeFaceInfo();
                    NativeMyFontsLib.MyFtGetFaceInfo(faceHandle, ref exportTypeInfo);
                    fontFace.HasKerning = exportTypeInfo.hasKerning;
                    //for shaping engine***
                    SetShapingEngine(fontFace,
                        font.Lang,
                        font.HBDirection,
                        font.ScriptCode);
                    fonts.Add(filename, fontFace);
                    //------------------- 
                    //uint glyphIndex1;
                    //int char1 = NativeMyFontsLib.MyFtGetFirstChar(faceHandle, out glyphIndex1);
                    //List<CharAndGlyphMap> charMap = new List<CharAndGlyphMap>(); 
                    //while (char1 != 0)
                    //{
                    //    char c = (char)char1;
                    //    charMap.Add(new CharAndGlyphMap(glyphIndex1, c));
                    //    char1 = NativeMyFontsLib.MyFtGetNextChar(faceHandle, char1, out glyphIndex1);
                    //}
                    //------------------- 

                    //load glyph map
                }
                else
                {
                    //load font error
                    Marshal.FreeHGlobal(unmanagedMem);
                }
            }
            //-------------------------------------------------
            //get font that specific size from found font face
            //-------------------------------------------------
            NativeFont nativeFont = fontFace.GetFontAtPointSize(font.SizeInPoints);
            registerFonts.Add(font, nativeFont);

        }
        public NativeFont GetResolvedNativeFont(RequestFont f)
        {
            NativeFont found;
            registerFonts.TryGetValue(f, out found);
            f.ActualFont = found;
            return found;
        }
    }
}
