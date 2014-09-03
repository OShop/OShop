using Orchard.UI.Resources;
using System;

namespace OShop.Resources {
    public class OShopResources : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("OShopAdmin").SetUrl("oshop-admin.min.css", "oshop-admin.css");
        }
    }
}