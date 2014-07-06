﻿//BSD, 2014, WinterDev 

// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Collections.Generic;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text;
using System.Threading;
using HtmlRenderer.Entities;
using HtmlRenderer.Handlers;
using HtmlRenderer.Utils;

namespace HtmlRenderer.Dom
{
    /// <summary>
    /// CSS box for iframe element.<br/>
    /// If the iframe is of embedded YouTube or Vimeo video it will show image with play.
    /// </summary>
    public sealed class CssBoxFrame : CssBox
    {
        #region Fields and Consts

        /// <summary>
        /// the image word of this image box
        /// </summary>
        readonly CssImageRun _imageWord;

        /// <summary>
        /// is the iframe is of embeded video
        /// </summary>
        private readonly bool _isVideo;

        /// <summary>
        /// the title of the video
        /// </summary>
        private string _videoTitle;

        /// <summary>
        /// the url of the video thumbnail image
        /// </summary>
        private string _videoImageUrl;

        /// <summary>
        /// link to the video on the site
        /// </summary>
        private string _videoLinkUrl;

        ///// <summary>
        ///// handler used for image loading by source
        ///// </summary>
        //private ImageLoadHandler _imageLoadHandler;

        /// <summary>
        /// is image load is finished, used to know if no image is found
        /// </summary>
        private bool _imageLoadingComplete;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="parent">the parent box of this box</param>
        /// <param name="tag">the html tag data of this box</param>
        internal CssBoxFrame(CssBox parent, BridgeHtmlElement tag, BoxSpec spec)
            : base(parent, tag, spec)
        {

 
    
            this.SetTextContent(new RunCollection(new CssImageRun()));

            //wait for another technique *

            //Uri uri;
            //if (Uri.TryCreate(GetAttribute("src"), UriKind.Absolute, out uri))
            //{
            //    if (uri.Host.IndexOf("youtube.com", StringComparison.InvariantCultureIgnoreCase) > -1)
            //    {
            //        _isVideo = true;
            //        LoadYoutubeDataAsync(uri);
            //    }
            //    else if (uri.Host.IndexOf("vimeo.com", StringComparison.InvariantCultureIgnoreCase) > -1)
            //    {
            //        _isVideo = true;
            //        LoadVimeoDataAsync(uri);
            //    }
            //}

            //if (!_isVideo)
            //{
            //    SetErrorBorder();
            //}
        }

        /// <summary>
        /// Get the href link of the box (by default get "href" attribute)
        /// </summary>
        public override string HrefLink
        {
            get { return _videoLinkUrl ?? GetAttribute("src"); }
        }

