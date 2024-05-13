

using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace TilerMain
{
    public static class BluetoothContent//蓝牙功能的静态方法，因为实例都是从InstanceGlobal获取的，这里全部标注为静态以简化调用
    {
        public static int SendData(string[] dataStrings)//数据发送,返回一个整数表示状态
        {
            Thread.Sleep(250);
            int errorCode = 0;
            List<byte> dataBytesList =[ 0xFF,];//设置数据包，初始化塞入包头

            foreach (string dataString in dataStrings)//将需要发送的数据贴为二进制放入数据包（并不是将字符串转为二进制）
            {
                byte data = byte.Parse(dataString);
                dataBytesList.Add(data);
            }
            dataBytesList.Add(0xFE);//设置包尾
            dataBytesList.Add(0xFD);
            byte[] packet = [.. dataBytesList];//列表转数组。
            try //尝试发送，出现错误将错误码设置为1，当然，未来可以细分不同的错误类型
            {
                InstanceGlobal.BluetoothStream.Write(packet);
            }
            catch (Exception)
            {
                errorCode = 1;
            }
            return errorCode;
        }

        public async static Task<int> SendDataAsync(string[] dataStrings)//发送数据的异步包装，看需要使用
        {
            int errorCode = 0;
            List<byte> dataBytesList = [0xFF];

            foreach (string dataString in dataStrings)
            {
                byte data = byte.Parse(dataString);
                dataBytesList.Add(data);
            }
            dataBytesList.Add(0xFE);
            dataBytesList.Add(0xFD);
            byte[] packet = [.. dataBytesList];
            try 
            {
                await InstanceGlobal.BluetoothStream.WriteAsync(packet);
            }
            catch (Exception)
            {
                errorCode = 1;
            }
            return errorCode;
        }
        public async static Task<byte[]> ReceiveDataAsync()
        {
            byte[] buffer = new byte[1];
            int bytesRead;
            List<byte> receivedData = [];
            try 
            {
                while (true) //等待包头
                {
                    bytesRead = await InstanceGlobal.BluetoothStream.ReadAsync(buffer);
                    if (bytesRead > 0 && buffer[0] == 0xFF) break;
                }
                while (true)
                {
                    bytesRead = await InstanceGlobal.BluetoothStream.ReadAsync(buffer);
                    if (bytesRead <= 0) continue;
                    receivedData.Add(buffer[0]);
                    if (receivedData.Count >= 3 && receivedData[^2] == 0xFE && receivedData[^1] == 0xFD)
                    {
                        receivedData.RemoveRange(receivedData.Count - 2, 2);
                        break;
                    }

                }
            } catch (Exception) 
            {
                receivedData.Clear();
            }
            return [.. receivedData];
        }

        public static async Task<bool> GetPakcageSucessful()
        {
            byte[] bytes = await ReceiveDataAsync();
            if (bytes[0] == 0xAA) { return true; }
            return false;
        }
    }

}
