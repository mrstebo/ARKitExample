using SceneKit;
using UIKit;

namespace ARKitExample.Nodes
{
    internal class CubeNode : SCNNode
    {
        public CubeNode(float size, UIColor color)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(size, color),
                Position = new SCNVector3(0, size / 2, 0),
                PhysicsBody = SCNPhysicsBody.CreateDynamicBody()
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(float size, UIColor color)
        {
            var geometry = SCNBox.Create(size, size, size, 0);
            var material = new SCNMaterial();

            material.Diffuse.Contents = color;

            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