        /// <summary>
        /// is the iframe is of embeded video
        /// </summary>
        public bool IsVideo
        {
            get { return _isVideo; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            //if (_imageLoadHandler != null)
            //    _imageLoadHandler.Dispose();
            base.Dispose();
        }


        #region Private methods

        /// <summary>
        /// Load YouTube video data (title, image, link) by calling YouTube API.
        /// </summary>
        private void LoadYoutubeDataAsync(Uri uri)
        {
            //wait for another technique ***
            throw new NotSupportedException("wait for another tech");
            //ThreadPool.QueueUserWorkItem(state =>
            //    {
            //        try
            //        {
            //            var apiUri = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}?v=2&alt=json", uri.Segments[2]));

            //            var client = new WebClient();
            //            client.Encoding = Encoding.UTF8;
            //            client.DownloadStringCompleted += OnDownloadYoutubeApiCompleted;
            //            client.DownloadStringAsync(apiUri);
            //        }
            //        catch (Exception ex)
            //        {
            //            HtmlContainer.ReportError(HtmlRenderErrorType.Iframe, "Failed to get youtube video data: " + uri, ex);
            //            HtmlContainer.RequestRefresh(false);
            //        }
            //    });
        }

        /// <summary>
        /// Parse YouTube API response to get video data (title, image, link).
        /// </summary>
        private void OnDownloadYoutubeApiCompleted(object sender, DownloadStringCompletedEventArgs e)
        {


            //try
            //{
            //    if (!e.Cancelled)
            //    {
            //        if (e.Error == null)
            //        {
            //            var idx = e.Result.IndexOf("\"media$title\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                idx = e.Result.IndexOf("\"$t\"", idx);
            //                if (idx > -1)
            //                {
            //                    idx = e.Result.IndexOf('"', idx + 4);
            //                    if (idx > -1)
            //                    {
            //                        var endIdx = e.Result.IndexOf('"', idx + 1);
            //                        while (e.Result[endIdx - 1] == '\\')
            //                            endIdx = e.Result.IndexOf('"', endIdx + 1);
            //                        if (endIdx > -1)
            //                        {
            //                            _videoTitle = e.Result.Substring(idx + 1, endIdx - idx - 1).Replace("\\\"", "\"");
            //                        }
            //                    }
            //                }
            //            }

            //            idx = e.Result.IndexOf("\"media$thumbnail\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                var iidx = e.Result.IndexOf("sddefault", idx);
            //                if (iidx > -1)
            //                {

            //                    //if (string.IsNullOrEmpty(Width)) Width = "640px";
            //                    //if (string.IsNullOrEmpty(Height)) Height = "480px";

            //                    if (this.Width.IsEmpty)
            //                    {
            //                        this.Width = CssLength.MakePixelLength(640);
            //                    }
            //                    if (this.Height.IsEmpty)
            //                    {
            //                        this.Height = CssLength.MakePixelLength(480);
            //                    }

            //                }
            //                else
            //                {
            //                    iidx = e.Result.IndexOf("hqdefault", idx);
            //                    if (iidx > -1)
            //                    {
            //                        //if (string.IsNullOrEmpty(Width)) Width = "480px";
            //                        if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(480);

            //                        //if (string.IsNullOrEmpty(Height)) Height = "360px";
            //                        if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(360);
            //                    }
            //                    else
            //                    {
            //                        iidx = e.Result.IndexOf("mqdefault", idx);
            //                        if (iidx > -1)
            //                        {
            //                            //if (string.IsNullOrEmpty(Width)) Width = "320px";
            //                            if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(320);

            //                            //if (string.IsNullOrEmpty(Height)) Height = "180px";
            //                            if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(180);
            //                        }
            //                        else
            //                        {
            //                            iidx = e.Result.IndexOf("default", idx);
            //                            // if (string.IsNullOrEmpty(Width)) Width = "120px";
            //                            if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(120);

            //                            //if (string.IsNullOrEmpty(Height)) Height = "90px";
            //                            if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(90);
            //                        }
            //                    }
            //                }

            //                iidx = e.Result.LastIndexOf("http:", iidx, StringComparison.Ordinal);
            //                if (iidx > -1)
            //                {
            //                    var endIdx = e.Result.IndexOf('"', iidx);
            //                    if (endIdx > -1)
            //                    {
            //                        _videoImageUrl = e.Result.Substring(iidx, endIdx - iidx).Replace("\\\"", "\"").Replace("\\", "");
            //                    }
            //                }
            //            }

            //            idx = e.Result.IndexOf("\"link\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                idx = e.Result.IndexOf("http:", idx);
            //                if (idx > -1)
            //                {
            //                    var endIdx = e.Result.IndexOf('"', idx);
            //                    if (endIdx > -1)
            //                    {
            //                        _videoLinkUrl = e.Result.Substring(idx, endIdx - idx).Replace("\\\"", "\"").Replace("\\", "");
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            HandleDataLoadFailure(e.Error, "YouTube");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HtmlContainer.ReportError(HtmlRenderErrorType.Iframe, "Failed to parse YouTube video response", ex);
            //}

            //HandlePostApiCall(sender);
        }

        /// <summary>
        /// Load Vimeo video data (title, image, link) by calling Vimeo API.
        /// </summary>
        private void LoadVimeoDataAsync(Uri uri)
        {
            //plan use resource manager ***
            throw new NotSupportedException("wait for another technique");
            //ThreadPool.QueueUserWorkItem(state =>
            //{
            //    try
            //    {
            //        var apiUri = new Uri(string.Format("http://vimeo.com/api/v2/video/{0}.json", uri.Segments[2]));

            //        var client = new WebClient();
            //        client.Encoding = Encoding.UTF8;
            //        client.DownloadStringCompleted += OnDownloadVimeoApiCompleted;
            //        client.DownloadStringAsync(apiUri);
            //    }
            //    catch (Exception ex)
            //    {
            //        _imageLoadingComplete = true;
            //        SetErrorBorder();
            //        HtmlContainer.ReportError(HtmlRenderErrorType.Iframe, "Failed to get vimeo video data: " + uri, ex);
            //        HtmlContainer.RequestRefresh(false);
            //    }
            //});
        }

        /// <summary>
        /// Parse Vimeo API response to get video data (title, image, link).
        /// </summary>
        private void OnDownloadVimeoApiCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            throw new NotSupportedException("wait for another technique");
            //try
            //{
            //    if (!e.Cancelled)
            //    {
            //        if (e.Error == null)
            //        {
            //            var idx = e.Result.IndexOf("\"title\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                idx = e.Result.IndexOf('"', idx + 7);
            //                if (idx > -1)
            //                {
            //                    var endIdx = e.Result.IndexOf('"', idx + 1);
            //                    while (e.Result[endIdx - 1] == '\\')
            //                        endIdx = e.Result.IndexOf('"', endIdx + 1);
            //                    if (endIdx > -1)
            //                    {
            //                        _videoTitle = e.Result.Substring(idx + 1, endIdx - idx - 1).Replace("\\\"", "\"");
            //                    }
            //                }
            //            }

            //            idx = e.Result.IndexOf("\"thumbnail_large\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                //if (string.IsNullOrEmpty(Width)) //Width = "640";
            //                if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(640);
            //                if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(360);
            //            }
            //            else
            //            {
            //                idx = e.Result.IndexOf("thumbnail_medium", idx);
            //                if (idx > -1)
            //                {
            //                    if (this.Width.IsEmpty)
            //                    {
            //                        this.Width = CssLength.MakePixelLength(200);
            //                    }
            //                    //if (string.IsNullOrEmpty(Width)) 
            //                    //    Width = "200";
            //                    //if (string.IsNullOrEmpty(Height))
            //                    //    Height = "150";

            //                    if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(200);
            //                    if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(150);

            //                }
            //                else
            //                {
            //                    idx = e.Result.IndexOf("thumbnail_small", idx);
            //                    //if (string.IsNullOrEmpty(Width))
            //                    //    Width = "100";
            //                    if (this.Width.IsEmpty) this.Width = CssLength.MakePixelLength(100);

            //                    //if (string.IsNullOrEmpty(Height)) Height = "75";
            //                    if (this.Height.IsEmpty) this.Height = CssLength.MakePixelLength(75);
            //                }
            //            }
            //            if (idx > -1)
            //            {
            //                idx = e.Result.IndexOf("http:", idx);
            //                if (idx > -1)
            //                {
            //                    var endIdx = e.Result.IndexOf('"', idx);
            //                    if (endIdx > -1)
            //                    {
            //                        _videoImageUrl = e.Result.Substring(idx, endIdx - idx).Replace("\\\"", "\"").Replace("\\", "");
            //                    }
            //                }
            //            }

            //            idx = e.Result.IndexOf("\"url\"", StringComparison.Ordinal);
            //            if (idx > -1)
            //            {
            //                idx = e.Result.IndexOf("http:", idx);
            //                if (idx > -1)
            //                {
            //                    var endIdx = e.Result.IndexOf('"', idx);
            //                    if (endIdx > -1)
            //                    {
            //                        _videoLinkUrl = e.Result.Substring(idx, endIdx - idx).Replace("\\\"", "\"").Replace("\\", "");
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            HandleDataLoadFailure(e.Error, "Vimeo");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HtmlContainer.ReportError(HtmlRenderErrorType.Iframe, "Failed to parse Vimeo video response", ex);
            //}

            //HandlePostApiCall(sender);
        }

