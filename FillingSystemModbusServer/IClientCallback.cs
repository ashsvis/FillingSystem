using System;
using System.ServiceModel;

namespace FillingSystemModbusServer
{
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void PropertyUpdated(DateTime servertime, string category, string pointname, string propname, string value);

    }
}
