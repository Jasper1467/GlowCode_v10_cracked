using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;							
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;

using AppModule.NamedPipes;


namespace GlowCode
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class RemoteCOM
    {
        IChannel m_channel;

        public RemoteCOM()
        {
        }

        bool Open(String name)
        {
            if ( m_channel.Open( name ) )
                return true;
            else
            {
                Close();
                return false;
            }
        }
        public bool Close()
        {
            IChannel channel = m_channel;
            m_channel = null;
            if (channel != null)
                return channel.Close();
            else
                return false;
        }
        public String OpenPipe()
        {
            Close();
            m_channel = new NamedPipeChannel();
            if (!Open("\\\\.\\pipe\\GlowCodeCommander"))
                return "<not connected>";
            else
                return m_channel.Recv();
        }
        public String OpenTcpIp(String name)
        {
            Close();
            m_channel = new TcpChannel();
            if (!Open(name))
                return "<not connected>";
            else
                return m_channel.Recv();
        }

        public String Execute(String command)
        {
            if (m_channel == null)
                return "<not connected>";
            else
            {
                m_channel.Send(command);
                return m_channel.Recv();
            }
        }

    }

    interface IChannel
    {
        bool Open(string name);
        bool Close();
        bool Send(String s);
        String Recv();
    }

    class NamedPipeChannel : IChannel
    {

        IntPtr hFile;
        Encoding encoder = Encoding.UTF8;
        int BUFFER_SIZE = 0x10000;
     public bool Open(string name)
        {
            hFile = NamedPipeNative.CreateFile(
                 name,
                 NamedPipeNative.GENERIC_READ | NamedPipeNative.GENERIC_WRITE,
                 0,
                 null,
                 3,
                 0,
                 0);

            if (hFile == (IntPtr)(-1))
                return false;
            uint mode = NamedPipeNative.PIPE_READMODE_MESSAGE | NamedPipeNative.PIPE_WAIT;
            return NamedPipeNative.SetNamedPipeHandleState(hFile, ref mode, IntPtr.Zero, IntPtr.Zero);
        }
        public bool Close()
        {
            return NamedPipeNative.CloseHandle(hFile);
        }
        public bool Send(String s)
        {
            uint num;
            byte[] buffer = encoder.GetBytes(s);
            return NamedPipeNative.WriteFile(hFile, buffer, (uint)buffer.Length, out num, 0);
        }
        public String Recv()
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            uint numRead;
            if (NamedPipeNative.ReadFile(hFile, buffer, (uint)BUFFER_SIZE, out numRead, 0))
            {
                string s = String.Format("{0}", encoder.GetString(buffer, 0, (int)numRead));
                return s;
            }
            else
                return "";
        }
    }

    class TcpChannel : IChannel
    {
        private Socket m_sock;						// Server connection
        Encoding encoder = Encoding.UTF8;
        int BUFFER_SIZE = 0x10000;
        public bool Open(string name)
        {
            bool bResult = false;
            try
            {
                // Close the socket if it is still open
                if (m_sock != null && m_sock.Connected)
                {
                    m_sock.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    m_sock.Close();
                }

                // Create the socket object
                m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string[] sa = name.Split(':');
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(sa[0]), Int32.Parse(sa[1]));
                m_sock.Connect(epServer);
                bResult = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "Server Connect failed: "+ ex.Message );
            }
            return bResult;
        }
        public bool Close()
        {
            m_sock.Close();
            return true;
        }
        public bool Send(String s)
        {
            byte[] buffer = encoder.GetBytes(s);
            return m_sock.Send(buffer) == buffer.Length;
        }
        public String Recv()
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int cRecv = m_sock.Receive(buffer);
            string s = encoder.GetString(buffer, 0, cRecv);
            return s;
        }
    }
}
namespace AppModule
{
    public class Win32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strclassName, string strWindowName);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow( IntPtr hwnd );
    }
}

