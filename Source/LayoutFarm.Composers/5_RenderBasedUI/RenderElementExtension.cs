﻿//Apache2, 2014-2016, WinterDev

namespace LayoutFarm.InternalUI
{
    static class RenderElementExtension
    {
        public static void AddChild(this RenderElement renderBox, UIElement ui)
        {
            renderBox.AddChild(ui.GetPrimaryRenderElement(renderBox.Root));
        }
    }
}