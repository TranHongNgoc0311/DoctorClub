using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DoctorClub.Hubs
{
    public class OnlineCount:Hub
    {
		private static long count = 0;
		private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<OnlineCount>();

		// Overridable hub methods  
		public override Task OnConnected()
		{
			count++;
			hubContext.Clients.All.online(count);
			return base.OnConnected();
		}
		public override Task OnReconnected()
		{
			count++;
			hubContext.Clients.All.online(count);
			return base.OnReconnected();
		}
		public override Task OnDisconnected(bool stopCalled)
		{
			count--;
			hubContext.Clients.All.online(count);
			return base.OnDisconnected(stopCalled);
		}
	}
}