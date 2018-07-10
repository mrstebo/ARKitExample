using System;
using System.Collections.Generic;
using System.Linq;
using ARKit;
using ARKitExample.Nodes;
using Foundation;
using SceneKit;
using UIKit;

namespace ARKitExample
{
    public partial class ViewController : UIViewController
    {
        private readonly ARSCNView sceneView;

        protected ViewController(IntPtr handle) : base(handle)
        {
            this.sceneView = new ARSCNView
            {
                AutoenablesDefaultLighting = true,
                //DebugOptions = ARSCNDebugOptions.ShowFeaturePoints,
                Delegate = new SceneViewDelegate()
            };
            this.View.AddSubview(this.sceneView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.sceneView.Frame = this.View.Frame;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.GravityAndHeading
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.sceneView.Session.Pause();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (touches.AnyObject is UITouch touch)
            {
                var point = touch.LocationInView(this.sceneView);
                var hits = this.sceneView.HitTest(point, ARHitTestResultType.ExistingPlaneUsingExtent);
                var hit = hits.FirstOrDefault();

                if (hit == null) return;

                var cubeNode = new CubeNode(0.05f, UIColor.White)
                {
                    Position = new SCNVector3(
                        hit.WorldTransform.Column3.X,
                        hit.WorldTransform.Column3.Y + 0.1f,
                        hit.WorldTransform.Column3.Z
                    )
                };

                this.sceneView.Scene.RootNode.AddChildNode(cubeNode);
            }
        }

        class SceneViewDelegate : ARSCNViewDelegate
        {
            private readonly IDictionary<NSUuid, PlaneNode> planeNodes = new Dictionary<NSUuid, PlaneNode>();

            public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
            {
                if (anchor is ARPlaneAnchor planeAnchor)
                {
                    var planeNode = new PlaneNode(planeAnchor);
                    node.AddChildNode(planeNode);
                    this.planeNodes.Add(anchor.Identifier, planeNode);
                }
            }

            public override void DidRemoveNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
            {
                if (anchor is ARPlaneAnchor planeAnchor)
                {
                    this.planeNodes[anchor.Identifier].RemoveFromParentNode();
                    this.planeNodes.Remove(anchor.Identifier);
                }
            }

            public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
            {
                if (anchor is ARPlaneAnchor planeAnchor)
                {
                    //this.planeNodes[anchor.Identifier].Update(planeAnchor);
                }
            }
        }
    }
}
