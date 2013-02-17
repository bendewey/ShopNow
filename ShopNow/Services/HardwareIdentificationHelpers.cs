using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System.Profile;

namespace ShopNow.Services
{
    public static class HardwareIdentificationHelpers
    {
        public static string GetHardwareId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var stream = token.Id.AsStream();
            using (var reader = new BinaryReader(stream))
            {
                var bytes = reader.ReadBytes((int) stream.Length);
                return BitConverter.ToString(bytes);
            }
        }
    }
}