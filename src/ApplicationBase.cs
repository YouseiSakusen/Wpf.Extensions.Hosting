using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Wpf.Extensions.Hosting;

public abstract class ApplicationBase : Application
{
	protected async override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		this.initializeHost();

		if (this.host == null) return;
		await this.host.StartAsync();
	}

	private IHost? host = null;

	private void initializeHost()
	{
		this.host = Host.CreateDefaultBuilder()
			.ConfigureServices((builderContext, services) =>
			{
				this.ConfigureServices(builderContext, services);
			})
			.Build();
	}

	protected abstract void ConfigureServices(HostBuilderContext builderContext, IServiceCollection services);

	protected async override void OnExit(ExitEventArgs e)
	{
		base.OnExit(e);

		if (this.host == null) return;
		await this.host.StopAsync();
		this.host.Dispose();
	}
}
