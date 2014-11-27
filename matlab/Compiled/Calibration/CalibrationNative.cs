/*
* MATLAB Compiler: 5.1 (R2014a)
* Date: Thu Nov 27 20:09:34 2014
* Arguments: "-B" "macro_default" "-W" "dotnet:GSI.Matlab,Calibration,0.0,private" "-T"
* "link:lib" "-d" "D:\Code\MatlabCompilation\Debug" "-v"
* "D:\Code\matlab\FindRotationAndPixelSize.m"
* "class{Calibration:D:\Code\matlab\FindRotationAndPixelSize.m}" 
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace GSI.MatlabNative
{

  /// <summary>
  /// The Calibration class provides a CLS compliant, Object (native) interface to the
  /// MATLAB functions contained in the files:
  /// <newpara></newpara>
  /// D:\Code\matlab\FindRotationAndPixelSize.m
  /// <newpara></newpara>
  /// deployprint.m
  /// <newpara></newpara>
  /// printdlg.m
  /// </summary>
  /// <remarks>
  /// @Version 0.0
  /// </remarks>
  public class Calibration : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB Compiler Runtime
    /// instance.
    /// </summary>
    static Calibration()
    {
      if (MWMCR.MCRAppInitialized)
      {
        try
        {
          Assembly assembly= Assembly.GetExecutingAssembly();

          string ctfFilePath= assembly.Location;

          int lastDelimiter= ctfFilePath.LastIndexOf(@"\");

          ctfFilePath= ctfFilePath.Remove(lastDelimiter, (ctfFilePath.Length - lastDelimiter));

          string ctfFileName = "Matlab.ctf";

          Stream embeddedCtfStream = null;

          String[] resourceStrings = assembly.GetManifestResourceNames();

          foreach (String name in resourceStrings)
          {
            if (name.Contains(ctfFileName))
            {
              embeddedCtfStream = assembly.GetManifestResourceStream(name);
              break;
            }
          }
          mcr= new MWMCR("",
                         ctfFilePath, embeddedCtfStream, true);
        }
        catch(Exception ex)
        {
          ex_ = new Exception("MWArray assembly failed to be initialized", ex);
        }
      }
      else
      {
        ex_ = new ApplicationException("MWArray assembly could not be initialized");
      }
    }


    /// <summary>
    /// Constructs a new instance of the Calibration class.
    /// </summary>
    public Calibration()
    {
      if(ex_ != null)
      {
        throw ex_;
      }
    }


    #endregion Constructors

    #region Finalize

    /// <summary internal= "true">
    /// Class destructor called by the CLR garbage collector.
    /// </summary>
    ~Calibration()
    {
      Dispose(false);
    }


    /// <summary>
    /// Frees the native resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary internal= "true">
    /// Internal dispose function
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        disposed= true;

        if (disposing)
        {
          // Free managed resources;
        }

        // Free native resources
      }
    }


    #endregion Finalize

    #region Methods

    /// <summary>
    /// Provides a single output, 0-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize()
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", new Object[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize(Object imga)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga);
    }


    /// <summary>
    /// Provides a single output, 2-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize(Object imga, Object imgb)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb);
    }


    /// <summary>
    /// Provides a single output, 3-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize(Object imga, Object imgb, Object imgwidth)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth);
    }


    /// <summary>
    /// Provides a single output, 4-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <param name="deltax">Input argument #4</param>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize(Object imga, Object imgb, Object imgwidth, 
                                     Object deltax)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth, deltax);
    }


    /// <summary>
    /// Provides a single output, 5-input Objectinterface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <param name="deltax">Input argument #4</param>
    /// <param name="deltay">Input argument #5</param>
    /// <returns>An Object containing the first output argument.</returns>
    ///
    public Object FindRotationAndPixelSize(Object imga, Object imgb, Object imgwidth, 
                                     Object deltax, Object deltay)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth, deltax, deltay);
    }


    /// <summary>
    /// Provides the standard 0-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", new Object[]{});
    }


    /// <summary>
    /// Provides the standard 1-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="imga">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut, Object imga)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga);
    }


    /// <summary>
    /// Provides the standard 2-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut, Object imga, Object imgb)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb);
    }


    /// <summary>
    /// Provides the standard 3-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut, Object imga, Object imgb, 
                                       Object imgwidth)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth);
    }


    /// <summary>
    /// Provides the standard 4-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <param name="deltax">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut, Object imga, Object imgb, 
                                       Object imgwidth, Object deltax)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth, deltax);
    }


    /// <summary>
    /// Provides the standard 5-input Object interface to the FindRotationAndPixelSize
    /// MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <param name="deltax">Input argument #4</param>
    /// <param name="deltay">Input argument #5</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public Object[] FindRotationAndPixelSize(int numArgsOut, Object imga, Object imgb, 
                                       Object imgwidth, Object deltax, Object deltay)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth, deltax, deltay);
    }


    /// <summary>
    /// Provides an interface for the FindRotationAndPixelSize function in which the
    /// input and output
    /// arguments are specified as an array of Objects.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of Object output arguments</param>
    /// <param name= "argsIn">Array of Object input arguments</param>
    /// <param name= "varArgsIn">Array of Object representing variable input
    /// arguments</param>
    ///
    [MATLABSignature("FindRotationAndPixelSize", 5, 2, 0)]
    protected void FindRotationAndPixelSize(int numArgsOut, ref Object[] argsOut, Object[] argsIn, params Object[] varArgsIn)
    {
        mcr.EvaluateFunctionForTypeSafeCall("FindRotationAndPixelSize", numArgsOut, ref argsOut, argsIn, varArgsIn);
    }

    /// <summary>
    /// This method will cause a MATLAB figure window to behave as a modal dialog box.
    /// The method will not return until all the figure windows associated with this
    /// component have been closed.
    /// </summary>
    /// <remarks>
    /// An application should only call this method when required to keep the
    /// MATLAB figure window from disappearing.  Other techniques, such as calling
    /// Console.ReadLine() from the application should be considered where
    /// possible.</remarks>
    ///
    public void WaitForFiguresToDie()
    {
      mcr.WaitForFiguresToDie();
    }



    #endregion Methods

    #region Class Members

    private static MWMCR mcr= null;

    private static Exception ex_= null;

    private bool disposed= false;

    #endregion Class Members
  }
}