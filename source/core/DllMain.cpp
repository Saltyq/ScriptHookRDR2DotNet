bool sGameReloaded = false;

#pragma managed
#include <stdio.h>  /* defines FILENAME_MAX */
#define WINDOWS
#ifdef WINDOWS
#include <direct.h>
#define GetCurrentDir _getcwd
#endif
#include<iostream>

// Import C# code base
#using "ScriptHookRDRDotNet.netmodule"

using namespace System;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

[assembly:AssemblyTitleAttribute("Script Hook RDR2 .NET")];
[assembly:AssemblyCompanyAttribute("Salty, SHVDN: crosire & contributors")];
[assembly:AssemblyProductAttribute("ScriptHookRDRDotNet")];
[assembly:AssemblyDescriptionAttribute("An ASI plugin for Red Dead Redemption 2, which allows running scripts written in any .NET language in-game.")];
[assembly:AssemblyVersionAttribute("1.0.0.0")];
[assembly:AssemblyCopyrightAttribute("Copyright © 2015 crosire | Copyright © 2019 SaltyDev")];
// Sign with a strong name to distinguish from older versions and cause .NET framework runtime to bind the correct assemblies
// There is no version check performed for assemblies without strong names (https://docs.microsoft.com/en-us/dotnet/framework/deployment/how-the-runtime-locates-assemblies)
[assembly:AssemblyKeyFileAttribute("PublicKeyToken.snk")];

