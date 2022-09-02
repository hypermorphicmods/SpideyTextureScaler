namespace SpideyTextureScaler
{
    public class Program
    {
        public List<TextureBase> texturestats { get; set; }

        [STAThread]
        static void Main()
        {
            var p = new Program();
            p.texturestats = new List<TextureBase>() {
                new Source(),
                new DDS(),
                new Output(),
            };
            
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(p));

            
        }
    }

    public abstract class TextureBase
    {
        // grid display fields
        public string? Name { get; set; }
        public bool Ready { get; set; }
        public uint? Width { get; set; }
        public uint? Height { get; set; }
        public uint? Mipmaps { get; set; }
        public uint? HDMipmaps { get; set; }
        public double? BytesPerPixel { get; set; }
        public uint? Size { get; set; }
        public uint? HDSize { get; set; }
        public DXGI_FORMAT? Format { get; set; }

        // display
        public string? Filename { get; set; }

        // non-display
        public int basemipsize;
        public int sd_width;
        public int sd_height;
        public int aspect;
        

        public abstract bool Read(out string output, out int errorrow, out int errorcol);
        public void ResetVisible()
        {
            Width = null;
            Height = null;
            Mipmaps = null;
            HDMipmaps = null;
            BytesPerPixel = null;
            Size = null;
            HDSize = null;
            Format = null;
            Ready = false;
            Filename = "Choose a file...";
        }
    }
}
