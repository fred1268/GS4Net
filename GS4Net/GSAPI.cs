using System;
using System.Runtime.InteropServices;

namespace GS4Net
{
   internal class GSAPI
   {
#if WIN32
      [DllImport("gsdll32.dll", EntryPoint = "gsapi_new_instance")]
      private static extern int NewInstance(out IntPtr pinstance, IntPtr caller_handle);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
      private static extern int InitWithArgs(IntPtr instance, int argc, string[] argv);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_exit")]
      private static extern int Exit(IntPtr instance);

      [DllImport("gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
      private static extern void DeleteInstance(IntPtr instance);
#endif

#if WIN64
      [DllImport("gsdll64.dll", EntryPoint = "gsapi_new_instance")]
      private static extern int NewInstance(out IntPtr pinstance, IntPtr caller_handle);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_init_with_args")]
      private static extern int InitWithArgs(IntPtr instance, int argc, string[] argv);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_exit")]
      private static extern int Exit(IntPtr instance);

      [DllImport("gsdll64.dll", EntryPoint = "gsapi_delete_instance")]
      private static extern void DeleteInstance(IntPtr instance);
#endif

      public static void GS(string[] args)
      {
         lock (mutex)
         {
            NewInstance(out IntPtr gs, IntPtr.Zero);
            try
            {
               int result = InitWithArgs(gs, args.Length, args);

               if (result < 0)
               {
                  throw new ExternalException(String.Format("Ghostscript conversion error {0}: result may be incorrect", result), result);
               }
            }
            finally
            {
               Exit(gs);
               DeleteInstance(gs);
            }
         }
      }

      // Ensure we do not reenter GS from another thread
      private static readonly object mutex = new object();
   }
}
