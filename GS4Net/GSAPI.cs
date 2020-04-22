using System;
using System.Runtime.InteropServices;

namespace GS4Net
{
   internal class GSAPI
   {
      // WIN32
      [DllImport("gsdll32.dll", EntryPoint = "gsapi_new_instance")]
      private static extern int NewInstance32(out IntPtr pinstance, IntPtr caller_handle);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
      private static extern int InitWithArgs32(IntPtr instance, int argc, string[] argv);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_exit")]
      private static extern int Exit32(IntPtr instance);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
      private static extern void DeleteInstance32(IntPtr instance);

      // WIN64
      [DllImport("gsdll64.dll", EntryPoint = "gsapi_new_instance")]
      private static extern int NewInstance64(out IntPtr pinstance, IntPtr caller_handle);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_init_with_args")]
      private static extern int InitWithArgs64(IntPtr instance, int argc, string[] argv);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_exit")]
      private static extern int Exit64(IntPtr instance);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_delete_instance")]
      private static extern void DeleteInstance64(IntPtr instance);

      public static void GS32(string[] args)
      {
         lock (mutex)
         {
            NewInstance32(out IntPtr gs, IntPtr.Zero);
            try
            {
               int result = InitWithArgs32(gs, args.Length, args);

               if (result < 0)
               {
                  throw new ExternalException(String.Format("Ghostscript conversion error {0}: result may be incorrect", result), result);
               }
            }
            finally
            {
               Exit32(gs);
               DeleteInstance32(gs);
            }
         }
      }

      public static void GS64(string[] args)
      {
         lock (mutex)
         {
            NewInstance64(out IntPtr gs, IntPtr.Zero);
            try
            {
               int result = InitWithArgs64(gs, args.Length, args);

               if (result < 0)
               {
                  throw new ExternalException(String.Format("Ghostscript conversion error {0}: result may be incorrect", result), result);
               }
            }
            finally
            {
               Exit64(gs);
               DeleteInstance64(gs);
            }
         }
      }

      // Ensure we do not reenter GS from another thread
      private static readonly object mutex = new object();
   }
}
