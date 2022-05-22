GlowCode 10.0
World's Fastest Profiler for Native and Managed Code

Requirements:
32-bit or 64-bit version of
Windows 10, Windows 8, Windows 7, XP, Vista, or Server.

General comments:
Try profiling scribble.exe, GlowCode itself, or your production executables.

=======================================

Latest installation files:

For 64-bit platforms:
GlowCode-x64-Setup-10.0.1002.1.exe
June 27, 2017 
v10.0 Build 1002

For 32-bit platforms:
GlowCode-x86-Setup-10.0.1002.1.exe
June 27, 2017
v10.0 Build 1002

=======================================

Release notes:

June 27, 2017 v10.0 Build 1002
1. Improved the help system.

Apr-26-2016 v10.0 Build 1002
1. Fixed failure to launch and find leaks properly on Windows Server 2012 R2.

Mar-14-2016 v10.0 Build 1001
1. Added support for Windows 10.
2. Removed warning message related to Microsoft DIA Symbol Handler.
 
Jun-13-2014 v9.2 Build 3005
1. Fixed failure of saved CallTree to properly save stackTrace symbolic names.

Apr-09-2014 v9.2 Build 3004
1. Added ProfilingController.ForceNativeHooks commander method to allow manual loading of native hooks as a workaround for late-loaded dll's.

Apr-02-2014 v9.2 Build 3003
1. Extended timeout and retry options to allow for slow loading profile targets.

Aug-01-2013 v9.1 Build 3002
1. Fixed negative time with managed code function unwind.
2. Fixed startup hang with Office Managed Code Add-in.
3. Fixed Export to CSV-Excel mismatches column data, Trac#87.
4. Expanded Attach mode options on Windows x64. Previous versions used the CreateRemoteThread, which sometimes did not work with services. This version adds support for attach using DebugActiveProcess, which works for attaching to 64-bit Services. Generally speaking, attaching to a service requires that GlowCode x64 be run with Admin Privileges.

Nov-16-2012 v9.0 Build 3001
1. Added support for Visual Studio 2012

Nov-01-2012 v9.0 Build 3000
1. Added support for Windows 8.

Oct-4-2012 v9.0 Build 2008
1. Fixed minor error in gcSym.exe

Aug-25-2012 v9.0 Build 2007
1. Added support for running with VS2010 msdia100.dll.
2. Fixed hidden profiler controller error, trac#85.

Feb-07-2012 v8.2 Build 2006
1. Removed dependency in GetProcAddressHelper.exe on msvcr90.dll.

Sep-14-2011 v8.2 Build 2006
1. Added support for Peeking at running programs for target info. Useful particularly for getting target info for ASP.net apps that use WebDev WebServer.
2. Added better support in Profiler Controller Open Page for project rename, copy, and delete.

