using ARKit;
using SceneKit;
using UIKit;

namespace ARKitExample.Nodes
{
    internal class PlaneNode : SCNNode
    {
        private const float PlaneHeight = 0.001f;

        private readonly SCNBox planeGeometry;

        public PlaneNode(ARPlaneAnchor planeAnchor)
        {
            Geometry = (planeGeometry = CreateGeometry(planeAnchor));
            Position = new SCNVector3(0, -PlaneHeight / 2, 0);
            PhysicsBody = SCNPhysicsBody.CreateKinematicBody();
        }

        public void Update(ARPlaneAnchor planeAnchor)
        {
            planeGeometry.Width = planeAnchor.Extent.X;
            planeGeometry.Length = planeAnchor.Extent.Z;
            Position = new SCNVector3(
                planeAnchor.Center.X, 
                planeAnchor.Center.Y, 
                planeAnchor.Center.Z);
        }

        private static SCNBox CreateGeometry(ARPlaneAnchor planeAnchor)
        {
            var geometry = SCNBox.Create(planeAnchor.Extent.X, PlaneHeight, planeAnchor.Extent.Z, 0);
            var topMaterial = new SCNMaterial();
            var bottomMaterial = new SCNMaterial();

            topMaterial.Diffuse.Contents = UIColor.FromRGBA(255, 255, 255, 20);
            bottomMaterial.Diffuse.Contents = UIColor.Black;

            geometry.Materials = new[] { topMaterial, bottomMaterial, bottomMaterial, bottomMaterial };

            return geometry;
        }
    }
}