        /// <summary>
        /// Handle error occurred during video data load to handle if the video was not found.
        /// </summary>
        /// <param name="ex">the exception that occurred during data load web request</param>
        /// <param name="source">the name of the video source (YouTube/Vimeo/Etc.)</param>
        private void HandleDataLoadFailure(Exception ex, string source)
        {
            throw new NotSupportedException("wait for another technique");
            //var webError = ex as WebException;
            //var webResponse = webError != null ? webError.Response as HttpWebResponse : null;
            //if (webResponse != null && webResponse.StatusCode == HttpStatusCode.NotFound)
            //{
            //    _videoTitle = "The video is not found, possibly removed by the user.";
            //}
            //else
            //{
            //    HtmlContainer.ReportError(HtmlRenderErrorType.Iframe, "Failed to load " + source + " video data", ex);
            //}
        }

        /// <summary>
        /// Create image handler for downloading video image if found and release the WebClient instance used for API call.
        /// </summary>
        private void HandlePostApiCall(object sender)
        {
            throw new NotSupportedException("wait for another technique");
            //try
            //{
            //    if (_videoImageUrl != null)
            //    {
            //        _imageLoadHandler = new ImageLoadHandler(HtmlContainer, OnLoadImageComplete);
            //        _imageLoadHandler.LoadImage(_videoImageUrl, HtmlTag);
            //    }
            //    else
            //    {
            //        _imageLoadingComplete = true;
            //        SetErrorBorder();
            //    }

            //    var webClient = (WebClient)sender;
            //    webClient.DownloadStringCompleted -= OnDownloadYoutubeApiCompleted;
            //    webClient.DownloadStringCompleted -= OnDownloadVimeoApiCompleted;
            //    webClient.Dispose();

            //    HtmlContainer.RequestRefresh(IsLayoutRequired());
            //}
            //catch
            //{ }
        }

