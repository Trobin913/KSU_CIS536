using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampFireScene
{
    public class AssetManger
    {
        private string _assetDirectory;

        public AssetManger(string assetDirectory)
        {
            _assetDirectory = assetDirectory;
        }

        public void LoadAssets(object sender, EventArgs e)
        {
            //Load in all assets from the disk here.
        }
    }
}
