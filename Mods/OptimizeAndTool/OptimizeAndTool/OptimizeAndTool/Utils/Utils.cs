using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace OptimizeAndTool.Utils
{
    public static class Utils
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

        /// <summary>
        /// 交换<see langword="index1"/>和<see langword="index2"/>的位置<br/>
        /// <see langword="index1"/>是:<paramref name="item"/><br/>
        /// <see langword="index2"/>是:<paramref name="item"/>加上<paramref name="off"/>
        /// </summary>
        public static void MoveItem<T>(this List<T> list, T item, int off)
        {
            int index1 = list.IndexOf(item);
            if (list.IndexInRange(index1) != true) return;
            int index2 = index1 + off;
            if (list.IndexInRange(index2) != true) return;

            list[index1] = list[index2];
            list[index2] = item;
        }
    }
}
