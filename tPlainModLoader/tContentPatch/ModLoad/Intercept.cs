using System;
using System.Collections.Generic;

namespace tContentPatch.ModLoad
{
    internal class Intercept : IModLoader
    {
        public Action<List<ModObject>> OnLoaded = null;
        public Action<Exception> OnLoadException = null;
        private IModLoader ml = null;
        private List<ModObject> mos = null;

        public Intercept(IModLoader ml)
        {
            this.ml = ml;
        }

        public List<ModObject> Load()
        {
            try
            {
                mos = ml.Load();
            }
            catch (Exception ex)
            {
                OnLoadException?.Invoke(ex);
                throw ex;
            }
            OnLoaded?.Invoke(mos);
            return mos;
        }

        public void Unload() => ml.Unload();

        public void CancelLoad() => ml.CancelLoad();

        public string GetTip() => ml.GetTip();

        public bool IsLoading() => ml.IsLoading();

        public void ProgressBar(out int val, out int max) => ml.ProgressBar(out val, out max);
    }
}