        /// <summary>
        /// Paints the fragment
        /// </summary>
        /// <param name="g">the device to draw to</param>
        protected override void PaintImp(IGraphics g, PaintVisitor p)
        {

            var rects = new RectangleF(0, 0, this.SizeWidth, this.SizeHeight);
            p.PushLocalClipArea(this.SizeWidth, this.SizeHeight);

            PaintBackground(p, rects, true, true);


            p.PaintBorders(this, rects, true, true);

            var word = this.FirstRun;
            var tmpRect = word.Rectangle;

            tmpRect.Height -= ActualBorderTopWidth + ActualBorderBottomWidth + ActualPaddingTop + ActualPaddingBottom;
            tmpRect.Y += ActualBorderTopWidth + ActualPaddingTop;
            tmpRect.X = (float)Math.Floor(tmpRect.X);
            tmpRect.Y = (float)Math.Floor(tmpRect.Y);
            var rect = Rectangle.Round(tmpRect);

            DrawImage(g, rect);

            DrawTitle(g, rect);

            DrawPlay(g, rect);

            p.PopLocalClipArea();
        }

        /// <summary>
        /// Draw video image over the iframe if found.
        /// </summary>
        private void DrawImage(IGraphics g, Rectangle rect)
        {
            if (_imageWord.Image != null)
            {
                if (_imageWord.ImageRectangle == Rectangle.Empty)
                    g.DrawImage(_imageWord.Image, rect);
                else
                    g.DrawImage(_imageWord.Image, rect, _imageWord.ImageRectangle);

                //if (_imageWord.Selected)
                //{
                //    //g.FillRectangle(GetSelectionBackBrush(true), _imageWord.Left + offset.X, _imageWord.Top + offset.Y, _imageWord.Width + 2, DomUtils.GetCssLineBoxByWord(_imageWord).LineHeight);
                //}
            }
            else if (_isVideo && !_imageLoadingComplete)
            {
                RenderUtils.DrawImageLoadingIcon(g, rect);
                if (rect.Width > 19 && rect.Height > 19)
                {
                    g.DrawRectangle(Pens.LightGray, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        /// <summary>
        /// Draw video title on top of the iframe if found.
        /// </summary>
        private void DrawTitle(IGraphics g, Rectangle rect)
        {
            if (_videoTitle != null && _imageWord.Width > 40 && _imageWord.Height > 40)
            {
                var font = FontsUtils.GetCachedFont("Arial", 9f, System.Drawing.FontStyle.Regular);
                g.FillRectangle(RenderUtils.GetSolidBrush(System.Drawing.Color.FromArgb(160, 0, 0, 0)), rect.Left, rect.Top, rect.Width, FontsUtils.GetFontHeight(font) + 7);

                using (var sf = new StringFormat(StringFormat.GenericTypographic))
                {
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    var titleRect = new RectangleF(rect.Left + 3, rect.Top + 3, rect.Width - 6, rect.Height - 6);
                    g.DrawString(_videoTitle, font, System.Drawing.Color.WhiteSmoke, titleRect.Location, SizeF.Empty);
                }
            }
        }

        /// <summary>
        /// Draw play over the iframe if we found link url.
        /// </summary>
        private void DrawPlay(IGraphics g, Rectangle rect)
        {
            if (_isVideo && _imageWord.Width > 70 && _imageWord.Height > 50)
            {
                var smooth = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var size = new Size(60, 40);
                var left = rect.Left + (rect.Width - size.Width) / 2;
                var top = rect.Top + (rect.Height - size.Height) / 2;
                g.FillRectangle(RenderUtils.GetSolidBrush(System.Drawing.Color.FromArgb(160, 0, 0, 0)), left, top, size.Width, size.Height);

                using (var path = new GraphicsPath())
                {
                    path.AddLine(left + size.Width / 3f + 1, top + 3 * size.Height / 4f, left + size.Width / 3f + 1, top + size.Height / 4f);
                    path.AddLine(left + size.Width / 3f + 1, top + size.Height / 4f, left + 2 * size.Width / 3f + 1, top + size.Height / 2f);
                    path.CloseFigure();
                    g.FillPath(Brushes.White, path);
                }

                g.SmoothingMode = smooth;
            }
        }

        /// <summary>
        /// Assigns words its width and height
        /// </summary>
        /// <param name="g">the device to use</param>
        internal override void MeasureRunsSize(LayoutVisitor lay)
        {
            if (!this.RunSizeMeasurePass)
            {
                MeasureWordSpacing(lay);
                this.RunSizeMeasurePass = true;
            }
            CssLayoutEngine.MeasureImageSize(_imageWord);
        }

        ///// <summary>
        ///// Set error image border on the image box.
        ///// </summary>
        //private void SetErrorBorder()
        //{

        //    this.SetAllBorders(
        //        CssBorderStyle.Solid, CssLength.MakePixelLength(2),
        //        System.Drawing.Color.FromArgb(0xA0, 0xA0, 0xA0));

        //    BorderRightColor = BorderBottomColor = System.Drawing.Color.FromArgb(0xE3, 0xE3, 0xE3);// "#E3E3E3";
        //}

        /// <summary>
        /// On image load process is complete with image or without update the image box.
        /// </summary>
        /// <param name="image">the image loaded or null if failed</param>
        /// <param name="rectangle">the source rectangle to draw in the image (empty - draw everything)</param>
        /// <param name="async">is the callback was called async to load image call</param>
        private void OnLoadImageComplete(Image image, Rectangle rectangle, bool async)
        {
            _imageWord.Image = image;
            _imageWord.ImageRectangle = rectangle;
            _imageLoadingComplete = true;
            this.RunSizeMeasurePass = false;

            //if (_imageLoadingComplete && image == null)
            //{
            //    SetErrorBorder();
            //}

            if (async)
            {
                HtmlContainer.RequestRefresh(IsLayoutRequired());
            }
        }

        private bool IsLayoutRequired()
        {
            var width = this.Width;// new CssLength(Width);
            var height = this.Height;// new CssLength(Height);
            return (width.Number <= 0 || width.UnitOrNames != CssUnitOrNames.Pixels) || (height.Number <= 0 || height.UnitOrNames != CssUnitOrNames.Pixels);
        }

        #endregion
    }
}