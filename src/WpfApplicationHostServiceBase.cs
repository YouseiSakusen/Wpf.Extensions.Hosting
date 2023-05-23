using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Wpf.Extensions.Hosting;

public class WpfApplicationHostServiceBase<T> : IHostedService where T : Window
{
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		if (Application.Current.Windows.OfType<T>().Any()) return;

		var mainWindow = this.serviceProvider.GetRequiredService<T>();

		mainWindow.Closed += (sender, e) => this.appLifeTime.StopApplication();

		mainWindow.Show();

		await Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken)
		=> await Task.CompletedTask;

	private readonly IServiceProvider serviceProvider;
	private readonly IHostApplicationLifetime appLifeTime;

	public WpfApplicationHostServiceBase(IServiceProvider provider, IHostApplicationLifetime appHostLifetime)
	=> (this.serviceProvider, this.appLifeTime) = (provider, appHostLifetime);
}
