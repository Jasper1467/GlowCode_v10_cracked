/*
To run the script, from the command line, type: cscript test.js 
Before running test.js for the first time, you must run GlowCodeRemoteCOM-install.bat from a "Visual Studio Command Prompt".
*/

var stdin = WScript.StdIn;
var stdout = WScript.StdOut;
var wshShell = WScript.CreateObject("WScript.Shell");
var bEnableWrite = false;

stdout.Write( "This simple script will perform an automated run of the GlowCode profiler.\n" )
stdout.Write( "It will look for or create a GlowCode project called notepad and try\nrunning c:\\windows\\system32\\notepad.exe.\n" )
stdout.Write( "It will insert some text in notepad and save it to the file test.txt.\n" )
stdout.Write( "It will start and stop profiling several times, and save the results of each profile segment in a separate file: test1, test2, test3.callExplorer.\n" )
stdout.Write( "Finally, it will exit GlowCode.\n\n" )
stdout.Write( "Before running test.js for the first time, you must run\nGlowCodeRemoteCOM-install.bat.\n\n" )

/*Start the GlowCode profiler with an argument to open a NamedPipe for commander input*/
var startCmd = "c:\\Program Files\\GlowCode 9.0\\glowcode.exe /cmd:NamedPipeCommander.Open";
stdout.WriteLine( "Exec " + startCmd );
var oGlowCode = wshShell.Exec( startCmd );
MyWaitActivate( oGlowCode.ProcessID );

/*GlowCode.RemoteCOM is basically a simple wrapper for NamedPipe calls*/
var GlowCodeRemote = WScript.CreateObject( "GlowCode.RemoteCOM" );
stdout.WriteLine( GlowCodeRemote.OpenPipe( ) )

/*Use the NamedPipe to talk with the GlowCode Profiling Controller. First, make it visible.*/
GlowCodeRemote.Execute( "ProfilingController.SetVisible true" ) 
/*Go to the first tab page in the GlowCode Profiling Controller, the "Open" page.*/
GlowCodeRemote.Execute( "ProfilingController.FirstTabPage" ) 
/*Refer to the previously-saved GlowCode project, called notepad.*/
GlowCodeRemote.Execute( "ProfilingController.SetProjectName notepad" )
/*Go to the next tab page in the GlowCode Profiling Controller, the "Target" page.*/ 
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" ) 

result = GlowCodeRemote.Execute( "ProfilingController.TargetNameExists" );
rex = /^True/m
if ( "True" != result.match( rex ) )
{
	GlowCodeRemote.Execute( "ProfilingController.SetTargetName c:\\windows\\system32\\notepad.exe" );
}
result = GlowCodeRemote.Execute( "ProfilingController.TargetNameExists" );
while ( "True" != result.match( rex ) )
{
	stdout.WriteLine( "\nGive a valid target name in the Profiling Controller Target name textbox\nThen type ENTER here to continue" );
	stdin.ReadLine();
	result = GlowCodeRemote.Execute( "ProfilingController.TargetNameExists" );
}

/*Go to the next tab page in the GlowCode Profiling Controller, the "Setup" page.*/ 
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" )
/*Go to the next tab page in the GlowCode Profiling Controller, the "Running" page.*/ 
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" )
/*Add hooks for dlls of interest.*/
GlowCodeRemote.Execute( "ProfilingController.CreateHookFile c:\\windows\\system32\\gdi32.dll" );
GlowCodeRemote.Execute( "ProfilingController.CreateHookFile c:\\windows\\system32\\user32.dll" );
/*Resume the run which had been suspended on the GlowCode Profiling Controller "Setup" page.*/
GlowCodeRemote.Execute( "ProfilingController.RunResume" ) 

/*Wait until target is the active window so we can send some keystrokes to it*/
result = GlowCodeRemote.Execute( "Target.RunningProcessId" );
stdout.WriteLine( result )
rex = /^[0-9]+/m
s = result.match( rex );
targetProcessId = parseInt( s );

MyWaitActivate( targetProcessId );
wshShell.SendKeys( "Hello 1 from test.js{ENTER}" );
wshShell.SendKeys( "%(fs)test.txt{ENTER}y" );

/*Export the profile results of the function summary to an csv-Excel file*/
GlowCodeRemote.Execute( "Viewer.CallTree.Export CallTreeFunctionSummary test.csv 2 true true" ) 

/*Save the profile results to a GlowCode .callExplorer file, then clear the data from the GlowCode viewer.*/
GlowCodeRemote.Execute( "Viewer.CallTree.SaveSelf test1.callExplorer" ) 
GlowCodeRemote.Execute( "Viewer.CallTree.Clear" ) 

/*Stop collecting GlowCode profile data, then send keystrokes to Notepad, then re-start GlowCode data collection.*/
GlowCodeRemote.Execute( "ProfilingController.CollectProfileData False" )
wshShell.SendKeys( "Hello 2 from test.js{ENTER}" );
wshShell.SendKeys( "%(fs)" );
GlowCodeRemote.Execute( "ProfilingController.CollectProfileData True" )

/*Save the profile results to another .callExplorer file, then clear the data from the GlowCode viewer.*/
GlowCodeRemote.Execute( "Viewer.CallTree.SaveSelf test2.callExplorer" ) 
GlowCodeRemote.Execute( "Viewer.CallTree.Clear" ) 

/*Set a GlowCode trigger, send keystrokes, save results to a third file, then clear the viewer.*/
GlowCodeRemote.Execute( "ProfilingController.SetTriggerFunction NOTEPAD!_NPWndProc@16" )
wshShell.SendKeys( "Hello 3 from test.js{ENTER}" );
wshShell.SendKeys( "%(fs)" );
GlowCodeRemote.Execute( "Viewer.CallTree.SaveSelf test3.callExplorer" ) 
GlowCodeRemote.Execute( "Viewer.CallTree.Clear" ) 

/*Remove the trigger*/
GlowCodeRemote.Execute( "ProfilingController.SetTriggerFunction <always>" )

stdout.WriteLine( "Type ENTER to finish this script" );
stdin.ReadLine();

/*Execute "next" on the GlowCode Profiling Controller's "Running," "Stop," then "Close" pages.*/
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" ) 
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" ) 
GlowCodeRemote.Execute( "ProfilingController.NextTabPage" ) 

//wshShell.Popup( "test.js Pause" )

GlowCodeRemote.Execute( "Main.ExitProgram" ) 
GlowCodeRemote.Close 


/*Wait until an application with TitleOrProcessId is successfully activated*/
function MyWaitActivate( TitleOrProcessId )
{
	stdout.Write("Activate: " + TitleOrProcessId )
	var WshShell = WScript.CreateObject("WScript.Shell")
	var bActivated = false
	while ( !bActivated )
	{
		bActivated = WshShell.AppActivate( TitleOrProcessId )
		stdout.Write( "." );
		WScript.Sleep( 500 )
	}
	stdout.WriteLine("");
}
	

function MyWriteLine( s )
{
     if ( bEnableWrite )
          MyWriteLine( s );
}