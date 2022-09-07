using System.Configuration;
using System.CommandLine;
using System.Runtime.InteropServices;
using System.Reflection;

namespace SpideyTextureScaler
{
    public class Program
    {
        public List<TextureBase> texturestats { get; set; }

        [STAThread]
        static int Main(string[] args)
        {
            var p = new Program();
            p.texturestats = new List<TextureBase>() {
                new Source(),
                new DDS(),
                new Output(),
            };

            if (args.Length > 0)
            {
                return p.ConsoleMain(args);
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(p));
            return 0;
        }

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        public int ConsoleMain(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            Console.WriteLine("\r\n");

            var rootcommand = new RootCommand($"(v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}) Extract or replace textures.  See \"[command] --help\" for additional options.");
            // global
            var outputdir = new Option<DirectoryInfo?>(name: "--outputdir", description: "Output directory for generated files");
            outputdir.AddAlias("-o");
            rootcommand.AddGlobalOption(outputdir);
            // extract
            var source = new Argument<FileInfo?>(name: "source", "Source .texture file from the game");
            source.AddValidator(result => { if (!result.GetValueForArgument(source).Exists) result.ErrorMessage = "Source file not found"; });
            var allowsd = new Option<bool>("--allowsd", "Allow standard definition exports if the .hd.texture file is missing");
            var extractcommand = new Command("extract", "Extract .dds texture from input .texture/.hd.texture pairs.")
            {
                source,
                allowsd,
            };
            rootcommand.AddCommand(extractcommand);
            // replace
            var ddsfile = new Argument<FileInfo?>(name: "ddsfile", description: "Texture file in .dds format");
            ddsfile.AddValidator(result => { if (!result.GetValueForArgument(ddsfile).Exists) result.ErrorMessage = "DDS texture file not found"; });
            var ignoreformat = new Option<bool>("--ignoreformat", "Ignore DXGI format mismatches");
            var testmode = new Option<bool>("--testmode", "Replace only HD mipmaps");
            var replacecommand = new Command("replace", "Inject .dds texture into new .texture/.hd.texture files")
            {
                source,
                ddsfile,
                ignoreformat,
                testmode
            };
            rootcommand.AddCommand(replacecommand);

            extractcommand.SetHandler((source, outputdir, allowsd) => { Extract(source, outputdir, allowsd); }, source, outputdir, allowsd);
            replacecommand.SetHandler((source, ddsfile, outputdir, ignoreformat, testmode) => { Replace(source, ddsfile, outputdir, ignoreformat, testmode); }, source, ddsfile, outputdir, ignoreformat, testmode);
            var ret = rootcommand.Invoke(args);
            FreeConsole();
            return ret;
        }

        public void InitGlobals(FileInfo sourceInfo, DirectoryInfo? outputdirInfo, out string outputdir)
        {
            if (!sourceInfo.Exists)
            {
                Console.WriteLine("Source file not found");
                throw new FileNotFoundException();
            }
            if (outputdirInfo is not null && !outputdirInfo.Exists)
            {
                Console.WriteLine("Output directory not found");
                throw new FileNotFoundException();
            }

            outputdir = outputdirInfo?.FullName ?? "";
            string output;
            int errorrow;
            int errorcol;
            var source = (Source)texturestats[0];
            source.Filename = sourceInfo.FullName;
            source.Read(out output, out errorrow, out errorcol);

            Console.WriteLine(output);
            if (!source.Ready)
                throw new Exception("Problem with source file");
        }

        public void Extract(FileInfo sourceInfo, DirectoryInfo? outputdirInfo, bool allowsd)
        {
            string outputdir;
            Console.WriteLine($"Extract {sourceInfo.FullName}");
            InitGlobals(sourceInfo, outputdirInfo, out outputdir);

            var tex = (Source)texturestats[0];
            var savedds = new DDS();
            savedds.Filename = Path.ChangeExtension(tex.Filename, ".dds");
            savedds.Filename = outputdir != "" ? Path.Combine(outputdir, Path.GetFileName(savedds.Filename)) : savedds.Filename;
            savedds.Mipmaps = tex.Mipmaps;
            savedds.Images = tex.Images;
            savedds.Format = tex.Format;
            savedds.basemipsize = tex.basemipsize;
            byte[] hdmips = null;
            string output;

            savedds.HDMipmaps = 0;
            savedds.Width = tex.Width;
            savedds.Height = tex.Height;

            if (tex.HDSize > 0)
            {
                if (tex.hdfilename == "")
                {
                    if (!allowsd)
                        throw new Exception("HD texture data not available.  Provide a .hd.texture/_hd.texture file or enable --allowsd");
                    savedds.Width = (uint)tex.sd_width;
                    savedds.Height = (uint)tex.sd_height;
                }
                else
                {
                    savedds.HDMipmaps = tex.HDMipmaps;
                    hdmips = File.ReadAllBytes(tex.hdfilename);
                }
            }

            if (!savedds.Write(hdmips, tex.mipmaps, out output))
            {
                Console.WriteLine(output);
                throw new Exception("Extraction failed");
            };

            Console.WriteLine(output);
        }

        private void Replace(FileInfo sourceInfo, FileInfo ddsInfo, DirectoryInfo? outputdirInfo, bool ignoreformat, bool testmode)
        {
            string outputdir;
            Console.WriteLine($"Extract {sourceInfo.FullName}");
            InitGlobals(sourceInfo, outputdirInfo, out outputdir);
            var tex = (Source)texturestats[0];

            string output;
            int errorrow;
            int errorcol;
            var dds = (DDS)texturestats[1];
            dds.Filename = ddsInfo.FullName;
            if (tex.Images > 1 && !dds.Filename.ToLower().EndsWith(".a0.dds"))
                throw new Exception("Array textures must be named with .Ax.dds convention");
            dds.Read(out output, out errorrow, out errorcol);
            Console.WriteLine(output);
            if (!dds.Ready)
                throw new Exception("Problem with DDS file");

            var ddss = new List<DDS>() { dds };
            var stub = ddss[0].Filename.Substring(0, ddss[0].Filename.Length - ".a0.dds".Length);
            for (int i = 1; i < tex.Images; i++)
            {
                ddss.Add(new DDS());
                ddss[i].Filename = $"{stub}.A{i}.dds";
                if (!File.Exists(ddss[i].Filename))
                    throw new Exception($"Missing array image {i}: {ddss[i].Filename}");
                if (!ddss[i].Read(out output, out errorrow, out errorcol))
                {
                    Console.WriteLine(output);
                    return;
                }
                Console.WriteLine(output);
            }
            Output outobj = (Output)texturestats[2];
            outobj.Filename = Path.ChangeExtension(ddsInfo.FullName, ".texture");
            outobj.Filename = outputdir != "" ? Path.Combine(outputdir, Path.GetFileName(outobj.Filename)) : outobj.Filename;
            if (tex.Filename == outobj.Filename)
                throw new Exception("Input and output .texture files cannot be the same.  Choose an output directory, or rename your .dds");
            outobj.Generate(
                (Source)(texturestats[0]),
                ddss,
                testmode,
                ignoreformat,
                out output, out errorrow, out errorcol);

            Console.WriteLine(output);
            if (errorcol > -1)
                throw new Exception("Error writing output texture");
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
        public uint? Images { get; set;  }
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

        public const string defaultfilelabel = "Choose a file...";

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
            Filename = defaultfilelabel;
        }
    }
}