public ref class ScriptHookRDRDotNet
{
public:
	[RDR2DN::ConsoleCommand("Print the default help")]
	static void Help()
	{
		console->PrintInfo("~c~--- Help ---");
		console->PrintInfo("The console accepts ~h~C# expressions~h~ as input and has full access to the scripting API. To print the result of an expression, simply add \"return\" in front of it.");
		console->PrintInfo("You can use \"P\" as a shortcut for the player character and \"V\" for the current vehicle (without the quotes).");
		console->PrintInfo("Example: \"return P.IsInVehicle()\" will print a boolean value indicating whether the player is currently sitting in a vehicle to the console.");
		console->PrintInfo("~c~--- Commands ---");
		console->PrintHelpText();
	}
	[RDR2DN::ConsoleCommand("Print the help for a specific command")]
	static void Help(String^ command)
	{
		console->PrintHelpText(command);
	}

	[RDR2DN::ConsoleCommand("Clear the console history and pages")]
	static void Clear()
	{
		console->Clear();
	}

	[RDR2DN::ConsoleCommand("Reload all scripts from the scripts directory")]
	static void Reload()
	{
		//console->PrintInfo("~y~Reloading ...");

		// Force a reload on next tick
		sGameReloaded = true;
	}

	[RDR2DN::ConsoleCommand("Load scripts from a file")]
	static void Start(String^ filename)
	{
		if (!IO::Path::IsPathRooted(filename))
			filename = IO::Path::Combine(domain->ScriptPath, filename);
		if (!IO::Path::HasExtension(filename))
			filename += ".dll";

		String^ ext = IO::Path::GetExtension(filename)->ToLower();
		if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
			console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
			return;
		}

		domain->StartScripts(filename);
	}
	[RDR2DN::ConsoleCommand("Abort all scripts from a file")]
	static void Abort(String^ filename)
	{
		if (!IO::Path::IsPathRooted(filename))
			filename = IO::Path::Combine(domain->ScriptPath, filename);
		if (!IO::Path::HasExtension(filename))
			filename += ".dll";

		String^ ext = IO::Path::GetExtension(filename)->ToLower();
		if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
			console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
			return;
		}

		domain->AbortScripts(filename);
	}
	[RDR2DN::ConsoleCommand("Abort all scripts currently running")]
	static void AbortAll()
	{
		domain->Abort();

		console->PrintInfo("Stopped all running scripts. Use \"Start(filename)\" to start them again.");
	}

	[RDR2DN::ConsoleCommand("List all loaded scripts")]
	static void ListScripts()
	{
		console->PrintInfo("~c~--- Loaded Scripts ---");
		for each (auto script in domain->RunningScripts)
			console->PrintInfo(IO::Path::GetFileName(script->Filename) + " ~h~" + script->Name + (script->IsRunning ? (script->IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
	}

internal:
	static RDR2DN::Console^ console = nullptr;
	static RDR2DN::ScriptDomain^ domain = RDR2DN::ScriptDomain::CurrentDomain;
	static WinForms::Keys reloadKey = WinForms::Keys::None;
	static WinForms::Keys consoleKey = WinForms::Keys::F4;

	static void SetConsole()
	{
		console = (RDR2DN::Console^)AppDomain::CurrentDomain->GetData("Console");
	}

};

static void ScriptHookRDRDotNet_ManagedInit()
{
	RDR2DN::Console^% console = ScriptHookRDRDotNet::console;
	RDR2DN::ScriptDomain^% domain = ScriptHookRDRDotNet::domain;

	// Unload previous domain (this unloads all script assemblies too)
	if (domain != nullptr)
		RDR2DN::ScriptDomain::Unload(domain);

	// Clear log from previous runs
	RDR2DN::Log::Clear();

	// Load configuration
	String^ scriptPath = "scripts";

	try
	{
		array<String^>^ config = IO::File::ReadAllLines(IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));

		for each (String ^ line in config)
		{
			// Perform some very basic key/value parsing
			line = line->Trim();
			if (line->StartsWith("//"))
				continue;
			array<String^>^ data = line->Split('=');
			if (data->Length != 2)
				continue;

			if (data[0] == "ReloadKey")
				Enum::TryParse(data[1], true, ScriptHookRDRDotNet::reloadKey);
			else if (data[0] == "ConsoleKey")
				Enum::TryParse(data[1], true, ScriptHookRDRDotNet::consoleKey);
			else if (data[0] == "ScriptsLocation")
				scriptPath = data[1];
		}
		RDR2DN::Log::Message(RDR2DN::Log::Level::Info, "Config loaded from ", IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));

	}
	catch (Exception ^ ex)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "Failed to load config: ", ex->ToString());
	}

	// Create a separate script domain
	
	String^ directory = IO::Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location);
	domain = RDR2DN::ScriptDomain::Load(directory, scriptPath);
	if (domain == nullptr)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "return null on scriptdomain::load() in ", scriptPath);
		return;
	}


	// Console Stuff -- commented out until internal drawtext is solved
	/*try
	{
		// Instantiate console inside script domain, so that it can access the scripting API
		console = (RDR2DN::Console^)domain->AppDomain->CreateInstanceFromAndUnwrap(
			RDR2DN::Console::typeid->Assembly->Location, RDR2DN::Console::typeid->FullName);

		// Print welcome message
		console->PrintInfo("~c~--- Community Script Hook RDR2 .NET  ---");
		console->PrintInfo("~c~--- Type \"Help()\" to print an overview of available commands ---");

		// Update console pointer in script domain
		domain->AppDomain->SetData("Console", console);
		domain->AppDomain->DoCallBack(gcnew CrossAppDomainDelegate(&ScriptHookRDRDotNet::SetConsole));

		// Add default console commands
		console->RegisterCommands(ScriptHookRDRDotNet::typeid);
	}
	catch (Exception ^ ex)
	{
		RDR2DN::Log::Message(RDR2DN::Log::Level::Error, "Failed to create console: ", ex->ToString());
	}*/

	// Start scripts in the newly created domain
	domain->Start();
}

static void ScriptHookRDRDotNet_ManagedTick()
{
	RDR2DN::Console^ console = ScriptHookRDRDotNet::console;
	if (console != nullptr)
		console->DoTick();

	RDR2DN::ScriptDomain^ scriptdomain = ScriptHookRDRDotNet::domain;
	if (scriptdomain != nullptr)
		scriptdomain->DoTick();
}

