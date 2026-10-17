/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

//phieu muon hop thanh cac BorrowItem
public class BorrowingSlip
{
    private readonly List<BorrowItem> _items = new();

    public BorrowingSlip(
        string slipId,
        IBorrower borrower,
        string teachingRoom,
        DateTime borrowedDate)
    {
        if (string.IsNullOrWhiteSpace(slipId))
            throw new ArgumentException(
                "Mã phiếu không được rỗng.",
                nameof(slipId));

        if (borrower == null)
            throw new ArgumentNullException(
                nameof(borrower));

        if (string.IsNullOrWhiteSpace(teachingRoom))
            throw new ArgumentException(
                "Phòng dạy không được rỗng.",
                nameof(teachingRoom));

        SlipId = slipId.Trim();
        Borrower = borrower;
        TeachingRoom = teachingRoom.Trim();
        BorrowedDate = borrowedDate;
    }

    public string SlipId { get; }
    public IBorrower Borrower { get; }
    public string TeachingRoom { get; }
    public DateTime BorrowedDate { get; }

    public IReadOnlyList<BorrowItem> Items =>
        _items;


    public void AddDevice(
        TeachingDevice device,
        int borrowDays)
    {
        ValidateBorrowing(
            device,
            borrowDays);

        var item = new BorrowItem(
            device,
            BorrowedDate,
            BorrowedDate.AddDays(borrowDays));

        device.MarkAsBorrowed();

        _items.Add(item);
    }


    //kiem tra het truoc khi thay doi trang thai
    private void ValidateBorrowing(
        TeachingDevice device,
        int borrowDays)
    {
        if (device == null)
            throw new ArgumentNullException(
                nameof(device));

        if (borrowDays <= 0
            || borrowDays > device.MaxBorrowDays)
        {
            throw new InvalidBorrowPeriodException(
                device.DeviceId,
                borrowDays,
                device.MaxBorrowDays);
        }

        if (_items.Any(
            i => i.Device.DeviceId == device.DeviceId))
        {
            throw new DuplicateDeviceException(
                device.DeviceId);
        }

        if (_items.Count
            >= Borrower.GetMaximumDevices())
        {
            throw new BorrowLimitExceededException(
                Borrower.BorrowerId,
                Borrower.GetMaximumDevices());
        }

        if (device.Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(
                device.DeviceId,
                device.Status);
        }
    }


    public decimal ReturnDevice(
        string deviceId,
        DateTime returnedDate)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException(
                "Mã thiết bị không được rỗng.",
                nameof(deviceId));

        BorrowItem? item =
            _items.FirstOrDefault(
                i => i.Device.DeviceId == deviceId);

        if (item == null)
            throw new DeviceNotInSlipException(
                deviceId);

        if (item.IsReturned)
            throw new DeviceAlreadyReturnedException(
                deviceId);

        if (returnedDate < item.BorrowedDate)
            throw new BorrowingException(
                "Ngày trả không được trước ngày mượn.");

        decimal lateFee =
            CalculateLateFee(
                item,
                returnedDate);

        item.Return(returnedDate);

        item.Device.MarkAsAvailable();

        return lateFee;
    }


    public decimal CalculateTotalLateFee() =>
        _items
            .Where(i => i.ReturnedDate.HasValue)
            .Sum(
                i => CalculateLateFee(
                    i,
                    i.ReturnedDate!.Value));


    private static decimal CalculateLateFee(
        BorrowItem item,
        DateTime returnedDate)
    {
        int lateDays =
            (returnedDate.Date
             - item.DueDate.Date).Days;

        return lateDays > 0
            ? lateDays * item.Device.LateFeePerDay
            : 0m;
    }
}