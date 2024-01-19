using System.IO.Ports;

namespace ArcadeLauncher.Services
{
    public class FirmwareService : BackgroundService
    {
        private readonly ILogger<FirmwareService> logger;
        private readonly Dictionary<string, Action<string>> ReadCommands;
        private readonly GameService gameService;
        public FirmwareService(ILogger<FirmwareService> logger, GameService service)
        {
            this.logger = logger;
            this.gameService = service;

            ReadCommands = new Dictionary<string, Action<string>>()
            {
                {"CLG", CloseGame }
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = new Task(() => ExecuteLoop(stoppingToken));
            task.Start();
            return task;
        }

        private void ExecuteLoop(CancellationToken token)
        {
            //return;
            SerialPort port = new SerialPort("COM3", 115200);

            try
            {

            port.Open();
            }
            catch
            {
                return;
            }
            port.ReadTimeout = 500;
            while (port.IsOpen && !token.IsCancellationRequested)
            {
                try
                {
                    string line = port.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                        HandleCommand(line);

                    else
                    {

                    }
                }
                catch
                {

                }

            }
        }

        private void HandleCommand(string command)
        {
            if (ReadCommands.TryGetValue(command.Split(" ")[0], out var action))
            {
                action(command);
            }
            else
            {
                logger.LogError($"command {command} not found");
            }

        }

        private void CloseGame(string cmd)
        {
            gameService.CloseGame();
        }
    }
}
