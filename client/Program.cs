using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using client;

var ip = IPAddress.Loopback;
var port = 4444;

var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);
Command command = null!;
string response = null!;
Console.WriteLine($"client started [{ip}:{port}]");
while (true)
{
    Console.WriteLine("Write command or HELP: ");

    var strtxt = Console.ReadLine()!.ToUpper();

    if (strtxt == "HELP")
    {

        Console.WriteLine();
        Console.WriteLine("Command List : ");
        Console.WriteLine(Command.ProcessList);
        Console.WriteLine($"{Command.Run} <process_name>");
        Console.WriteLine($"{Command.Kill} <process_name>");
        Console.WriteLine("HELP");
        Console.ReadLine();
        Console.Clear();
        continue;
    }

    var input = strtxt.Split(' ');
    switch (input[0])
    {
        case Command.ProcessList:
            command = new Command { Text = input[0] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            var processList = JsonSerializer.Deserialize<string[]>(response);
            foreach (var processName in processList!)
            {
                Console.WriteLine($"{processName}");
            }
            Console.ReadLine();
            Console.Clear();
            break;
        case Command.Run:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.ReadLine();
            break;

        case Command.Kill:
            command = new Command { Text = input[0], Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            Console.ReadLine();
            Console.Clear();
            break;
        default:
            break;
    }

}