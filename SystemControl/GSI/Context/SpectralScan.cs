using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Context
{
    /// <summary>
    /// Implements a spectral scan sequence.
    /// </summary>
    public class SpectralScan
    {
        /// <summary>
        /// Creates a new spectral scan
        /// </summary>
        /// <param name="context">The scan context</param>
        /// <param name="region">The scan region and definitions.</param>
        /// <param name="doVerticalStacking">If true the stacking is done on the vertical
        /// image axis (perpendicular to the direction of the bytes in the image)</param>
        public SpectralScan(SpectralWorkContext context,
            SpectralScanRectangle region, 
            double stepSize,
            double stepSizeInPx,
            double scanSpeed, bool useSpeedupOffsets, bool doStopMotion=false)
        {
            Context = context;
            Region = region;
            ScanSpeed = scanSpeed;
            StepSize = stepSize;
            StepSizeInPixels = stepSizeInPx;
            UseSpeedupOffsets = useSpeedupOffsets;
            DoStopMotion = doStopMotion;
            
            CalculateScanParameters();
        }

        #region members

        /// <summary>
        /// The spectral context.
        /// </summary>
        public SpectralWorkContext Context { get; private set; }

        /// <summary>
        /// The region of the scan. If updated, a call to calculate scan parameters
        /// must be made.
        /// </summary>
        public SpectralScanRectangle Region { get; set; }

        /// <summary>
        /// The scan speed that is to be used.
        /// </summary>
        public double ScanSpeed { get; private set; }

        /// <summary>
        /// The step size in pixiels.
        /// </summary>
        public double StepSizeInPixels { get; private set; }

        /// <summary>
        /// The step size in pixiels.
        /// </summary>
        public double StepSize { get; private set; }

        /// <summary>
        /// If true, the stage will take some offsets to 
        /// allow for gradient speeup.
        /// </summary>
        public bool UseSpeedupOffsets { get; private set; }

        /// <summary>
        /// If true, uses stop motion configuration to do the scan, otherwise
        /// uses continues motion to scan.
        /// </summary>
        public bool DoStopMotion { get; private set; }

        /// <summary>
        /// Called when the vect
        /// </summary>
        public event EventHandler VectorCaptured;

        /// <summary>
        /// Called when a line is compleated.
        /// </summary>
        public event EventHandler LineCompleated;

        /// <summary>
        /// Called when an image has been captured.
        /// </summary>
        public event EventHandler ImageCaptured;

        /// <summary>
        /// Called after the vector has been written.
        /// </summary>
        public event EventHandler<GSI.Processing.VectorReadyEventArgs> VectorWritten;

        /// <summary>
        /// The total number of images in memory.
        /// </summary>
        public int ImagesInMemory { get; private set; }

        /// <summary>
        /// The total number of captured vectors.
        /// </summary>
        public int CapturedVectors { get; private set; }

        /// <summary>
        /// The size of the line in vectors.
        /// </summary>
        public int LineSize { get; private set; }

        /// <summary>
        /// The width of the scan.
        /// </summary>
        public double ScanWidth { get; private set; }

        /// <summary>
        /// The height of the scan.
        /// </summary>
        public double ScanHeight { get; private set; }

        /// <summary>
        /// The scan offset location (to allow that the first frame of stacking
        /// will the frame where x=Start y=Start.
        /// </summary>
        public double ScanStartOffset { get; private set; }

        /// <summary>
        /// The image width (with respect to the scan axis).
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// The image height (with respect to the scan axis).
        /// </summary>
        public int ImageHeight { get; private set; }

        /// <summary>
        /// The current vector index.
        /// </summary>
        public int CurrentVectorIndex { get; private set; }

        /// <summary>
        /// The current line index.
        /// </summary>
        public int CurrentLineIndex { get; private set; }

        /// <summary>
        /// The total number of pixels scanned.
        /// </summary>
        public long TotalNumberOfPixelsRead { get; private set; }

        /// <summary>
        /// If true recodring images.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// The total number of pixels.
        /// </summary>
        public int NumberOfPixelsInImage { get; private set; }

        #endregion

        #region claculations

        public void CalculateScanParameters()
        {
            // um. 
            ImageWidth = (!Region.InvertedScanAxis ? Context.Camera.Width : Context.Camera.Height);
            ImageHeight = (Region.InvertedScanAxis ? Context.Camera.Width : Context.Camera.Height);

            ScanWidth = Region.InvertedScanAxis ? Region.Height : Region.Width;
            ScanHeight = !Region.InvertedScanAxis ? Region.Height : Region.Width;

            LineSize = (int)Math.Floor(ScanWidth / Region.PixelSizeInPositionUnits);
            NumberOfPixelsInImage = (int)Math.Floor(ScanHeight / Region.PixelSizeInPositionUnits) * LineSize;
        }

        #endregion

        #region Scanning

        void DoScan(Stream strm)
        {
            CalculateScanParameters();

            // claculating the scan regions where the data is taken from
            // assuming that : x=0,y=0 is left top.

            // get the zero position.
            double x = Region.InvertedScanAxis ? Region.Y : Region.X;
            double y = !Region.InvertedScanAxis ? Region.Y : Region.X;

            double stopMotionDeltaX = Region.InvertedScanAxis ? 0 : StepSize;
            double stopMotionDeltaY = Region.InvertedScanAxis ? StepSize : 0;

            // creating the stacking collector.
            Processing.StackingCollector collector = new Processing.StackingCollector(
                ImageWidth, ImageHeight, StepSizeInPixels,
                Region.DoVerticalStacking, false);

            // creating the stacking writer.
            Processing.StackingWriter writer = new Processing.StackingWriter(strm);  
            writer.WriteInitialize(LineSize, Region.InvertedScanAxis ? ImageWidth : ImageHeight,
                collector.NumberOfValuesPerPiexl,collector.StepSize, Region.PixelSizeInPositionUnits);

            // adding the camera collection.
            int numberOfImagesPushedThisRound = 0;
            int imgIndex = 0;
            IsRecording = false;
            double maxY = y + ScanHeight;
            double offset = collector.NumberOfImagesPerStack * StepSizeInPixels * Region.PixelSizeInPositionUnits;
            double yStep = ImageHeight * Region.PixelSizeInPositionUnits;

            int captureStartOffset = (int)Math.Floor(offset / Region.PixelSizeInPositionUnits);
            bool isInLine = false;
            List<int> firstLineOffsets = new List<int>();
            double captureStartPosition = 0;
            double expectedCaptureStartPosition = x;

            Func<DateTime, int> GetPixelOffset = (stamp) =>
            {
                double curposition = captureStartPosition +
                    (stamp - Context.Camera.ZeroTime).TotalSeconds * ScanSpeed;

                int offsetFromZero =
                    (int)Math.Floor(
                    (curposition - expectedCaptureStartPosition) / Region.PixelSizeInPositionUnits);

                return offsetFromZero;
            };

            collector.VectorReady += (s, e) =>
            {
                if (CurrentVectorIndex < LineSize)
                {
                    // write if needed.
                    int pixelOffset = GetPixelOffset(e.InstigatingImage.TimeStamp);
                    if (!isInLine)
                    {
                        firstLineOffsets.Add(pixelOffset);
                        isInLine = true;
                    }
                    writer.WriteVector(e.Vector);
                    CapturedVectors += 1;
                    CurrentVectorIndex += 1;
                    TotalNumberOfPixelsRead += e.Vector.GetLength(0);
                    if (VectorWritten != null)
                        VectorWritten(this, e);
                }
                if (VectorCaptured != null)
                    VectorCaptured(this, null);
            };

            Context.OnImageCaptured += (s, e) =>
            {
                if(!IsRecording)
                {
                    return;
                }
                // write image data.
                int pixelOffset = GetPixelOffset(e.TimeStamp);
                collector.PushImage(e.Data, imgIndex, e.TimeStamp);
                ImagesInMemory = Context.PendingImageCount;
                numberOfImagesPushedThisRound++;
                imgIndex++;
                if (ImageCaptured != null)
                    ImageCaptured(this, null);
            };

            // calculating regions.
            ImagesInMemory = 0;
            CurrentLineIndex = -1;
            TotalNumberOfPixelsRead = 0;

            while (y <= maxY)
            {
                // considering the directionallity of the scan.
                // scanning in the correct direction.
                double x0, y0, x1, y1, vx = 0, vy = 0;
                if (Region.InvertedScanAxis)
                {
                    x0 = y;
                    x1 = y;
                    y0 = x - offset;
                    y1 = x + ScanWidth;
                    vy = ScanSpeed;
                }
                else
                {
                    y0 = y;
                    y1 = y;
                    x0 = x - offset;
                    x1 = x + ScanWidth;
                    vx = ScanSpeed;
                }

                // prepare new line.
                collector.Reset(); // reset the data stack. This is a new line.
                imgIndex = 0; // reset the image index. (newline).
                CurrentVectorIndex = 0; 
                numberOfImagesPushedThisRound = 0;
                IsRecording = false;
                isInLine = false;

                // flush buffered data to disk.
                writer.Flush();

                // collect any memory that needs collecting.
                GC.Collect();

                // need to sleep since stage is funcked up.
                System.Threading.Thread.Sleep(100);

                if (DoStopMotion)
                {
                    Context.PositionControl.DoStopMotionPath(x0, y0, x1, y1, stopMotionDeltaX, stopMotionDeltaY, false,
                    () =>
                    {
                        // start
                        Context.Camera.StopCapture();
                        Context.Camera.SetZeroTime();
                        captureStartPosition = Context.PositionControl.PositionX;

                        // set the recording state.
                        IsRecording = true;

                        // reached end position.
                        CurrentLineIndex += 1;
                    },
                    ()=>
                    {
                        // point reached
                        Context.Camera.Capture();
                        // System.Threading.Thread.Sleep(100);
                    },
                    ()=>
                    {
                        // end
                        Context.StopCapture();
                    });
                }
                else {
                    Context.PositionControl.DoPath(x0, y0, x1, y1, vx, vy, false, UseSpeedupOffsets,
                    () =>
                    {
                        // the zero time of the camera.
                        Context.Camera.SetZeroTime();

                        captureStartPosition = Context.PositionControl.PositionX;

                        // set the recording state.
                        IsRecording = true;

                        // reached start position.
                        Context.StartCapture();

                        // reached end position.
                        CurrentLineIndex += 1;
                    },
                    () =>
                    {
                        // reched end position. stop recording.
                        Context.StopCapture();
                    });
                }

                // waiting for images in memory before continue (memory issues),
                // and not to allow stacking of overflow images;
                do
                {
                    System.Threading.Thread.Sleep(20);
                }
                while (Context.IsProcessingPendingImages || Context.PendingImageCount > 0);

                int numberOfDumptedEmptyVectorsAtEOL = LineSize - CurrentVectorIndex;

                if (numberOfDumptedEmptyVectorsAtEOL > 0)
                {
                    for (int i = 0; i < numberOfDumptedEmptyVectorsAtEOL; i++)
                    {
                        writer.WriteVector(new byte[ImageHeight, collector.NumberOfValuesPerPiexl]);
                    }
                }

                ImagesInMemory = Context.PendingImageCount;
                
                // called when the line is compleated.
                if (LineCompleated != null)
                    LineCompleated(this, null);

                y = y + yStep;
            }

            Context.Dispose();
            writer.Dispose();

            System.Diagnostics.Trace.WriteLine("First vector in line positions: " +
                string.Join(",", firstLineOffsets.Select(v => v.ToString())));
        }

        public void Scan(Stream strm, Action callOnComplete = null, bool async = true)
        {
            if (async)
                Task.Run(() =>
                {
                    DoScan(strm);
                    if (callOnComplete != null)
                        callOnComplete();
                });
            else
            {
                DoScan(strm);
                if (callOnComplete != null)
                    callOnComplete();
            }
        }

        #endregion
    }

    public class SpectralScanRectangle
    {
        public SpectralScanRectangle(double x, double y, double width, double height,
            double pixelSizeInPositionUnits, bool doVerticalStacking, bool invertedAxis)
        {
            X=x;
            Y=y;
            Width=width;
            Height=height;
            DoVerticalStacking = doVerticalStacking;
            InvertedScanAxis = invertedAxis;
            PixelSizeInPositionUnits = pixelSizeInPositionUnits;
        }

        /// <summary>
        /// Start poisiton
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Start poisiton
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// The width of the scan.
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// The height of the scan.
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// The size of one pixel in position units.
        /// </summary>
        public double PixelSizeInPositionUnits { get; private set; }

        /// <summary>
        /// If ture the 
        /// </summary>
        public bool InvertedScanAxis { get; set; }

        /// <summary>
        /// If true the stacking is done on the vertical
        /// image axis (perpendicular to the direction of the bytes in the image)
        /// </summary>
        public bool DoVerticalStacking { get; set; }
    }
}
