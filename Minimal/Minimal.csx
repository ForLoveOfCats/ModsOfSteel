//Neither of these using statements are required for this minimal gamemode however any larger one will
using System;
using Godot;



private class CustomCommands
{
	private static Minimal Self; //To access the gamemode instance
	public CustomCommands(Minimal SelfArg)
	{
		Self = SelfArg;
	}


	public void Test() //Called in the console with Gm.Test()
	{
		Console.Print("Custom commands work!");
	}
}



public class Minimal : Gamemode //Gamemode inherits Godot.Node
{
	public override void _Ready() //Provided by Godot.Node
	{
		if(Net.Work.IsNetworkServer())
			Net.SteelRpc(Scripting.Self, nameof(Scripting.RequestGmLoad), Name); //Load same gamemode on all connected clients

		API.Gm = new CustomCommands(this);
	}


	public override void OnPlayerConnect(int Id)
	{
		if(Net.Work.IsNetworkServer())
			Scripting.Self.RpcId(Id, nameof(Scripting.RequestGmLoad), Name); //Load same gamemode on newly connected client
	}


	public override void OnUnload()
	{
		if(Net.Work.IsNetworkServer())
			Net.SteelRpc(Scripting.Self, nameof(Scripting.RequestGmUnload)); //Unload gamemode on all clients

		API.Gm = new API.EmptyCustomCommands(); //Could also set to null but this gives better error message when empty
	}
}


return new Minimal();
