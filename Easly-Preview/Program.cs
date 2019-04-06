namespace EaslyPreview
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using BaseNode;
    using EaslyController.Controller;
    using EaslyController.Layout;
    using EaslyDraw;
    using PolySerializer;

    /// <summary>
    /// A class to convert Easly source files to a bitmap.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The standard DPI for the default font size.
        /// </summary>
        public const double Dpi = 96;

        static int Main(string[] args)
        {
            int Result;

            try
            {
                // Require source and destination file.
                if (args.Length >= 2)
                {
                    string SourceFilePath = args[0];
                    if (File.Exists(SourceFilePath))
                    {
                        string DestinationFilePath = args[1];

                        Result = PreviewFile(SourceFilePath, DestinationFilePath);
                    }
                    else
                        return -2;
                }
                else
                    Result = -1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Result = -3;
            }

            return Result;
        }

        private static int PreviewFile(string sourceFilePath, string destinationFilePath)
        {
            using (FileStream SourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream DestinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
                {
                    return PreviewFile(SourceStream, DestinationStream);
                }
            }
        }

        /// <summary>
        /// Reads <paramref name="sourceStream"/> as an easly source code and from it create a bitmap using the BMP format in <paramref name="destinationStream"/>.
        /// </summary>
        /// <param name="sourceStream">The source code.</param>
        /// <param name="destinationStream">The destination bitmap.</param>
        /// <returns></returns>
        private static int PreviewFile(FileStream sourceStream, FileStream destinationStream)
        {
            // Create a serializer than can read text or binary formats.
            Serializer Serializer = new Serializer();
            Serializer.Format = SerializationFormat.BinaryPreferred;

            // Reads the source stream as an easly source code.
            INode RootNode = Serializer.Deserialize(sourceStream) as INode;

            // Create a controller for this source code.
            ILayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(RootNode);
            ILayoutController Controller = LayoutController.Create(RootIndex);

            Size ViewSize;

            // Create and open a visual on which to render.
            DrawingVisual DrawingVisual = new DrawingVisual();
            using (DrawingContext dc = DrawingVisual.RenderOpen())
            {
                // Create a draw context using default fonts and brushes.
                DrawContext DrawContext = DrawContext.CreateDrawContext(new Typeface("Consolas"), 10, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, null, null, hasCommentIcon: true, displayFocus: true);

                // Create a view using custom frames (for how code is organized).
                ILayoutControllerView ControllerView = LayoutControllerView.Create(Controller, EaslyEdit.CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
                ControllerView.SetCommentDisplayMode(EaslyController.Constants.CommentDisplayModes.All);

                // Run the measure step to obtain the bitmap size.
                ControllerView.MeasureAndArrange();
                ViewSize = ControllerView.ViewSize;

                // Draw a white background.
                dc.DrawRectangle(Brushes.White, null, new System.Windows.Rect(0, 0, ViewSize.Width.Draw, ViewSize.Height.Draw));

                // Draw the source code.
                DrawContext.SetWpfDrawingContext(dc);
                ControllerView.Draw(ControllerView.RootStateView);
            }

            // At this stage, the visual is ready with drawing data.

            // Create a bitmap and renders the visual on it.
            RenderTargetBitmap Bitmap = new RenderTargetBitmap((int)ViewSize.Width.Draw, (int)ViewSize.Height.Draw, Dpi, Dpi, PixelFormats.Default);
            Bitmap.Render(DrawingVisual);

            // Save the bitmap to the destination file with a BMP format encoder.
            BmpBitmapEncoder Encoder = new BmpBitmapEncoder();
            BitmapFrame Frame = BitmapFrame.Create(Bitmap);
            Encoder.Frames.Add(Frame);
            Encoder.Save(destinationStream);

            return 0;
        }
    }
}
