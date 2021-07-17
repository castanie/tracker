using System;
using System.Drawing;
using System.Drawing.Imaging;

using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace Logic
{
    class Capture
    {
        private static readonly FeatureLevel[] featureLevels = new FeatureLevel[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
        };

        private static readonly Texture2DDescription textureDesc = new Texture2DDescription
        {
            Width = 1920,
            Height = 1080,
            MipLevels = 1,
            ArraySize = 1,
            Format = Format.B8G8R8A8_UNorm,
            SampleDescription = { Count = 1, Quality = 0 },
            Usage = ResourceUsage.Staging,
            BindFlags = BindFlags.None,
            CpuAccessFlags = CpuAccessFlags.Read,
            OptionFlags = ResourceOptionFlags.None,
        };

        ID3D11Device device;
        IDXGIOutputDuplication duplication;

        public Capture()
        {
            using var factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
            using var adapter = factory.GetAdapter(0);
            using var output = adapter.GetOutput(0).QueryInterface<IDXGIOutput1>();
            D3D11.D3D11CreateDevice(adapter, DriverType.Unknown, DeviceCreationFlags.None, featureLevels, out this.device);
            this.duplication = output.DuplicateOutput(device);
        }

        public unsafe Bitmap Start()
        {
            duplication.AcquireNextFrame(100, out var frameInfo, out var desktopResource);

            var desktopTexture = device.CreateTexture2D(textureDesc);
            device.ImmediateContext.CopyResource(desktopTexture, desktopResource.QueryInterface<ID3D11Texture2D>());

            var mapRes = device.ImmediateContext.Map(desktopTexture, 0, MapMode.Read, Vortice.Direct3D11.MapFlags.None);

            var frame = new Bitmap(1920, 1080, PixelFormat.Format32bppRgb);
            var mapDest = frame.LockBits(new Rectangle(0, 0, 1920, 1080), ImageLockMode.WriteOnly, frame.PixelFormat);
            var size = 1920 * 4 * 1080;
            Buffer.MemoryCopy(mapRes.DataPointer.ToPointer(), mapDest.Scan0.ToPointer(), size, size);
            frame.UnlockBits(mapDest);

            duplication.ReleaseFrame();

            return frame;
        }
    }
}