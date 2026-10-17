/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

public class Lecturer : IBorrower
{
    public Lecturer(
        string borrowerId,
        string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
            throw new ArgumentException(
                "Mã người mượn không được rỗng.",
                nameof(borrowerId));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException(
                "Tên người mượn không được rỗng.",
                nameof(fullName));

        BorrowerId = borrowerId.Trim();
        FullName = fullName.Trim();
    }

    public string BorrowerId { get; }
    public string FullName { get; }

    public int GetMaximumDevices() => 3;

    public override string ToString() =>
        $"[{BorrowerId}] {FullName} - Giảng viên";
}


public class TeachingAssistant : IBorrower
{
    public TeachingAssistant(
        string borrowerId,
        string fullName)
    {
        if (string.IsNullOrWhiteSpace(borrowerId))
            throw new ArgumentException(
                "Mã người mượn không được rỗng.",
                nameof(borrowerId));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException(
                "Tên người mượn không được rỗng.",
                nameof(fullName));

        BorrowerId = borrowerId.Trim();
        FullName = fullName.Trim();
    }

    public string BorrowerId { get; }
    public string FullName { get; }

    public int GetMaximumDevices() => 2;

    public override string ToString() =>
        $"[{BorrowerId}] {FullName} - Trợ giảng";
}