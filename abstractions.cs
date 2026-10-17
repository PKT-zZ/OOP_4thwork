/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Globalization;

namespace DeviceManagement;

public enum DeviceStatus
{
    Available,
    Borrowed,
    UnderMaintenance
}

public interface IBorrower
{
    string BorrowerId { get; }
    string FullName { get; }

    int GetMaximumDevices();
}

//lop truu tuong chua phan chung cua moi thiet bi
public abstract class Device
{
    //nguong sdung >5y
    private const int MaintenanceAgeLimitYears = 5;

    protected Device(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        DeviceStatus status)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException(
                "Mã thiết bị không được rỗng.",
                nameof(deviceId));

        if (string.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentException(
                "Tên thiết bị không được rỗng.",
                nameof(deviceName));

        if (purchasePrice <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(purchasePrice),
                purchasePrice,
                "Giá mua phải lớn hơn 0.");

        int currentYear = DateTime.Now.Year;

        if (yearOfUse <= 0 || yearOfUse > currentYear)
            throw new ArgumentOutOfRangeException(
                nameof(yearOfUse),
                yearOfUse,
                $"Năm đưa vào sử dụng phải từ 1 đến {currentYear}.");

        //Ma thiet bi chi dc gan o day nen ko doi dc sau khi tao
        DeviceId = deviceId.Trim();
        DeviceName = deviceName.Trim();
        YearOfUse = yearOfUse;
        PurchasePrice = purchasePrice;
        Status = status;
    }

    public string DeviceId { get; }
    public string DeviceName { get; }
    public int YearOfUse { get; }
    public decimal PurchasePrice { get; }
    public DeviceStatus Status { get; protected set; }

    //So nam da sdung tinh theo nam hien tai
    public int YearsInUse =>
        DateTime.Now.Year - YearOfUse;

    public bool IsOlderThanFiveYears =>
        YearsInUse > MaintenanceAgeLimitYears;

    //Chi phi bao tri du kien trong 1 nam
    public abstract decimal CalculateAnnualMaintenanceCost();

    //dinh dang tien kieu 36.000.000 đ
    public static string FormatMoney(decimal amount) =>
        amount.ToString("N0", CultureInfo.InvariantCulture)
            .Replace(',', '.') + " đ";

    public override string ToString() =>
        $"[{DeviceId}] {GetType().Name} {DeviceName} | " +
        $"Năm {YearOfUse} ({YearsInUse} năm) | " +
        $"Giá mua {FormatMoney(PurchasePrice)} | {Status}";
}


//thiet bi dung cho nghiep vu muon tra
public abstract class TeachingDevice : Device
{
    protected TeachingDevice(
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
            status)
    {
        if (maxBorrowDays <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(maxBorrowDays),
                maxBorrowDays,
                "Số ngày mượn tối đa phải lớn hơn 0.");

        if (lateFeePerDay < 0)
            throw new ArgumentOutOfRangeException(
                nameof(lateFeePerDay),
                lateFeePerDay,
                "Phí trả muộn không được âm.");

        MaxBorrowDays = maxBorrowDays;
        LateFeePerDay = lateFeePerDay;
    }

    public int MaxBorrowDays { get; }
    public decimal LateFeePerDay { get; }

    public void MarkAsBorrowed()
    {
        Status = DeviceStatus.Borrowed;
    }

    public void MarkAsAvailable()
    {
        Status = DeviceStatus.Available;
    }

    public override string ToString() =>
        $"{base.ToString()} | Mượn tối đa {MaxBorrowDays} ngày";
}