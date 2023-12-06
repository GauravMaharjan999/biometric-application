﻿using Attendance_ZKTeco_Service.Interfaces;
using Attendance_ZKTeco_Service.Models;
using AttendanceFetch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZkSoftwareEU;

namespace Attendance_ZKTeco_Service.Logics
{
    public class ZKTecoAttendance_DataFetchBL : IZKTecoAttendance_DataFetchBL
    {
        public ZKTecoAttendance_DataFetchBL()
        {

        }
        public async Task<DataResult<List<MachineInfo>>> GetData_Zkteco(AttendanceDevice model)
        {
            DeviceManipulator manipulator = new DeviceManipulator();
            try
            {
                //Validation
                if (model.IPAddress == string.Empty || model.Port <= 0)
                {
                    return new DataResult<List<MachineInfo>> { ResultType = ResultType.Failed, Message = "The Device IP Address or Port is mandotory !!" };
                }

                bool Isconnected = manipulator.Connect_device(model.IPAddress, model.Port);
                if (!Isconnected)
                {
                    return new DataResult<List<MachineInfo>> { ResultType = ResultType.Failed, Message = "Device not Connected!!" };
                }
                try
                {
                    List<MachineInfo> lstEnrollData = new List<MachineInfo>();

                    CZKEUEMNetClass machine1 = new CZKEUEMNetClass();
                    
                    List<MachineInfo> data = manipulator.GetLogData(model.DeviceMachineNo, model.IPAddress,model.Port);
                    if (data.Count > 0)
                    {
                        #region Push  Attendance Log Data to HR Server
                        //
                        //DataResult result = PushData(data, model.Id, model.StatusChgUserId, model.ClientAlias);
                        //if (result.ResultType == ResultType.Success)
                        //{
                        //    //manipulator.ClearGLog(model.DeviceMachineNo, model.IPAddress);
                        //    return new DataResult { ResultType = ResultType.Success, Message = "Data Pull and Push Success!!", dataRow = data };
                        //}
                        //else
                        //{
                        //    manipulator.Enable_Device(model.DeviceMachineNo);
                        //    manipulator.Disconnect();
                        //    return new DataResult { ResultType = ResultType.Failed, Message = "Data Push Failed on Application!!" };
                        //}
                        #endregion

                        return new DataResult<List<MachineInfo>> { ResultType = ResultType.Success, Message = "Data Retrived from Attendance Device Successfully!",Data=data.ToList() };
                    }
                    else
                    {
                        //manipulator.ClearGLog(model.DeviceMachineNo, model.IPAddress);
                        return new DataResult<List<MachineInfo>> { ResultType = ResultType.Failed, Message = "No any data found to pull from Attendance Device!!" };
                    }
                }
                catch (Exception ex)
                {
                    return new DataResult<List<MachineInfo>> { ResultType = ResultType.Failed, Message = $"Data pull failed from attendance device!! {ex.Message}" };
                };
            }
            catch (Exception ex)
            {
                return new DataResult<List<MachineInfo>> { ResultType = ResultType.Failed, Message = $"Device not connected!! {ex.Message}." };
            };
        }


        public async Task<DataResult> DeleteData_Zkteco(AttendanceDevice model)
        {
            DeviceManipulator manipulator = new DeviceManipulator();
            try
            {
                //Validation
                if (model.IPAddress == string.Empty || model.Port <= 0)
                {
                    return new DataResult { ResultType = ResultType.Failed, Message = "The Device IP Address or Port is mandotory !!" };
                }

                bool Isconnected = manipulator.Connect_device(model.IPAddress, model.Port);
                if (!Isconnected)
                {
                    return new DataResult { ResultType = ResultType.Failed, Message = "Device not Connected!!" };
                }
                try
                {

                    CZKEUEMNetClass machine1 = new CZKEUEMNetClass();

                    bool result  = manipulator.ClearLogData(model.DeviceMachineNo, model.IPAddress);
                    if (result==true)
                    {
                        return new DataResult { ResultType = ResultType.Success, Message = "Data Deleted from Attendance Device Successfully!"};
                    }
                    else
                    {
                        //manipulator.ClearGLog(model.DeviceMachineNo, model.IPAddress);
                        return new DataResult { ResultType = ResultType.Failed, Message = "No any data found to delete from Attendance Device!!" };
                    }
                }
                catch (Exception ex)
                {
                    return new DataResult { ResultType = ResultType.Exception, Message = $"Data delete failed from attendance device!! {ex.Message}" };
                };
            }
            catch (Exception ex)
            {
                return new DataResult { ResultType = ResultType.Failed, Message = $"Device not connected!! {ex.Message}." };
            };
        }




    }
}