Jul-15-2011 v8.1 Build 2005
1. Added support for fibers (in Profiler: Setup > Advanced > Fiber Aware checkbox).
2. Fixed failure of "Collect profile data" checkbox to disable collection of profile data (fixes Trac #68).
3. Fixed failure of gcSym.exe (v1.3) to find symbols in some vs2005 builds (fixes Trac #70).
4. Improved filtering of symbols for correct code section location in gc6prof.dll.

Mar-25-2011 v8.0 Build 2004
1. Added command in Running page to hook dll's that were late loaded and missed.
2. Improved UI for Profiling Controller.
3. Fixed "Source File" column in CALL Summary


Dec-16-2010 v8.0 Build 2003
1. Fixed small UI issues: allow single click on CheckedListBox entries, update StackTrace , Trac #58, tiling issue with Commander Pane, toggle visibility of Profiler Controller.


Dec-07-2010 v8.0 Build 2002
1. Added byte counting to Caller/Callee Details view.

Dec-01-2010 v8.0 Build 2001
1. Added byte counting to allocation reports (ref #8).
2. Fixed failure of gcSym.exe on x64 platforms to access symbol server properly (ref #55).

Sep-02-2010 v8.0 Build 2000
1. New: added user-selectable clock options in the Profiling Controller > Setup > Clock tabpage. For details, see Help > Contents > Latest Features.
2. Moved "Exclude wait time" from the Running page to the CallTree viewer.
3. New for managed code: CLR Object Snapshot Diffs: In the "+-Object" column, for each ObjectID, GlowCode now shows a positive value to indicate the number of objects added since the previous snapshot, or a negative value to show the number of objects subtracted since the previous snapshot.
4. Fixed failure to display CLR Object Snapshot values for x64 target applications.
5. Fixed failure to track handles properly on x64.


Jun-11-2010 v7.2 Build 1099
1. Fixed failure of managed profiler to work with .NET Framework 4.0.


Jun-01-2010 v7.2 Build 1098
1. In Saved/Loaded Call Explorer, in the Detailed Function window, fixed failure to properly look up function names of caller and callee functions.
2. Call Explorer window position, size, and column settings are saved when you have advanced past the Profiling Controller Stop tab to the Close tab.
3. Function Hooks window position, size, and columns are saved.

May-11-2010 V7.2 Build 1097
1. Improved good hook safety and fair hook safety usability on x64 executables.
2. Added gcsym.exe=EarlyCoInitialize environment variable option. Use option when gcsym.exe execution on WOW64 platform when cannot find msvcrp71.dll error occurs.

Mar-29-2010 V7.2 Build 1096
1. Fixed the "Cannot InitHiddenEntry: kernel32..." errors.
2. Fixed failure to properly recognize mult-byte nops and some SSE2 instructions. This prevented some hookable functions on x64 from being hooked.
3. Fixed failure to unwind exceptions properly when exception handler is itself profiled.
4. Fixed multithread access issue with LoadLibraryTable.
5. Fixed very slow finding functioniD_2_hookstatus lookup on x64.
6. Added included and exclude filter options to profiler.

Feb-10-2010 V7.2 Build 1095
1. Fixed failure to properly display new calltree nodes when parent is already displayed.
2. Fixed gcSym.exe failure to properly lookup 64-bit symbols from Dll's in system32 directory.
3. Changed thread time column in calltree to display total of top-level children rather than thread lifetime. This change based on general user requests.

Dec-22-2009 V7.2 Build 1094
1. Added "Exclude wait time" feature and trackbar in Running Page of Profiling Controller.
2. Corrected double counting of heap blocks
3. Made "Threshold time" formatting language-independent.

Oct-09-2009 V7.1 Build 1093
1. On all platforms, fixed failure during resource tracking of "Kernel32 Heap" that occurred when attaching to running applications with TlsAlloc values >= 0x40;

Sep-17-2009 V7.1 Build 1092
1. For GlowCode-x64, GetLastError() values were sometimes being changed improperly in profiled functions and during resource tracking. This has been corrected.
2. On all platforms, improved presentation of "Missing DIA registration" message box.
3. On all platforms, improved Pipe timeout status presentation and cancel option.

Aug-04-2009 V7.1 Build 1091
1. Added Function Detail window, accessed from Summary pane of CallExplorer, right mouse click, "Show Caller/Callee Details".

Jul-01-2009 V7.0 Build 1090
1. On all platforms, _alloca_probe added to list of excluded function names.

Jun-24-2009 V7.0 Build 1089
1. On all platforms, msdia80.dll from vs2005 is supported as well as msdia90.dll for reading pdb files.

Jun-19-2009 V7.0 Build 1088
1. On 64-bit platforms, insured that floating point return values are always preserved on profiled functions.

Release notes:
Jun-18-2009 V7.0 Build 1087
1. On 64-bit platforms, added safety check for 0x0000 opcode. Also prevented resource tracking failure when api call has more than 4 arguments.
2. On all platforms, filter out names with "`" content.
3. On all platforms, GlowCode tests at startup to warn if CLSID_DiaSource is not registered, and provides registration instructions.
4. On all platforms, a new CLSID is assigned to GlowCode.CLR Profiler.7 to prevent overwriting by older GlowCode 6.x installations.

Jun-04-2009 V7.0 Build 1086
1. On all platforms, corrected stackTrace recursion error (stack overflow) when mixed "Resource tracking" options are enabled.
2. Fixed IMallocSpy error that occurs if pending Free exists during process exit.

May-18-2009 V7.0 Build 1085
1. On all platforms, fixed failure of hook dialog to show correct module and hook status.
2. Improved symbol-handling on 64-bit platforms, by reverting to using the 32-bit gcSym.exe, since Microsoft apparently doesn't support the DIA symbol library in 64-bit.

May-07-2009 V7.0 Build 1084
1. Fixed failure on WindowsXP x64 platform to launch x32 native targets.
2. On all x64 platforms, improved reliability of launching managed targets.
3. Error message prompts are now On as the default setting.

Mar-23-2009 V7.0 Build 1083
1. Fixed failure to attach to already running targets.
2. Improved stacktraces on x64 targets

Mar-14-2009 V7.0 Build 1082
1. Added support for stacktraces for non-heap resource allocations in Win32 platform.
2. Implemented x64 Managed profiling.

Feb-11-2009 V7.0 Build 1081
1. Implemented all x64 features except Managed profiling.

Dec-03-2008 V6.2 x64 Build 1080
1. Added heap tracking and leak detection. 
   Enable using Setup->Native->Resource tracking->Kernel32 Heap checkbox
   and Setup->Native->StackTrace enabled

Nov-03-2008 V6.2 x64 Build 1070
1. Implemented support for unmanaged profiling of x64 executables.
   Unzip installation zip file to any writable directory and run.
2. x64 features remaining to be implemented:
	a. Managed profiling
	b. Resource tracking and heap leak detection.
	c. Hook status details in Hooks dialog.


Oct-23-2008 V6.2 Build 1069
GlowCode6p2-Setup-1069.exe
1. Improved error handling when creating project root directory.
2. Time display formatting now assumes signed time values.
3. Sys.SetProcessAffinity will set the process affinity in the target process, then essentially disable external calls to Kernel32::SetProcessAffinity to prevent unintended changes to process affinity.


Jul-07-2008 V6.2 Build 1068
GlowCode6p2-Setup-1068.exe
1. Improved dialog for creating new projects. Features include selecting target exe, copying previous settings, and guaranteeing unique project names.
2. Fixed error using '@' character in program arguments

Jun-25-2008 V6.2 Build 1067
GlowCode6p2-Setup-1067.exe
1. Added byte counts to stack trace reports.

Jun-20-2008 V6.2 Build 1066
GlowCode6p2-Setup-1066.exe
1. Fixed occasional miscounting of heap blocks during multithreaded HeapAlloc.
2. Added automatic hide option for Profiler Controller UI after running resumes.
3. Improved subpane resizing in CallExplorer during outsized conditions.


Jun-06-2008 V6.2 Build 1065
GlowCode6p2-Setup-1065.exe
1. Fixed memory tracking issue relating to HeapReAlloc on Vista.
2. Extended maximum stack trace size to 255.
3. Changed toolbar behavior slightly to support single-click focus.

May-20-2008 V6.2 Build 1064
GlowCode6p2-Setup-1064.exe
1. Fixed memory leak detection issues relating to large memory blocks as well heap walking in Vista.

May-07-2008 V6.2 Build 1063
GlowCode6p2-Setup-1063.exe
1. Fixed memory tracking anomaly on Vista that resulted in double stack trace signatures.

Apr-15-2008 V6.2 Build 1062
GlowCode6p2-Setup-1062.exe
1. Improved leak detection with large memory blocks, debug heaps, and vs2008 c-runtime.
2. Tested and verified profiling with vs2008 mfc builds.
 
Mar-17-2008 V6.2 Build 1061
GlowCode6p2-Setup-1061.exe
1. Improved speed of CallExplorer file save/open/load by x10. New format no longer reads pre-V6.2 CallExplorer files, which can be read by using a v6.1 installation.
2. Improved memory efficency of CallExplorer viewer by 50 percent.

Jan-20-2008 V6.1 Build 1060
GlowCode6p1-Setup-1060.exe
1. Updated gcSym.exe to work properly with cv symbol files.

Dec-28-2007 V6.1 Build 1059
GlowCode6p1-Setup-1059.exe
1. Updated gcSym.exe to work properly with .pdb symbol files generated by Intel compiler.

Dec-17-2007 V6.1 Build 1058
GlowCode6p1-Setup-1058.exe
1. Fixed premature dismissal of VisualStudio when viewing source file from within GlowCode.
2. Reduced likelyhood of exceptions in memory allocation stacktraces.
3. Improved safety of function hooks.

Nov-21-2007 V6.1 Build 1057
GlowCode6p1-Setup-1057.exe
1. Added command to CallTree viewer for use by remote controllers:
Viewer.CallTree.Export <viewName> <filename> <iExportType> <bAllRows> <bAllCols>
viewName is one of the following:
	CallTree
	CallTreeFunctionSummary
	CallTreeManagedHeap
	CallTreeNativeMemory
	CallTreeNativeStackTrace
	StackFunctionNames
filename is any valid filename
iExportType is one of the following
	0 = XML tree
	1 = Comma-delimited
	2 = csv-Excel
for example:
Viewer.CallTree.Export CallTreeFunctionSummary test.csv 2 true true

Nov-21-2007 V6.1 Build 1056
GlowCode6p1-Setup-1056.exe
1. Added support for csv-Excel export, a Excel-optimized csv format that removes embedded commas from numbers and text.
2. Corrected failure when user attempted to export to an XML or CSV file which was open in another application.

Oct-29-2007 V6.1 Build 1055
GlowCode6p1-Setup-1055.exe
1. Fixed various failures related to source filenames.

Oct-24-2007 V6.1 Build 1054
GlowCode6p1-Setup-1054.exe
1. Fixed failure of gcSym.bat to correctly reference installations with space in path. Path is now properly quoted.

Oct-21-2007 V6.1 Build 1053
GlowCode6p1-Setup-1053.exe
1. Prompt for retry in connection timeout with target.
2. Prompt for deleting old symbol files when reusing project.
3. Correctly update link to new gcSym.exe.


Oct-11-2007 V6.1 Build 1052
GlowCode6p1-Setup-1052.exe
1. Added Running->Ending options tabpage to Profiling Controller.
2. Improved help topics, in particular, for memory leak detection.
3. Added ability to view grayed-out Profiling Controller tabpages from active tabpage.

Sept-27-2007 V6.1 Build 1051
GlowCode6p1-Setup-1051.exe
1. Source file info is now displayed in the "Return functions" stack trace in "Native Memory". Disable feature by adding "dbgSym.ShowSrcFileInfo false" to the Profiling Controller->Setup->Custom textbox.
2. Improved behavior of Profiling Controller so it advances automatically when target app exits.
3. Added additional commands to Profiling Controller for use by remote controllers:
	ProfilingController.SetStartMode 0=Launch, 1=Attach by name, 2=Attach by processId
	ProfilingController.SetStopMode  0=Detach, 1=ExitProcess, 2=TerminateProcess.
4. Fixed failure of CallExplorer to properly save memory counts.
5. Improved handling in gcSym.exe for "module not found."

Aug-22-2007 V6.1 Build 1050
GlowCode6p1-Setup-1050.exe
1. Added support for controlling GlowCode remotely by another program.
This makes it easy to control GlowCode using other programs such as Windows Scripting Host.
Read test.js for details.
2. Fixed "Set selected in as new context" popup menu item in CallTree viewer.

Jun-19-2007 V6.1 Build 1049
GlowCode6p1-Setup-1049.exe
Supports right-click a function in CallTree to view its source file in VisualStudio.
Fixed defect with Detach/Attach introduced in build 1048.
Added support of late-loaded dlls in ws2003r2ee

May-15-2007 V6.1 Build 1048
GlowCode6p1-Setup-1048.exe
Improved loading of late-loaded dll symbols.

May-02-2007 V6.1 Build 1047
GlowCode6p1-Setup-1047.exe
Added support display of hook status in Hooks setup dialog.

Apr-19-2007 Build 1046
GlowCode6-Setup-1046.exe
Added support for XmlTree export format.
Fixed potential failure when target has large number (>50) running threads.

Apr-12-2007 Build 1045
GlowCode6-Setup-1045.exe
Added ProcessAffinity setting in FastBuffer tabpage. Default value of 1 prevents timing errors that occurred on some multi-processor systems.
Removed leading space in csv export that causes Excel parsing error.

Apr-08-2007 Build 1044
GlowCode6-Setup-1044.exe
Improved symbol handling in gcSym.exe of public symbols.

Apr-04-2007 Build 1043
GlowCode6-Setup-1043.exe
Fixed tree display wrap issue on Vista platform.
Fixed managed-code snapshot find references failure.

Mar-26-2007 Build 1042
GlowCode6-Setup-1042.exe
Module names now sorted in the "Select loaded modules for hooks" dialog
Added "Load .gcSym file info now" context menu item to "Function Hooks" dialog

Mar-19-2007 Build 1041
GlowCode6-Setup-1041.exe
Improved symbol handling in gcSym.exe.
Improved symbol loading in gc6prof.dll.
Minor cosmetic change, renamed "Hook criteria" dialog to "Modules hooked".
Rename symbol files *.nativeSym to *.nativeSym.old after conversion to *.gcSym format.

Mar-12-2007 Build 1040
GlowCode6-Setup-1040.exe
Easier native symbol management with tree view enable/disable of hooked modules and functions
Automatic loading of debug symbols in late-loaded dll's. Setup > Preload is no longer needed.
Correct handling of license keys even when mangled by mail clients.
Changed the label "Project Wizard" to "Profiling Controller" to clarify its purpose.

Jan-10-2007 Build 1039
GlowCode6-Setup-1039.exe
Improved Preload to search for dependencies in directory of preloaded dll.
Fixed error when dblclk heap allocation in CallExplorer.

18-Dec-2006 Build 1038
GlowCode6-Setup-1038.exe
Fixed open file error in CallExplorer viewer.
Fixed ManagedHeap tabpage display in CallExplorer.
Updated online help documentation.

08-Dec-2006 Build 1037
GlowCode6-SetupBuild1037.exe
Improved support for late-loaded dlls
Improved stackwalk on heap allocations
Fixed save file error in CallTree viewer

01-Dec-2006 Build 1036
Improved support for large records in FastBuffer
Added support for floating licenses.
Increased pipe.transact receive timeout to 15 sec.
Default StackWalk is Off.
Improved internal updating when Dll's loaded.

27-Nov-2006 Build 1035
Improved FastBuffer wrapping
Changed StackTrace default to none.
Extended timeout for PipeRecv

17-Nov-2006 Build 1034
Improved symbol loading behaviour and diagnostics
Added stack traces for Kernel32 heap allocations (and most C-runtime heaps).
New license management for LiquidWeb cgi creation.

30-Oct-2006 Build 1033
Improved rehooking of late-loaded functions
Fixed help window z-order problem.
Fixed failure to handle spaces in module paths
Fixed incorrect handling of module paths in hook criteria

Fixed exception on right-click below Summary area
25-Oct-2006 Build 1032
While setting function hooks, the UI is disabled to prevent user from attempting conflicting or confusing actions.
Added more verbose symbol loading messages.
Updated help dialog and buttons.

23-Oct-2006 Build 1031
Added options to hide, unsync motion of ProjectWizard
Added *.dll to hook criteria option
Update help documentation

20-Oct-2006 Build 1030
Added leak detection sample programs
Fixed minor UI defect on Wizard->Target tab.

19-Oct-2006
Added support for module file extensions in the Hooks criteria.
Added a two level tree view of modules and functions in the function trigger dialog.

17-Oct-2006
Added FastBuffer enable/disable data collection checkbox to Run page.

12-Oct-2006
Improved exception handling during function profiling.

11-Oct-2006
Added stack segment searching when looking for heap leaks

04-Oct-2006
Added optional module summary to Summary pane of CallExplorer. Enable/disable using Right-Mouse "Summarize by Module" popup menu.
Fixed format error in "% of Total Self Time" column.

25-Sep-2006
Changed Platform-target from "Any CPU" to "x86".

21-Sep-2006
Improved symbol handling to always prefer .pdb file in same directory as .exe file.
Improved exception handling and diagnostic loggin in symbol handler.

18-Sep-2006
Added "Symbol library diagnostic output" for help in symbol loading issues.
Removed dependency requiring > Windows 2000 version of Kernel32.dll (AddVectoredExceptionHandler);

13-Sep-2006
Improved symbol handling for pre-VS2005 projects.

09-Sep-2006
Improved symbol handling updates.
Added flush to pipe.transact command.
Added support for GlowCode 5.x license keys.

15-Aug-2006
Added support to Preload Dll's.
Added support for SymbolServer.
Improved Attach options.
Improved FairSafety hook tests: test for priviledged instructions, code pointers.

04-Aug-2006
Settings files now stored in Windows "Application Data" directory.
Added Preload and Custom tab pages.
Fixed error in CallTree Avg Time column.

03-Aug-2006
Created additional online help documentation.
Added new callTree viewer columns.

31-Jul-2006
Improved native code hook safety.

26-Jul-2006
Fixed cause of hookSafety sometimes being stuck on HighSafety.

25-Jul-2006
Fixed potential failure in enumerating debug symbols.
Polished ProfilerWizard UI.

19-Jul-2006
Support for export from viewers. Access using right-click mouse context menu.
Support for column hide in viewers. Access using right-click mouse context menu.
Support for additional Native Memory resource tracking.

11-Jul-2006
Added online help documentation
Added support for native handle tracking such as GDI handles.
Added support for native handle and heap checkpoints.

02-Jul-2006
Fixed failure to support module names with embedded spaces

27-Jun-2006
Removed dependency on debug libraries that caused failure on machines without VS8 installed
Improved hooks editor behavior

20-Jun-2006
Beta released
