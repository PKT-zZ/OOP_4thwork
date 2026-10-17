/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

//mot muc muon chi tham chieu den thiet bi
public class BorrowItem
{
    public BorrowItem(
        TeachingDevice device,
        DateTime borrowedDate,
        DateTime dueDate)
    {
        Device = device
            ?? throw new ArgumentNullException(nameof(device));

        if (dueDate < borrowedDate)
            throw new ArgumentException(
                "Ngày đến hạn không được trước ngày mượn.",
                nameof(dueDate));

        BorrowedDate = borrowedDate;
        DueDate = dueDate;
    }

    public TeachingDevice Device { get; }
    public DateTime BorrowedDate { get; }
    public DateTime DueDate { get; }

    public DateTime? ReturnedDate
    {
        get;
        private set;
    }

    public bool IsReturned =>
        ReturnedDate.HasValue;

    public void Return(DateTime returnedDate)
    {
        if (IsReturned)
            throw new DeviceAlreadyReturnedException(
                Device.DeviceId);

        if (returnedDate < BorrowedDate)
            throw new BorrowingException(
                "Ngày trả không được trước ngày mượn.");

        ReturnedDate = returnedDate;
    }
}