//Written for Warspear Online. https://store.steampowered.com/app/326360
using System.IO;

namespace Warspear_Online_Extractor
{
    class Program
    {
        public static BinaryReader br;
        static void Main(string[] args)
        {
            br = new(File.OpenRead(args[0]));

            if (new string(br.ReadChars(4)) != "MDPK")
                throw new System.Exception("This is not Warspear Online's pak file.");

            br.ReadInt16();//Version?
            int count = br.ReadInt32();
            string date = new string(br.ReadChars(10));
            br.BaseStream.Position += 6;

            System.Collections.Generic.List<Subfile> subfiles = new();
            for (int i = 0; i < count; i++)
            {
                subfiles.Add(new());
            }

            string path = Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//";
            foreach (Subfile file in subfiles)
            {
                Directory.CreateDirectory(path + Path.GetDirectoryName(file.name));
                br.BaseStream.Position = file.start;
                BinaryWriter bw = new(File.Create(path + file.name));
                bw.Write(br.ReadBytes(file.size));
                bw.Close();
            }
        }

        class Subfile
        {
            public int start = br.ReadInt32(), size = br.ReadInt32(), unknown = br.ReadInt32();
            byte unknown2 = br.ReadByte();
            public string name = new string(br.ReadChars(55)).TrimEnd('\0');
        }
    }
}
