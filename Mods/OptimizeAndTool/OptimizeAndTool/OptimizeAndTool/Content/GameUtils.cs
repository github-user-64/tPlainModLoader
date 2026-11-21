using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace OptimizeAndTool.Content
{
    public static class GameUtils
    {
        /// <summary>
        /// 获取现在的<see cref="WorldFileData"/>, 失败返回<see langword="null"/>
        /// </summary>
        public static WorldFileData GetNowWorldFileData()
        {
            try
            {
                string path = Path.Combine(ThisMod.mo.modPath, "temp_WorldFileData");

                using (FileStream stream = File.Create(path))
                {
                    BinaryWriter writer = new BinaryWriter(stream);

                    if (Main.WorldFileMetadata == null) Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
                    if (WorldGen.Manifest == null) WorldGen.Manifest = new WorldManifest();

                    #region
                    int[] pointers = new int[]
                    {
                        WorldFile.SaveFileFormatHeader(writer),
                        WorldFile.SaveWorldHeader(writer),
                    };

                    WorldFile.SaveFooter(writer);
                    WorldFile.SaveHeaderPointers(writer, pointers);
                    #endregion
                }

                WorldFileData data = WorldFile.GetAllMetadata(path, false);

                try
                {
                    File.Delete(path);
                }
                catch { }

                if (data.LoadException != null) return null;
                if (data._worldSizeName == null) data.SetWorldSize(data.WorldSizeX, data.WorldSizeY);

                return data;
            }
            catch
            {
                return null;
            }
        }
    }
}
