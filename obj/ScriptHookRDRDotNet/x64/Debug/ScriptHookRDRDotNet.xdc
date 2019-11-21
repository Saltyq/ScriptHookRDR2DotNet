<?xml version="1.0"?>
<doc>
    <members>
        <member name="P:RDR2DN.Console.IsOpen">
            <summary>
            Gets or sets whether the console is open.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.RegisterCommand(RDR2DN.ConsoleCommand,System.Reflection.MethodInfo)">
            <summary>
            Register the specified method as a console command.
            </summary>
            <param name="command">The command attribute of the method.</param>
            <param name="methodInfo">The method information.</param>
        </member>
        <member name="M:RDR2DN.Console.RegisterCommands(System.Type)">
            <summary>
            Register all methods with a <see cref="T:RDR2DN.ConsoleCommand"/> attribute in the specified type as console commands.
            </summary>
            <param name="type">The type to search for console command methods.</param>
        </member>
        <member name="M:RDR2DN.Console.UnregisterCommands(System.Type)">
            <summary>
            Unregister all methods with a <see cref="T:RDR2DN.ConsoleCommand"/> attribute that were previously registered.
            </summary>
            <param name="type">The type to search for console command methods.</param>
        </member>
        <member name="M:RDR2DN.Console.AddLines(System.String,System.String[])">
            <summary>
            Add text lines to the console. This call is thread-safe.
            </summary>
            <param name="prefix">The prefix for each line.</param>
            <param name="messages">The lines to add to the console.</param>
        </member>
        <member name="M:RDR2DN.Console.AddLines(System.String,System.String[],System.String)">
            <summary>
            Add colored text lines to the console. This call is thread-safe.
            </summary>
            <param name="prefix">The prefix for each line.</param>
            <param name="messages">The lines to add to the console.</param>
            <param name="color">The color of those lines.</param>
        </member>
        <member name="M:RDR2DN.Console.AddToInput(System.String)">
            <summary>
            Add text to the console input line.
            </summary>
            <param name="text">The text to add.</param>
        </member>
        <member name="M:RDR2DN.Console.AddClipboardContent">
            <summary>
            Paste clipboard content into the console input line.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.ClearInput">
            <summary>
            Clear the console input line.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.Clear">
            <summary>
            Clears the console output.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.PrintInfo(System.String,System.Object[])">
            <summary>
            Writes an info message to the console.
            </summary>
            <param name="msg">The composite format string.</param>
            <param name="args">The formatting arguments.</param>
        </member>
        <member name="M:RDR2DN.Console.PrintError(System.String,System.Object[])">
            <summary>
            Writes an error message to the console.
            </summary>
            <param name="msg">The composite format string.</param>
            <param name="args">The formatting arguments.</param>
        </member>
        <member name="M:RDR2DN.Console.PrintWarning(System.String,System.Object[])">
            <summary>
            Writes a warning message to the console.
            </summary>
            <param name="msg">The composite format string.</param>
            <param name="args">The formatting arguments.</param>
        </member>
        <member name="M:RDR2DN.Console.PrintHelpText">
            <summary>
            Writes the help text for all commands to the console.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.PrintHelpText(System.String)">
            <summary>
            Writes the help text for the specified command to the console.
            </summary>
            <param name="commandName">The command name to check.</param>
        </member>
        <member name="M:RDR2DN.Console.DoTick">
            <summary>
            Main execution logic of the console.
            </summary>
        </member>
        <member name="M:RDR2DN.Console.DoKeyEvent(System.Windows.Forms.Keys,System.Boolean)">
            <summary>
            Keyboard handling logic of the console.
            </summary>
            <param name="keys">The key that was originated this event and its modifiers.</param>
            <param name="status"><c>true</c> on a key down, <c>false</c> on a key up event.</param>
        </member>
        <member name="T:RDR2DN.NativeFunc">
            <summary>
            Class responsible for executing script functions.
            </summary>
        </member>
        <member name="M:RDR2DN.NativeFunc.NativeInit(System.UInt64)">
            <summary>
            Initializes the stack for a new script function call.
            </summary>
            <param name="hash">The function hash to call.</param>
        </member>
        <member name="M:RDR2DN.NativeFunc.NativePush64(System.UInt64)">
            <summary>
            Pushes a function argument on the script function stack.
            </summary>
            <param name="val">The argument value.</param>
        </member>
        <member name="M:RDR2DN.NativeFunc.NativeCall">
            <summary>
            Executes the script function call.
            </summary>
            <returns>A pointer to the return value of the call.</returns>
        </member>
        <member name="T:RDR2DN.NativeFunc.NativeTask">
            <summary>
            Internal script task which holds all data necessary for a script function call.
            </summary>
        </member>
        <member name="M:RDR2DN.NativeFunc.PushString(System.String)">
            <summary>
            Pushes a single string component on the text stack.
            </summary>
            <param name="str">The string to push.</param>
        </member>
        <member name="M:RDR2DN.NativeFunc.ConvertPrimitiveArguments(System.Object[])">
            <summary>
            Helper function that converts an array of primitive values to a native stack.
            </summary>
            <param name="args"></param>
            <returns></returns>
        </member>
        <member name="M:RDR2DN.NativeFunc.Invoke(System.UInt64,System.UInt64[])">
            <summary>
            Executes a script function inside the current script domain.
            </summary>
            <param name="hash">The function has to call.</param>
            <param name="args">A list of function arguments.</param>
            <returns>A pointer to the return value of the call.</returns>
        </member>
        <member name="M:RDR2DN.NativeFunc.InvokeInternal(System.UInt64,System.UInt64[])">
            <summary>
            Executes a script function immediately. This may only be called from the main script domain thread.
            </summary>
            <param name="hash">The function has to call.</param>
            <param name="args">A list of function arguments.</param>
            <returns>A pointer to the return value of the call.</returns>
        </member>
        <member name="T:RDR2DN.NativeMemory">
            <summary>
            Class responsible for managing all access to game memory.
            </summary>
        </member>
        <member name="M:RDR2DN.NativeMemory.CreateTexture(System.String)">
            <summary>
            Creates a texture. Texture deletion is performed automatically when game reloads scripts.
            Can be called only in the same thread as natives.
            </summary>
            <param name="filename"></param>
            <returns>Internal texture ID.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.DrawTexture(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)">
            <summary>
            Draws a texture on screen. Can be called only in the same thread as natives.
            </summary>
            <param name="id">Texture ID returned by <see cref="M:RDR2DN.NativeMemory.CreateTexture(System.String)"/>.</param>
            <param name="instance">The instance index. Each texture can have up to 64 different instances on screen at a time.</param>
            <param name="level">Texture instance with low levels draw first.</param>
            <param name="time">How long in milliseconds the texture instance should stay on screen.</param>
            <param name="sizeX">Width in screen space [0,1].</param>
            <param name="sizeY">Height in screen space [0,1].</param>
            <param name="centerX">Center position in texture space [0,1].</param>
            <param name="centerY">Center position in texture space [0,1].</param>
            <param name="posX">Position in screen space [0,1].</param>
            <param name="posY">Position in screen space [0,1].</param>
            <param name="rotation">Normalized rotation [0,1].</param>
            <param name="scaleFactor">Screen aspect ratio, used for size correction.</param>
            <param name="colorR">Red tint.</param>
            <param name="colorG">Green tint.</param>
            <param name="colorB">Blue tint.</param>
            <param name="colorA">Alpha value.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.GetGameVersion">
            <summary>
            Gets the game version enumeration value as specified by ScriptHookRDR2.
            </summary>
        </member>
        <member name="M:RDR2DN.NativeMemory.GetGlobalPtr(System.Int32)">
            <summary>
            Returns pointer to a global variable. IDs may differ between game versions.
            </summary>
            <param name="index">The variable ID to query.</param>
            <returns>Pointer to the variable, or <see cref="F:System.IntPtr.Zero"/> if it does not exist.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.FindPattern(System.String,System.String)">
            <summary>
            Searches the address space of the current process for a memory pattern.
            </summary>
            <param name="pattern">The pattern.</param>
            <param name="mask">The pattern mask.</param>
            <returns>The address of a region matching the pattern or <c>null</c> if none was found.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.#cctor">
            <summary>
            Initializes all known functions and offsets based on pattern searching.
            </summary>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadByte(System.IntPtr)">
            <summary>
            Reads a single 8-bit value from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The value at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadInt16(System.IntPtr)">
            <summary>
            Reads a single 16-bit value from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The value at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadInt32(System.IntPtr)">
            <summary>
            Reads a single 32-bit value from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The value at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadFloat(System.IntPtr)">
            <summary>
            Reads a single floating-point value from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The value at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadString(System.IntPtr)">
            <summary>
            Reads a null-terminated UTF-8 string from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The string at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadAddress(System.IntPtr)">
            <summary>
            Reads a single 64-bit value from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>The value at the address.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadMatrix(System.IntPtr)">
            <summary>
            Reads a 4x4 floating-point matrix from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>All elements of the matrix in row major arrangement.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.ReadVector3(System.IntPtr)">
            <summary>
            Reads a 3-component floating-point vector from the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <returns>All elements of the vector.</returns>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteByte(System.IntPtr,System.Byte)">
            <summary>
            Writes a single 8-bit value to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The value to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteInt16(System.IntPtr,System.Int16)">
            <summary>
            Writes a single 16-bit value to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The value to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteInt32(System.IntPtr,System.Int32)">
            <summary>
            Writes a single 32-bit value to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The value to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteFloat(System.IntPtr,System.Single)">
            <summary>
            Writes a single floating-point value to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The value to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteMatrix(System.IntPtr,System.Single[])">
            <summary>
            Writes a 4x4 floating-point matrix to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The elements of the matrix in row major arrangement to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.WriteVector3(System.IntPtr,System.Single[])">
            <summary>
            Writes a 3-component floating-point to the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="value">The vector components to write.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.SetBit(System.IntPtr,System.Int32)">
            <summary>
            Sets a single bit in the 32-bit value at the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="bit">The bit index to change.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.ClearBit(System.IntPtr,System.Int32)">
            <summary>
            Clears a single bit in the 32-bit value at the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="bit">The bit index to change.</param>
        </member>
        <member name="M:RDR2DN.NativeMemory.IsBitSet(System.IntPtr,System.Int32)">
            <summary>
            Checks a single bit in the 32-bit value at the specified <paramref name="address"/>.
            </summary>
            <param name="address">The memory address to access.</param>
            <param name="bit">The bit index to check.</param>
            <returns><c>true</c> if the bit is set, <c>false</c> if it is unset.</returns>
        </member>
        <member name="P:RDR2DN.Script.Interval">
            <summary>
            Gets or sets the interval in ms between each <see cref="E:RDR2DN.Script.Tick"/>.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.IsPaused">
            <summary>
            Gets whether executing of this script is paused or not.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.IsRunning">
            <summary>
            Gets the status of this script.
            So <c>true</c> if it is running and <c>false</c> if it was aborted.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.IsExecuting">
            <summary>
            Gets whether this is the currently executing script.
            </summary>
        </member>
        <member name="E:RDR2DN.Script.Tick">
            <summary>
            An event that is raised every tick of the script.
            </summary>
        </member>
        <member name="E:RDR2DN.Script.Aborted">
            <summary>
            An event that is raised when this script gets aborted for any reason.
            </summary>
        </member>
        <member name="E:RDR2DN.Script.KeyUp">
            <summary>
            An event that is raised when a key is lifted.
            </summary>
        </member>
        <member name="E:RDR2DN.Script.KeyDown">
            <summary>
            An event that is raised when a key is first pressed.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.Name">
            <summary>
            Gets the instance name of this script.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.Filename">
            <summary>
            Gets the path to the file that contains this script.
            </summary>
        </member>
        <member name="P:RDR2DN.Script.ScriptInstance">
            <summary>
            Gets the instance of the script.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.MainLoop">
            <summary>
            The main execution logic of all scripts.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.Start">
            <summary>
            Starts execution of this script.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.Abort">
            <summary>
            Aborts execution of this script.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.Pause">
            <summary>
            Pauses execution of this script.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.Resume">
            <summary>
            Resumes execution of this script.
            </summary>
        </member>
        <member name="M:RDR2DN.Script.Wait(System.Int32)">
            <summary>
            Pause execution of this script for the specified time.
            </summary>
            <param name="ms">The time in milliseconds to pause.</param>
        </member>
        <member name="P:RDR2DN.ScriptDomain.Name">
            <summary>
            Gets the friendly name of this script domain.
            </summary>
        </member>
        <member name="P:RDR2DN.ScriptDomain.ScriptPath">
            <summary>
            Gets the path to the directory containing scripts.
            </summary>
        </member>
        <member name="P:RDR2DN.ScriptDomain.AppDomain">
            <summary>
            Gets the application domain that is associated with this script domain.
            </summary>
        </member>
        <member name="P:RDR2DN.ScriptDomain.CurrentDomain">
            <summary>
            Gets the scripting domain for the current application domain.
            </summary>
        </member>
        <member name="P:RDR2DN.ScriptDomain.RunningScripts">
            <summary>
            Gets the list of currently running scripts in this script domain. This is used by the console implementation.
            </summary>
        </member>
        <member name="P:RDR2DN.ScriptDomain.ExecutingScript">
            <summary>
            Gets the currently executing script or <c>null</c> if there is none.
            </summary>
        </member>
        <member name="M:RDR2DN.ScriptDomain.#ctor(System.String)">
            <summary>
            Initializes the script domain inside its application domain.
            </summary>
            <param name="apiBasePath">The path to the root directory containing the scripting API assemblies.</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.Unload(RDR2DN.ScriptDomain)">
            <summary>
            Unloads scripts and destroys an existing script domain.
            </summary>
            <param name="domain">The script domain to unload.</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.Load(System.String,System.String)">
            <summary>
            Creates a new script domain.
            </summary>
            <param name="basePath">The path to the application root directory.</param>
            <param name="scriptPath">The path to the directory containing scripts.</param>
            <returns>The script domain or <c>null</c> in case of failure.</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.LoadScriptsFromSource(System.String)">
            <summary>
            Compiles and load scripts from a C# or VB.NET source code file.
            </summary>
            <param name="filename">The path to the code file to load.</param>
            <returns><c>true</c> on success, <c>false</c> otherwise</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.LoadScriptsFromAssembly(System.String)">
            <summary>
            Loads scripts from the specified assembly file.
            </summary>
            <param name="filename">The path to the assembly file to load.</param>
            <returns><c>true</c> on success, <c>false</c> otherwise</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.LoadScriptsFromAssembly(System.Reflection.Assembly,System.String)">
            <summary>
            Loads scripts from the specified assembly object.
            </summary>
            <param name="filename">The path to the file associated with this assembly.</param>
            <param name="assembly">The assembly to load.</param>
            <returns><c>true</c> on success, <c>false</c> otherwise</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.InstantiateScript(System.Type)">
            <summary>
            Creates an instance of a script.
            </summary>
            <param name="scriptType">The type of the script to instantiate.</param>
            <returns>The script instance or <c>null</c> in case of failure.</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.Start">
            <summary>
            Loads and starts all scripts.
            </summary>
        </member>
        <member name="M:RDR2DN.ScriptDomain.StartScripts(System.String)">
            <summary>
            Loads and starts all scripts in the specified file.
            </summary>
            <param name="filename"></param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.Abort">
            <summary>
            Aborts all running scripts.
            </summary>
        </member>
        <member name="M:RDR2DN.ScriptDomain.AbortScripts(System.String)">
            <summary>
            Aborts all running scripts from the specified file.
            </summary>
            <param name="filename"></param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.ExecuteTask(RDR2DN.IScriptTask)">
            <summary>
            Execute a script task in this script domain.
            </summary>
            <param name="task">The task to execute.</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.IsKeyPressed(System.Windows.Forms.Keys)">
            <summary>
            Gets the key down status of the specified key.
            </summary>
            <param name="key">The key to check.</param>
            <returns><c>true</c> if the key is currently pressed or <c>false</c> otherwise</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.PauseKeyEvents(System.Boolean)">
            <summary>
            Pauses or resumes handling of keyboard events in this script domain.
            </summary>
            <param name="pause"><c>true</c> to pause or <c>false</c> to resume</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.DoTick">
            <summary>
            Main execution logic of the script domain.
            </summary>
        </member>
        <member name="M:RDR2DN.ScriptDomain.DoKeyEvent(System.Windows.Forms.Keys,System.Boolean)">
            <summary>
            Keyboard handling logic of the script domain.
            </summary>
            <param name="keys">The key that was originated this event and its modifiers.</param>
            <param name="status"><c>true</c> on a key down, <c>false</c> on a key up event.</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.CleanupStrings">
            <summary>
            Free memory for all pinned strings.
            </summary>
        </member>
        <member name="M:RDR2DN.ScriptDomain.PinString(System.String)">
            <summary>
            Pins the memory of a string so that it can be used in native calls without worrying about the GC invalidating its pointer.
            </summary>
            <param name="str">The string to pin to a fixed pointer.</param>
            <returns>A pointer to the pinned memory containing the string.</returns>
        </member>
        <member name="M:RDR2DN.ScriptDomain.LookupScript(System.Object)">
            <summary>
            Finds the script object representing the specified <paramref name="scriptInstance"/> object.
            </summary>
            <param name="scriptInstance">The 'RDR2.Script' instance to check.</param>
        </member>
        <member name="M:RDR2DN.ScriptDomain.GetScriptAttribute(System.Type,System.String)">
            <summary>
            Checks if the script has a 'GTA.ScriptAttributes' attribute with the specified argument attached to it and returns it.
            </summary>
            <param name="scriptType">The script type to check for the attribute.</param>
            <param name="name">The named argument to search.</param>
        </member>
    </members>
</doc>
