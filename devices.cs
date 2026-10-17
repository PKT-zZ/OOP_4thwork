/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class AirConditionRemote : TeachingDevice
{
    private const decimal BaseRate = 0.05m;

    public AirConditionRemote(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        int maxBorrowDays,
        decimal lateFeePerDay,
        DeviceStatus status = DeviceStatus.Available)
        : base(
            deviceId,
            deviceName,
            yearOfUse,
            purchasePrice,
            maxBorrowDays,
            lateFeePerDay,
            status)
    {
    }

    public override decimal CalculateAnnualMaintenanceCost() =>
        PurchasePrice * BaseRate;
}


public class Microphone : TeachingDevice
{
    private const decimal BaseRate = 0.04m;

    public Microphone(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        int maxBorrowDays,
        decimal lateFeePerDay,
        DeviceStatus status = DeviceStatus.Available)
        : base(
            deviceId,
            deviceName,
            yearOfUse,
            purchasePrice,
            maxBorrowDays,
            lateFeePerDay,
            status)
    {
    }

    public override decimal CalculateAnnualMaintenanceCost() =>
        PurchasePrice * BaseRate;
}


public class ProjectorRemote : TeachingDevice
{
    private const decimal BaseRate = 0.03m;

    public ProjectorRemote(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        int maxBorrowDays,
        decimal lateFeePerDay,
        DeviceStatus status = DeviceStatus.Available)
        : base(
            deviceId,
            deviceName,
            yearOfUse,
            purchasePrice,
            maxBorrowDays,
            lateFeePerDay,
            status)
    {
    }

    public override decimal CalculateAnnualMaintenanceCost() =>
        PurchasePrice * BaseRate;
}