using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Content.Sources;
using tContentPatch;
using tContentPatch.ModLoad;
using Terraria;

namespace DeadEffect
{
    internal class ThisMod : Mod
    {
        public static ModObject mo { get; internal set; } = null;
        public static IAssetRepository Asset { get; internal set; } = null;

        public override void Load(ModObject mo)
        {
            ThisMod.mo = mo;

            LoadAssetReader(ThisMod.mo);

            Shader.ShaderManager.Load(Asset);
        }

        private static void LoadAssetReader(ModObject mo)
        {
            //AssetReaderCollection assetReaderCollection = XnaExtensions.Get<AssetReaderCollection>(Main.instance.Services);
            AssetReaderCollection assetReaderCollection = new AssetReaderCollection();
            assetReaderCollection.RegisterReader(new XnbReader(Main.instance.Services), ".xnb");

            AsyncAssetLoader asyncAssetLoader = new AsyncAssetLoader(assetReaderCollection, 20);
            //asyncAssetLoader.RequireTypeCreationOnTransfer(typeof(Texture2D));

            Asset = new AssetRepository(new AssetLoader(assetReaderCollection), asyncAssetLoader);
            Asset.SetSources(new IContentSource[]
            {
                new FileSystemContentSource(mo.modPath),
            });
        }

        public override void Unload()
        {
            Shader.ShaderManager.Unload();

            Asset?.Dispose();
        }
    }
}