namespace AppModule.NamedPipes
{
    #region Comments
    /// <summary>
    /// This utility class exposes kernel32.dll methods for named pipes communication.
    /// </summary>
    /// <remarks>
    /// Use the following links for complete information about the exposed methods:
    /// <list type="bullet">
    /// <item>
    /// <description><a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/ipc/base/pipe_functions.asp" target="_blank">Named Pipe Functions</a></description>
    /// </item>
    /// <item>
    /// <description><a href="http://msdn.microsoft.com/library/en-us/fileio/base/file_management_functions.asp" target="_blank">File Management Functions</a></description>
    /// </item>
    /// <item>
    /// <description><a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/handle_and_object_functions.asp" target="_blank">Handle and Object Functions</a></description>
    /// </item>
    /// <item>
    /// <description><a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/debug/base/system_error_codes.asp" target="_blank">System Error Codes</a></description>
    /// </item>
    /// </list>
    /// </remarks>
    #endregion
    public sealed class NamedPipeNative
    {
        #region Comments
        /// <summary>
        /// Outbound pipe access.
        /// </summary>
        #endregion
        public const uint PIPE_ACCESS_OUTBOUND = 0x00000002;
        #region Comments
        /// <summary>
        /// Duplex pipe access.
        /// </summary>
        #endregion
        public const uint PIPE_ACCESS_DUPLEX = 0x00000003;
        #region Comments
        /// <summary>
        /// Inbound pipe access.
        /// </summary>
        #endregion
        public const uint PIPE_ACCESS_INBOUND = 0x00000001;
        #region Comments
        /// <summary>
        /// Pipe blocking mode.
        /// </summary>
        #endregion
        public const uint PIPE_WAIT = 0x00000000;
        #region Comments
        /// <summary>
        /// Pipe non-blocking mode.
        /// </summary>
        #endregion
        public const uint PIPE_NOWAIT = 0x00000001;
        #region Comments
        /// <summary>
        /// Pipe read mode of type Byte.
        /// </summary>
        #endregion
        public const uint PIPE_READMODE_BYTE = 0x00000000;
        #region Comments
        /// <summary>
        /// Pipe read mode of type Message.
        /// </summary>
        #endregion
        public const uint PIPE_READMODE_MESSAGE = 0x00000002;
        #region Comments
        /// <summary>
        /// Byte pipe type.
        /// </summary>
        #endregion
        public const uint PIPE_TYPE_BYTE = 0x00000000;
        #region Comments
        /// <summary>
        /// Message pipe type.
        /// </summary>
        #endregion
        public const uint PIPE_TYPE_MESSAGE = 0x00000004;
        #region Comments
        /// <summary>
        /// Pipe client end.
        /// </summary>
        #endregion
        public const uint PIPE_CLIENT_END = 0x00000000;
        #region Comments
        /// <summary>
        /// Pipe server end.
        /// </summary>
        #endregion
        public const uint PIPE_SERVER_END = 0x00000001;
        #region Comments
        /// <summary>
        /// Unlimited server pipe instances.
        /// </summary>
        #endregion
        public const uint PIPE_UNLIMITED_INSTANCES = 255;
        #region Comments
        /// <summary>
        /// Waits indefinitely when connecting to a pipe.
        /// </summary>
        #endregion
        public const uint NMPWAIT_WAIT_FOREVER = 0xffffffff;
        #region Comments
        /// <summary>
        /// Does not wait for the named pipe.
        /// </summary>
        #endregion
        public const uint NMPWAIT_NOWAIT = 0x00000001;
        #region Comments
        /// <summary>
        /// Uses the default time-out specified in a call to the CreateNamedPipe method.
        /// </summary>
        #endregion
        public const uint NMPWAIT_USE_DEFAULT_WAIT = 0x00000000;
        #region Comments
        /// <summary>
        /// 
        /// </summary>
        #endregion
        public const uint GENERIC_READ = (0x80000000);
        #region Comments
        /// <summary>
        /// Generic write access to the pipe.
        /// </summary>
        #endregion
        public const uint GENERIC_WRITE = (0x40000000);
        #region Comments
        /// <summary>
        /// Generic execute access to the pipe.
        /// </summary>
        #endregion
        public const uint GENERIC_EXECUTE = (0x20000000);
        #region Comments
        /// <summary>
        /// Read, write, and execute access.
        /// </summary>
        #endregion
        public const uint GENERIC_ALL = (0x10000000);
        #region Comments
        /// <summary>
        /// Create new file. Fails if the file exists.
        /// </summary>
        #endregion
        public const uint CREATE_NEW = 1;
        #region Comments
        /// <summary>
        /// Create new file. Overrides an existing file.
        /// </summary>
        #endregion
        public const uint CREATE_ALWAYS = 2;
        #region Comments
        /// <summary>
        /// Open existing file.
        /// </summary>
        #endregion
        public const uint OPEN_EXISTING = 3;
        #region Comments
        /// <summary>
        /// Open existing file. If the file does not exist, creates it.
        /// </summary>
        #endregion
        public const uint OPEN_ALWAYS = 4;
        #region Comments
        /// <summary>
        /// Opens the file and truncates it so that its size is zero bytes.
        /// </summary>
        #endregion
        public const uint TRUNCATE_EXISTING = 5;
        #region Comments
        /// <summary>
        /// Invalid operating system handle.
        /// </summary>
        #endregion
        public const int INVALID_HANDLE_VALUE = -1;
        #region Comments
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        #endregion
        public const ulong ERROR_SUCCESS = 0;
        #region Comments
        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        #endregion
        public const ulong ERROR_CANNOT_CONNECT_TO_PIPE = 2;
        #region Comments
        /// <summary>
        /// All pipe instances are busy.
        /// </summary>
        #endregion
        public const ulong ERROR_PIPE_BUSY = 231;
        #region Comments
        /// <summary>
        /// The pipe is being closed.
        /// </summary>
        #endregion
        public const ulong ERROR_NO_DATA = 232;
        #region Comments
        /// <summary>
        /// No process is on the other end of the pipe.
        /// </summary>
        #endregion
        public const ulong ERROR_PIPE_NOT_CONNECTED = 233;
        #region Comments
        /// <summary>
        /// More data is available.
        /// </summary>
        #endregion
        public const ulong ERROR_MORE_DATA = 234;
        #region Comments
        /// <summary>
        /// There is a process on other end of the pipe.
        /// </summary>
        #endregion
        public const ulong ERROR_PIPE_CONNECTED = 535;
        #region Comments
        /// <summary>
        /// Waiting for a process to open the other end of the pipe.
        /// </summary>
        #endregion
        public const ulong ERROR_PIPE_LISTENING = 536;
        #region Comments
        /// <summary>
        /// Creates an instance of a named pipe and returns a handle for 
        /// subsequent pipe operations.
        /// </summary>
        /// <param name="lpName">Pointer to the null-terminated string that 
        /// uniquely identifies the pipe.</param>
        /// <param name="dwOpenMode">Pipe access mode, the overlapped mode, 
        /// the write-through mode, and the security access mode of the pipe handle.</param>
        /// <param name="dwPipeMode">Type, read, and wait modes of the pipe handle.</param>
        /// <param name="nMaxInstances">Maximum number of instances that can be 
        /// created for this pipe.</param>
        /// <param name="nOutBufferSize">Number of bytes to reserve for the output buffer.</param>
        /// <param name="nInBufferSize">Number of bytes to reserve for the input buffer.</param>
        /// <param name="nDefaultTimeOut">Default time-out value, in milliseconds.</param>
        /// <param name="pipeSecurityDescriptor">Pointer to a 
        /// <see cref="AppModule.NamedPipes.SECURITY_ATTRIBUTES">SECURITY_ATTRIBUTES</see> 
        /// object that specifies a security descriptor for the new named pipe.</param>
        /// <returns>If the function succeeds, the return value is a handle 
        /// to the server end of a named pipe instance.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateNamedPipe(
            String lpName,									// pipe name
            uint dwOpenMode,								// pipe open mode
            uint dwPipeMode,								// pipe-specific modes
            uint nMaxInstances,							// maximum number of instances
            uint nOutBufferSize,						// output buffer size
            uint nInBufferSize,							// input buffer size
            uint nDefaultTimeOut,						// time-out interval
            IntPtr pipeSecurityDescriptor		// SD
            );
        #region Comments
        /// <summary>
        /// Enables a named pipe server process to wait for a client 
        /// process to connect to an instance of a named pipe.
        /// </summary>
        /// <param name="hHandle">Handle to the server end of a named pipe instance.</param>
        /// <param name="lpOverlapped">Pointer to an 
        /// <see cref="AppModule.NamedPipes.Overlapped">Overlapped</see> object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ConnectNamedPipe(
            IntPtr hHandle,									// handle to named pipe
            Overlapped lpOverlapped					// overlapped structure
            );
        #region Comments
        /// <summary>
        /// Connects to a message-type pipe (and waits if an instance of the 
        /// pipe is not available), writes to and reads from the pipe, and then closes the pipe.
        /// </summary>
        /// <param name="lpNamedPipeName">Pointer to a null-terminated string 
        /// specifying the pipe name.</param>
        /// <param name="lpInBuffer">Pointer to the buffer containing the data written 
        /// to the pipe.</param>
        /// <param name="nInBufferSize">Size of the write buffer, in bytes.</param>
        /// <param name="lpOutBuffer">Pointer to the buffer that receives the data 
        /// read from the pipe.</param>
        /// <param name="nOutBufferSize">Size of the read buffer, in bytes.</param>
        /// <param name="lpBytesRead">Pointer to a variable that receives the number 
        /// of bytes read from the pipe.</param>
        /// <param name="nTimeOut">Number of milliseconds to wait for the 
        /// named pipe to be available.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CallNamedPipe(
            string lpNamedPipeName,
            byte[] lpInBuffer,
            uint nInBufferSize,
            byte[] lpOutBuffer,
            uint nOutBufferSize,
            byte[] lpBytesRead,
            int nTimeOut
            );
        #region Comments
        /// <summary>
        /// Creates or opens a file, directory, physical disk, volume, console buffer, 
        /// tape drive, communications resource, mailslot, or named pipe.
        /// </summary>
        /// <param name="lpFileName">Pointer to a null-terminated string that 
        /// specifies the name of the object to create or open.</param>
        /// <param name="dwDesiredAccess">Access to the object (reading, writing, or both).</param>
        /// <param name="dwShareMode">Sharing mode of the object (reading, writing, both, or neither).</param>
        /// <param name="attr">Pointer to a 
        /// <see cref="AppModule.NamedPipes.SecurityAttributes">SecurityAttributes</see> 
        /// object that determines whether the returned handle can be inherited 
        /// by child processes.</param>
        /// <param name="dwCreationDisposition">Action to take on files that exist, 
        /// and which action to take when files do not exist.</param>
        /// <param name="dwFlagsAndAttributes">File attributes and flags.</param>
        /// <param name="hTemplateFile">Handle to a template file, with the GENERIC_READ access right.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified file.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
            String lpFileName,						  // file name
            uint dwDesiredAccess,					  // access mode
            uint dwShareMode,								// share mode
            SecurityAttributes attr,				// SD
            uint dwCreationDisposition,			// how to create
            uint dwFlagsAndAttributes,			// file attributes
            uint hTemplateFile);					  // handle to template file
        #region Comments
        /// <summary>
        /// Reads data from a file, starting at the position indicated by the file pointer.
        /// </summary>
        /// <param name="hHandle">Handle to the file to be read.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives the data read from the file.</param>
        /// <param name="nNumberOfBytesToRead">Number of bytes to be read from the file.</param>
        /// <param name="lpNumberOfBytesRead">Pointer to the variable that receives the number of bytes read.</param>
        /// <param name="lpOverlapped">Pointer to an 
        /// <see cref="AppModule.NamedPipes.Overlapped">Overlapped</see> object.</param>
        /// <returns>The ReadFile function returns when one of the following 
        /// conditions is met: a write operation completes on the write end of 
        /// the pipe, the number of bytes requested has been read, or an error occurs.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(
            IntPtr hHandle,											// handle to file
            byte[] lpBuffer,								// data buffer
            uint nNumberOfBytesToRead,			// number of bytes to read
            out uint lpNumberOfBytesRead,			// number of bytes read
            uint lpOverlapped								// overlapped buffer
            );
        #region Comments
        /// <summary>
        /// Writes data to a file at the position specified by the file pointer.
        /// </summary>
        /// <param name="hHandle">Handle to the file.</param>
        /// <param name="lpBuffer">Pointer to the buffer containing the data to be written to the file.</param>
        /// <param name="nNumberOfBytesToWrite"></param>
        /// <param name="lpNumberOfBytesWritten">Pointer to the variable that receives the number of bytes written.</param>
        /// <param name="lpOverlapped">Pointer to an 
        /// <see cref="AppModule.NamedPipes.Overlapped">Overlapped</see> object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteFile(
            IntPtr hHandle,											// handle to file
            byte[] lpBuffer,							  // data buffer
            uint nNumberOfBytesToWrite,			// number of bytes to write
            out uint lpNumberOfBytesWritten,	// number of bytes written
            uint lpOverlapped								// overlapped buffer
            );
        #region Comments
        /// <summary>
        /// Retrieves information about a specified named pipe.
        /// </summary>
        /// <param name="hHandle">Handle to the named pipe for which information is wanted.</param>
        /// <param name="lpState">Pointer to a variable that indicates the current 
        /// state of the handle.</param>
        /// <param name="lpCurInstances">Pointer to a variable that receives the 
        /// number of current pipe instances.</param>
        /// <param name="lpMaxCollectionCount">Pointer to a variable that receives 
        /// the maximum number of bytes to be collected on the client's computer 
        /// before transmission to the server.</param>
        /// <param name="lpCollectDataTimeout">Pointer to a variable that receives 
        /// the maximum time, in milliseconds, that can pass before a remote named 
        /// pipe transfers information over the network.</param>
        /// <param name="lpUserName">Pointer to a buffer that receives the 
        /// null-terminated string containing the user name string associated 
        /// with the client application. </param>
        /// <param name="nMaxUserNameSize">Size of the buffer specified by the 
        /// lpUserName parameter.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNamedPipeHandleState(
            IntPtr hHandle,
            IntPtr lpState,
            ref uint lpCurInstances,
            IntPtr lpMaxCollectionCount,
            IntPtr lpCollectDataTimeout,
            IntPtr lpUserName,
            IntPtr nMaxUserNameSize
            );
        #region Comments
        /// <summary>
        /// Cancels all pending input and output (I/O) operations that were 
        /// issued by the calling thread for the specified file handle.
        /// </summary>
        /// <param name="hHandle">Handle to a file.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelIo(
            IntPtr hHandle
            );
        #region Comments
        /// <summary>
        /// Waits until either a time-out interval elapses or an instance 
        /// of the specified named pipe is available for connection.
        /// </summary>
        /// <param name="name">Pointer to a null-terminated string that specifies 
        /// the name of the named pipe.</param>
        /// <param name="timeout">Number of milliseconds that the function will 
        /// wait for an instance of the named pipe to be available.</param>
        /// <returns>If an instance of the pipe is available before the 
        /// time-out interval elapses, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WaitNamedPipe(
            String name,
            int timeout);
        #region Comments
        /// <summary>
        /// Retrieves the calling thread's last-error code value.
        /// </summary>
        /// <returns>The return value is the calling thread's last-error code value.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetLastError();
        #region Comments
        /// <summary>
        /// Flushes the buffers of the specified file and causes all buffered data to be written to the file.
        /// </summary>
        /// <param name="hHandle">Handle to an open file.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FlushFileBuffers(
            IntPtr hHandle);
        #region Comments
        /// <summary>
        /// Disconnects the server end of a named pipe instance from a client process.
        /// </summary>
        /// <param name="hHandle">Handle to an instance of a named pipe.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DisconnectNamedPipe(
            IntPtr hHandle);
        #region Comments
        /// <summary>
        /// Sets the read mode and the blocking mode of the specified named pipe.
        /// </summary>
        /// <remarks>
        /// If the specified handle is to the client end of a named pipe and if 
        /// the named pipe server process is on a remote computer, the function 
        /// can also be used to control local buffering.
        /// </remarks>
        /// <param name="hHandle">Handle to the named pipe instance.</param>
        /// <param name="mode">Pointer to a variable that supplies the new mode.</param>
        /// <param name="cc">Pointer to a variable that specifies the maximum 
        /// number of bytes collected on the client computer before 
        /// transmission to the server.</param>
        /// <param name="cd">Pointer to a variable that specifies the 
        /// maximum time, in milliseconds, that can pass before a remote 
        /// named pipe transfers information over the network.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetNamedPipeHandleState(
            IntPtr hHandle,
            ref uint mode,
            IntPtr cc,
            IntPtr cd);
        #region Comments
        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hHandle">Handle to an open object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        #endregion
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(
            IntPtr hHandle);
        #region Comments
        /// <summary>
        /// Sets the security descriptor attributes
        /// </summary>
        /// <param name="sd">Reference to a SECURITY_DESCRIPTOR structure.</param>
        /// <param name="bDaclPresent"></param>
        /// <param name="Dacl"></param>
        /// <param name="bDaclDefaulted"></param>
        /// <returns></returns>
        #endregion
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool SetSecurityDescriptorDacl(ref SECURITY_DESCRIPTOR sd, bool bDaclPresent, IntPtr Dacl, bool bDaclDefaulted);
        #region Comments
        /// <summary>
        /// Initializes a SECURITY_DESCRIPTOR structure.
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="dwRevision"></param>
        /// <returns></returns>
        #endregion
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool InitializeSecurityDescriptor(out SECURITY_DESCRIPTOR sd, int dwRevision);
        #region Comments
        /// <summary>
        /// Private constructor.
        /// </summary>
        #endregion
        private NamedPipeNative() { }
    }
    #region Comments
    /// <summary>
    /// Security Descriptor structure
    /// </summary>
    #endregion
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_DESCRIPTOR
    {
        private byte Revision;
        private byte Sbz1;
        private ushort Control;
        private IntPtr Owner;
        private IntPtr Group;
        private IntPtr Sacl;
        private IntPtr Dacl;
    }
    #region Comments
    /// <summary>
    /// Security Attributes structure.
    /// </summary>
    #endregion
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }
    #region Comments
    /// <summary>
    /// This class is used as a dummy parameter only.
    /// </summary>
    #endregion
    [StructLayout(LayoutKind.Sequential)]
    public class Overlapped
    {
    }
    #region Comments
    /// <summary>
    /// This class is used as a dummy parameter only.
    /// </summary>
    #endregion
    [StructLayout(LayoutKind.Sequential)]
    public class SecurityAttributes
    {
    }
}
