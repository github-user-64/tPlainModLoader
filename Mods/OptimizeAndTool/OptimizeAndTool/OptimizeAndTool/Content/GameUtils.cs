using OptimizeAndTool.Content.ServerList;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace OptimizeAndTool.Content
{
    public static class GameUtils
    {
        /// <summary>
        /// 获取现在的<see cref="WorldFileData"/>, 失败为<see langword="null"/>
        /// </summary>
        /// <returns>返回异常信息, 成功为<see langword="null"/></returns>
        public static Exception GetNowWorldFileData(out WorldFileData data)
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

                WorldFileData rv = WorldFile.GetAllMetadata(path, false);
                if (rv == null) throw new Exception("获取到的数据为null");

                try
                {
                    File.Delete(path);
                }
                catch { }

                if (rv.LoadException != null)
                {
                    data = null;
                    return rv.LoadException;
                }

                if (rv._worldSizeName == null) rv.SetWorldSize(rv.WorldSizeX, rv.WorldSizeY);

                data = rv;
                return null;
            }
            catch (Exception ex)
            {
                data = null;
                return ex;
            }
        }

        /// <summary>
        /// 获取现在的<see cref="ServerInfo"/>, 失败为<see langword="null"/>
        /// </summary>
        /// <returns>返回异常信息, 成功为<see langword="null"/></returns>
        public static Exception GetNowServerInfo(out ServerInfo info)
        {
            try
            {
                if (Main.netMode != 1) throw new Exception("不是在多人模式中");
                //if (Main.gameMenu == true) throw new Exception("不在游戏中");

                Exception ex = GetNowWorldFileData(out WorldFileData data);
                if (ex != null) throw new Exception("获取当前世界数据失败", ex);

                ServerInfo rv = ServerInfo.WorldFileDataToServerInfo(data);
                if (rv == null) throw new Exception("获取到的数据为null");

                rv.IP = Netplay.ServerIPText;
                rv.Port = Netplay.ListenPort;

                info = rv;
                return null;
            }
            catch (Exception ex)
            {
                info = null;
                return ex;
            }
        }
    }
}
