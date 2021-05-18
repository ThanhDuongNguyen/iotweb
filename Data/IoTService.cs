using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iotweb.Data
{
    public class IoTService
    {
        public event EventHandler<IoTDataPoint> InputMessageReceived;

        public async Task OnInputMessageReceived(IoTDataPoint IoTDataPoint)
        {
            await Task.Run(() => { InputMessageReceived?.Invoke(this, IoTDataPoint); });
        }

        public async Task SendTelemetry(IoTDataPoint IoTDataPoint)
        {
            if (IoTDataPoint != null)
            {
                await OnInputMessageReceived(IoTDataPoint);
            }
        }
    }
    public class IoTDataPoint
    {
      
        public int temperature { get; set; }
        public int humidity { get; set; }
    };
}
