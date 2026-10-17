/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class BorrowingException : Exception
{
    public BorrowingException(string message)
        : base(message)
    {
    }

    public BorrowingException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}


public class DeviceUnavailableException
    : BorrowingException
{
    public DeviceUnavailableException(
        string deviceId,
        DeviceStatus currentStatus)
        : base(
            $"Thiết bị {deviceId} không sẵn sàng. " +
            $"Trạng thái hiện tại: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }

    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }
}


public class BorrowLimitExceededException
    : BorrowingException
{
    public BorrowLimitExceededException(
        string borrowerId,
        int maximumDevices)
        : base(
            $"Người mượn {borrowerId} chỉ được mượn tối đa " +
            $"{maximumDevices} thiết bị trên một phiếu.")
    {
        BorrowerId = borrowerId;
        MaximumDevices = maximumDevices;
    }

    public string BorrowerId { get; }
    public int MaximumDevices { get; }
}


public class InvalidBorrowPeriodException
    : BorrowingException
{
    public InvalidBorrowPeriodException(
        string deviceId,
        int requestedDays,
        int maximumDays)
        : base(
            $"Thiết bị {deviceId}: thời hạn {requestedDays} ngày " +
            $"không hợp lệ, tối đa {maximumDays} ngày.")
    {
        DeviceId = deviceId;
        RequestedDays = requestedDays;
        MaximumDays = maximumDays;
    }

    public string DeviceId { get; }
    public int RequestedDays { get; }
    public int MaximumDays { get; }
}


public class DuplicateDeviceException
    : BorrowingException
{
    public DuplicateDeviceException(string deviceId)
        : base(
            $"Thiết bị {deviceId} đã tồn tại trong phiếu mượn.")
    {
    }
}


public class DeviceNotInSlipException
    : BorrowingException
{
    public DeviceNotInSlipException(string deviceId)
        : base(
            $"Thiết bị {deviceId} không thuộc phiếu mượn.")
    {
    }
}


public class DeviceAlreadyReturnedException
    : BorrowingException
{
    public DeviceAlreadyReturnedException(string deviceId)
        : base(
            $"Thiết bị {deviceId} đã được trả trước đó.")
    {
    }
}