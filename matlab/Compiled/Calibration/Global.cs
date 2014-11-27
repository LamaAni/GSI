/*
* MATLAB Compiler: 5.1 (R2014a)
* Date: Thu Nov 27 13:14:09 2014
* Arguments: "-B" "macro_default" "-W" "dotnet:GSI.Matlab,Global,0.0,private" "-T"
* "link:lib" "-d" "C:\Users\gsi\Documents\MATLAB\GSIProjMatlab\for_testing" "-v"
* "D:\Code\matlab\FindRotationAndPixelSize.m"
* "class{Global:D:\Code\matlab\FindRotationAndPixelSize.m}" 
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace GSI.Matlab
{

  /// <summary>
  /// The Global class provides a CLS compliant, MWArray interface to the MATLAB
  /// functions contained in the files:
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
  public class Global : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB Compiler Runtime
    /// instance.
    /// </summary>
    static Global()
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
    /// Constructs a new instance of the Global class.
    /// </summary>
    public Global()
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
    ~Global()
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
    /// Provides a single output, 0-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize()
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize(MWArray imga)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize(MWArray imga, MWArray imgb)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize(MWArray imga, MWArray imgb, MWArray imgwidth)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
    /// </summary>
    /// <remarks>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="imga">Input argument #1</param>
    /// <param name="imgb">Input argument #2</param>
    /// <param name="imgwidth">Input argument #3</param>
    /// <param name="deltax">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize(MWArray imga, MWArray imgb, MWArray imgwidth, 
                                      MWArray deltax)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth, deltax);
    }


    /// <summary>
    /// Provides a single output, 5-input MWArrayinterface to the
    /// FindRotationAndPixelSize MATLAB function.
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
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray FindRotationAndPixelSize(MWArray imga, MWArray imgb, MWArray imgwidth, 
                                      MWArray deltax, MWArray deltay)
    {
      return mcr.EvaluateFunction("FindRotationAndPixelSize", imga, imgb, imgwidth, deltax, deltay);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut, MWArray imga)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut, MWArray imga, MWArray imgb)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut, MWArray imga, MWArray imgb, 
                                        MWArray imgwidth)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut, MWArray imga, MWArray imgb, 
                                        MWArray imgwidth, MWArray deltax)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth, deltax);
    }


    /// <summary>
    /// Provides the standard 5-input MWArray interface to the FindRotationAndPixelSize
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
    public MWArray[] FindRotationAndPixelSize(int numArgsOut, MWArray imga, MWArray imgb, 
                                        MWArray imgwidth, MWArray deltax, MWArray deltay)
    {
      return mcr.EvaluateFunction(numArgsOut, "FindRotationAndPixelSize", imga, imgb, imgwidth, deltax, deltay);
    }


    /// <summary>
    /// Provides an interface for the FindRotationAndPixelSize function in which the
    /// input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// M-Documentation:
    /// converting the images to matrix form.
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void FindRotationAndPixelSize(int numArgsOut, ref MWArray[] argsOut, MWArray[] 
                               argsIn)
    {
      mcr.EvaluateFunction("FindRotationAndPixelSize", numArgsOut, ref argsOut, argsIn);
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
