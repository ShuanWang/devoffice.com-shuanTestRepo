using Orchard.UI.Resources;

namespace Provoke.Highlights
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineScript("Angular").SetUrl("angular.min.js");
            manifest.DefineScript("AngularXEditable").SetUrl("xeditable.min.js").SetDependencies("Angular");
            manifest.DefineScript("AngularUiBootstrap").SetUrl("xeditable.min.js").SetDependencies("Angular");

            manifest.DefineStyle("AngularXEditable").SetUrl("xeditable.css");
        }
    }
}