static void ScriptHookRDRDotNet_ManagedKeyboardMessage(unsigned long keycode, bool keydown, bool ctrl, bool shift, bool alt)
{
	// Filter out invalid key codes
	if (keycode <= 0 || keycode >= 256)
		return;

	// Convert message into a key event
	auto keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	if (keydown && keys == ScriptHookRDRDotNet::reloadKey)
	{
		// Force a reload
		ScriptHookRDRDotNet::Reload();
		return;
	}

	RDR2DN::Console^ console = ScriptHookRDRDotNet::console;
	if (console != nullptr)
	{
		if (keydown && keys == ScriptHookRDRDotNet::consoleKey)
		{
			// Toggle open state
			console->IsOpen = !console->IsOpen;
			return;
		}

		// Send key events to console
		console->DoKeyEvent(keys, keydown);

		// Do not send keyboard events to other running scripts when console is open
		if (console->IsOpen)
			return;
	}

	RDR2DN::ScriptDomain^ scriptdomain = ScriptHookRDRDotNet::domain;
	if (scriptdomain != nullptr)
	{
		// Send key events to all scripts
		scriptdomain->DoKeyEvent(keys, keydown);
	}
}

#pragma unmanaged

#include <Main.h>
#include <Windows.h>
#include <WinBase.h>

PVOID sGameFiber = nullptr;
PVOID sScriptFiber = nullptr;

static void ScriptMain()
{
	sGameReloaded = true;

	// ScriptHookRDR2 already turned the current thread into a fiber, so we can safely retrieve it.
	sGameFiber = GetCurrentFiber();

	// Check if our CLR fiber already exists. It should be created only once for the entire lifetime of the game process.
	if (sScriptFiber == nullptr)
	{
		const LPFIBER_START_ROUTINE FiberMain = [](LPVOID lpFiberParameter) {
			// Main script execution loop
			while (true)
			{
				sGameReloaded = false;

				ScriptHookRDRDotNet_ManagedInit();

				// If the game is reloaded, ScriptHookRDR2 will call the script main function again.
				// This will set the global 'sGameReloaded' variable to 'true' and on the next fiber switch to our CLR fiber, run into this condition, therefore exiting the inner loop and re-initialize.
				while (!sGameReloaded)
				{
					ScriptHookRDRDotNet_ManagedTick();

					// Switch back to main script fiber used by ScriptHookRDR2.
					// Code continues from here the next time the loop below switches back to our CLR fiber.
					SwitchToFiber(sGameFiber);
				}
			}
		};

		// Create our own fiber for the common language runtime, aka CLR, once.
		// This is done because ScriptHookRDR2 switches its internal fibers sometimes, which would corrupt the CLR stack.
		sScriptFiber = CreateFiber(0, FiberMain, nullptr);
	}

	while (true)
	{
		// Yield execution and give it back to ScriptHookRDR2.
		scriptWait(0);

		// Switch to our CLR fiber and wait for it to switch back.
		SwitchToFiber(sScriptFiber);
	}
}

static void ScriptCleanup()
{
	DeleteFiber(sScriptFiber);
}

static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	ScriptHookRDRDotNet_ManagedKeyboardMessage(
		key,
		!isUpNow,
		(GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0,
		(GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0,
		isWithAlt != FALSE);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
	case DLL_PROCESS_ATTACH:
		// Avoid unnecessary DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications
		DisableThreadLibraryCalls(hModule);
		// Register ScriptHookRDRDotNet native script
		scriptRegister(hModule, ScriptMain);
		// Register handler for keyboard messages
		keyboardHandlerRegister(ScriptKeyboardMessage);
		break;
	case DLL_PROCESS_DETACH:
		ScriptCleanup();
		// Unregister ScriptHookRDRDotNet native script
		scriptUnregister(hModule);
		// Unregister handler for keyboard messages
		keyboardHandlerUnregister(ScriptKeyboardMessage);
		break;
	}

	return TRUE;
}


