using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace df
{
    public class UTF8Marshaler : ICustomMarshaler
    {
        /// <summary>
        /// convert utf8 const char * to c# string
        /// </summary>
        /// <param name="pNativeData"></param>
        /// <returns></returns>
        public static string PtrToStr(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;

            List<byte> bytes = new List<byte>();
            for (int offset = 0; ; offset++)
            {
                byte b = Marshal.ReadByte(pNativeData, offset);
                if (b == 0)
                    break;
                else
                    bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }


        public void CleanUpManagedData(object managedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeHGlobal(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            if (object.ReferenceEquals(managedObj, null))
                return IntPtr.Zero;
            if (!(managedObj is string))
                throw new InvalidOperationException();

            byte[] utf8bytes = Encoding.UTF8.GetBytes(managedObj as string);
            IntPtr ptr = Marshal.AllocHGlobal(utf8bytes.Length + 1);
            Marshal.Copy(utf8bytes, 0, ptr, utf8bytes.Length);
            Marshal.WriteByte(ptr, utf8bytes.Length, 0);
            return ptr;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return PtrToStr(pNativeData);
        }

        private static UTF8Marshaler instance = new UTF8Marshaler();
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return instance;
        }
    }
}
