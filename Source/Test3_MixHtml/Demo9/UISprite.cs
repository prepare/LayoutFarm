﻿//MIT, 2018-present, WinterDev

using System;
using System.Collections.Generic;

using PixelFarm.Drawing;
using PixelFarm.CpuBlit;
using LayoutFarm.RenderBoxes;

namespace LayoutFarm.UI
{
    class VgBridgeRenderElement : RenderElement
    {
        public VgBridgeRenderElement(RootGraphic rootGfx, int width, int height)
            : base(rootGfx, width, height)
        {

            //this.dbug_ObjectNote = "AAA";
            //this.NeedClipArea = false;
            this.MayHasChild = true;
            this.TransparentForAllEvents = true;
        }
        public override void ChildrenHitTestCore(HitChain hitChain)
        {
            RectD bound = new RectD();

            if (bound.Contains(hitChain.TestPoint.x, hitChain.TestPoint.y))
            {
                //check exact hit or the vxs part
                if (HitTestOnSubPart(this, hitChain.TextPointX, hitChain.TextPointY))
                {
                    hitChain.AddHitObject(this);
                    return; //return after first hit
                }
            }

            base.ChildrenHitTestCore(hitChain);
        }
        static bool HitTestOnSubPart(VgBridgeRenderElement _svgRenderVx, float x, float y)
        {
            //int partCount = _svgRenderVx.VgCmdCount; 
            //for (int i = partCount - 1; i >= 0; --i)
            //{
            //    //we do hittest top to bottom => (so => iter backward)

            //    VgCmd vx = _svgRenderVx.GetVgCmd(i);
            //    if (vx.Name != VgCommandName.Path)
            //    {
            //        continue;
            //    }
            //    //
            //    VgCmdPath path = (VgCmdPath)vx;
            //    //fine tune
            //    //hit test ***
            //    if (PixelFarm.CpuBlit.VertexProcessing.VertexHitTester.IsPointInVxs(path.Vxs, x, y))
            //    {
            //        return true;
            //    }
            //}
            //return false;


            return false;

        }
        public override void CustomDrawToThisCanvas(DrawBoard canvas, Rectangle updateArea)
        {
            throw new NotSupportedException();
            //if (RenderVx == null) return;
            ////--------------------------

            //if (RenderVx != null)
            //{
            //    canvas.DrawRenderVx(RenderVx, this.X, this.Y);
            //}
        }
        public override void ResetRootGraphics(RootGraphic rootgfx)
        {

        }
    }

    public class UISprite : UIElement
    {

        VgBridgeRenderElement _svgRenderElement;
#if DEBUG
        static int dbugTotalId;
        public readonly int dbugId = dbugTotalId++;
#endif
        public UISprite(float width, float height)
        {
            SetElementBoundsWH(width, height);
            this.AutoStopMouseEventPropagation = true;

        }
        public void LoadSvg(object renderVx)
        {
            //_svgRenderVx = renderVx;
            //if (_svgRenderElement != null)
            //{
            //    _svgRenderElement.RenderVx = renderVx;
            //    RectD bound = renderVx.GetBounds();
            //    this.SetSize((float)bound.Width, (float)bound.Height);
            //}
        }
        protected override void OnElementChanged()
        {

            if (_svgRenderElement != null)
            {
                //_svgRenderVx.SetBitmapSnapshot(null);

                //_svgRenderElement.RenderVx = _svgRenderVx;
                //_svgRenderVx.InvalidateBounds();
                //RectD bound1 = _svgRenderVx.GetBounds();

            }
        }
        protected override void OnMouseDown(UIMouseEventArgs e)
        {
            base.OnMouseDown(e);
        }
        public override void Walk(UIVisitor visitor)
        {

        }
        protected override bool HasReadyRenderElement
        {
            get { return _svgRenderElement != null; }
        }
        public override RenderElement CurrentPrimaryRenderElement
        {
            get { return _svgRenderElement; }
        }
        public override RenderElement GetPrimaryRenderElement(RootGraphic rootgfx)
        {
            if (_svgRenderElement == null)
            {
                _svgRenderElement = new VgBridgeRenderElement(rootgfx, 10, 10);
                _svgRenderElement.SetLocation((int)this.Left, (int)this.Top);

                _svgRenderElement.SetController(this);
                //if (_svgRenderVx != null)
                //{
                //    _svgRenderElement.RenderVx = _svgRenderVx;
                //    RectD bound = _svgRenderVx.GetBounds();
                //    this.SetSize((int)bound.Width, (int)bound.Height);
                //}
            }
            return _svgRenderElement;
        }
        public virtual void SetLocation(float left, float top)
        {
            SetElementBoundsLT(left, top);
            if (this.HasReadyRenderElement)
            {
                //TODO: review rounding here
                this.CurrentPrimaryRenderElement.SetLocation((int)left, (int)top);
            }
        }

        public virtual void SetSize(float width, float height)
        {
            SetElementBoundsWH(width, height);
            if (this.HasReadyRenderElement)
            {
                this.CurrentPrimaryRenderElement.SetSize((int)width, (int)height);
            }
        }

        public void SetBounds(float left, float top, float width, float height)
        {
            SetLocation(left, top);
            SetSize(width, height);
        }
        public float Left
        {
            get
            {
                if (this.HasReadyRenderElement)
                {
                    return this.CurrentPrimaryRenderElement.X;
                }
                else
                {
                    return BoundLeft;
                }
            }
        }
        public float Top
        {
            get
            {
                if (this.HasReadyRenderElement)
                {
                    return this.CurrentPrimaryRenderElement.Y;
                }
                else
                {
                    return BoundTop;
                }
            }
        }
        public float Right
        {
            get { return this.Left + Width; }
        }
        public float Bottom
        {
            get { return this.Top + Height; }
        }

        public float Width
        {
            get
            {
                if (this.HasReadyRenderElement)
                {
                    return this.CurrentPrimaryRenderElement.Width;
                }
                else
                {
                    return BoundWidth;
                }
            }
        }
        public float Height
        {
            get
            {
                if (this.HasReadyRenderElement)
                {
                    return this.CurrentPrimaryRenderElement.Height;
                }
                else
                {
                    return BoundHeight;
                }
            }
        }

        public override void InvalidateGraphics()
        {
            if (this.HasReadyRenderElement)
            {
                this.CurrentPrimaryRenderElement.InvalidateGraphics();
            }
        }
        public void InvalidateOuterGraphics()
        {
            if (this.CurrentPrimaryRenderElement != null)
            {
                this.CurrentPrimaryRenderElement.InvalidateGraphicBounds();
            }
        }
        public virtual void PerformContentLayout()
        {
        }
        protected virtual void Describe(UIVisitor visitor)
        {
            visitor.Attribute("left", this.Left);
            visitor.Attribute("top", this.Top);
            visitor.Attribute("width", this.Width);
            visitor.Attribute("height", this.Height);
        }


    }
}