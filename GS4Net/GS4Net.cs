using System;
using System.Collections.Generic;

namespace GS4Net
{
   public class GS4Net
   {
      // -d arguments
      private static readonly Dictionary<string, string> DEFAULTDARGS = new Dictionary<string, string>() {
         { "QUIET", "" },
         { "PARANOIDSAFER", "" },
         { "BATCH", "" },
         { "NOPAUSE", "" },
         { "NOPROMPT", "" },
         { "MaxBitmap", "102400000" },
         { "NumRenderingThreads", "4" },
         { "AlignToPixels", "0" },
         { "GridFitTT", "0" },
         { "TextAlphaBits", "4" },
         { "GraphicsAlphaBits", "4" },
         { "UseCropBox", "" },
         { "FirstPage", "1" },
         { "LastPage", "1" },
         { "DEVICEXRESOLUTION", "72" },
         { "DEVICEYRESOLUTION", "72" }
      };

      // -s arguments
      private static readonly Dictionary<string, string> DEFAULTSARGS = new Dictionary<string, string>() {
         { "PAPERSIZE", "a7" },
         { "OutputFile", "cover.png" },
         { "DEVICE", "png16m" }
      };

      public static readonly string REMOVE = "-+REMOVE+-";

      // More information at: https://www.ghostscript.com/doc/9.52/Use.htm#Parameter_switches
      public static void Generate32(string pdf, string img, Dictionary<string, string> dArgs, Dictionary<string, string> sArgs)
      {
         GSAPI.GS32(CombineArgs(pdf, img, dArgs, sArgs));
      }

      public static void Generate64(string pdf, string img, Dictionary<string, string> dArgs, Dictionary<string, string> sArgs)
      {
         GSAPI.GS64(CombineArgs(pdf, img, dArgs, sArgs));
      }

      public static void Generate(string pdf, string img, Dictionary<string, string> dArgs, Dictionary<string, string> sArgs)
      {
#if X86
         GSAPI.GS32(CombineArgs(pdf, img, dArgs, sArgs));
#elif X64
         GSAPI.GS64(CombineArgs(pdf, img, dArgs, sArgs));
#endif
      }

      private static Dictionary<string, string> MergeDictionary(Dictionary<string, string> def,  Dictionary<string, string> dic)
      {
         Dictionary<string, string> merged = new Dictionary<string, string>();
         foreach (string key in def.Keys)
         {
            merged.Add(key, def[key]);
         }
         if (dic != null)
         {
            foreach (string key in dic.Keys)
            {
               if (merged.ContainsKey(key))
               {
                  if (dic[key].Equals(REMOVE))
                  {
                     merged.Remove(key);
                  }
                  else
                  {
                     merged[key] = dic[key];
                  }
               }
               else
               {
                  merged.Add(key, dic[key]);
               }
            }
         }
         return merged;
      }

      private static string[] CombineArgs(string pdf, string img, Dictionary<string, string> dArgs, Dictionary<string, string> sArgs)
      {
         List<string> gsArgs = new List<string>();
         // -d arguments
         Dictionary<string, string> allArgs = MergeDictionary(DEFAULTDARGS, dArgs);
         foreach (string key in allArgs.Keys)
         {
            if (allArgs[key].Length == 0)
            {
               gsArgs.Add(String.Format("-d{0}", key));
            }
            else
            {
               gsArgs.Add(String.Format("-d{0}={1}", key, allArgs[key]));
            }
         }
         // -s arguments
         allArgs = MergeDictionary(DEFAULTSARGS, sArgs);
         if (img != null && img.Length != 0)
         {
            allArgs["OutputFile"] = img;
         }
         foreach (string key in allArgs.Keys)
         {
            if (allArgs[key].Length == 0)
            {
               gsArgs.Add(String.Format("-s{0}", key));
            }
            else
            {
               gsArgs.Add(String.Format("-s{0}={1}", key, allArgs[key]));
            }
         }
         gsArgs.Add("-q");
         gsArgs.Add(pdf);
         return gsArgs.ToArray();
      }
   }
}
