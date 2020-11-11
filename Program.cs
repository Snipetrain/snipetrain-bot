using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using snipetrain_bot.Services;

namespace snipetrain_bot
{
    class Program
    {
        private DiscordRunner _runner;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
	        Console.WriteLine("Launching SnipeBot, connecting..."); 

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

	        Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            var servicesProvider = BuildDi(configuration);

            _runner = servicesProvider.GetRequiredService<DiscordRunner>();
            await _runner.StartClient(servicesProvider);

            Console.WriteLine("Press ANY key to exit");
            Console.ReadLine();
        }

        private IServiceProvider BuildDi(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddScoped<ISnipetrainService, SnipetrainService>();
            services.AddScoped<IStreamersService, StreamersService>();

            services.AddSingleton<ITwitchService, TwitchService>();
            services.AddSingleton<IConfiguration>(configuration);
            
            services.AddSingleton<DiscordRunner>();

            return services.BuildServiceProvider();
        }

        // On Shut down
        private async void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("\nShutting down SnipeBot, disconnecting Bot...");
	        await _runner.StopClient();
        }
        
    }
}
